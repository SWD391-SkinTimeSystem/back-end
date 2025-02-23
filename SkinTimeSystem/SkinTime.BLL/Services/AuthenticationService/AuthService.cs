using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.AuthenticationService
{
    public class AuthService: IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenUtilities _tokenUtils;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, ITokenUtilities tokenUtilities, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _tokenUtils = tokenUtilities;
            _configuration = configuration;
        }

        public async Task<ServiceResult<User>> CreateUserWithGoogleWebToken(string token)
        {
            var repository = _unitOfWork.Repository<User>();

            var Validation = new GoogleJsonWebSignature.ValidationSettings { };

            GoogleJsonWebSignature.Payload data = await GoogleJsonWebSignature.ValidateAsync(token, Validation);

            User userInformation = new()
            {
                Username = data.Email,
                FullName = data.Name,
                Avatar = data.Picture,
                Gender = DAL.Enum.Gender.Other,
                Phone = "",
                Password = _tokenUtils.HashPassword(data.Email), // Unsecured, requires user to take action after login !
                Email = data.Email,
                Role = DAL.Enum.UserRole.Customer,
                Status = DAL.Enum.UserStatus.Active, // Automatically verify user if sign in using google account ?
            };
            userInformation.Password = _tokenUtils.HashPassword(userInformation.Password); 

            userInformation = await repository.AddAsync(userInformation);

            await _unitOfWork.Complete();

            return ServiceResult<User>.Success(userInformation);
        }

        public async Task<ServiceResult<User>> GetUserWithCredential(string account, string password)
        {
            /******* Data Retrieval *******/
            var userAccount = await _unitOfWork.Repository<User>().FindAsync(x => x.Username == account || x.Email == account);

            if (userAccount == null)
            {
                return ServiceResult<User>.Failed(ServiceError.ValidationFailed("Wrong login credential given"));
            }

            /******* Data Processing ******/

            // Convert the stored hashed passsword into byte array in order to extract the salt used when created.
            byte[] userHashedPassword = Convert.FromBase64String(userAccount.Password);

            byte[] saltBytes = new byte[16];
            Array.Copy(userHashedPassword, 0, saltBytes, 0, 16);

            // Regenerating the password hash with the given password.
            Rfc2898DeriveBytes hashingFunction = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
            byte[] hashedPasswordBytes = hashingFunction.GetBytes(40);

            byte[] recreatedHash = new byte[saltBytes.Length + hashedPasswordBytes.Length];
            Array.Copy(saltBytes, 0, recreatedHash, 0, 16);
            Array.Copy(hashedPasswordBytes, 0, recreatedHash, 16, hashedPasswordBytes.Length);

            // Check if the recreated hash is the same as the password hash (in base 64 string).
            if (Convert.ToBase64String(recreatedHash) == userAccount.Password)
            {
                return ServiceResult<User>.Success(userAccount);
            }
            return ServiceResult<User>.Failed(ServiceError.ValidationFailed("Wrong login credential given"));
        }

        public async Task<ServiceResult<User>> GetUserWithGoogleWebToken(string token)
        {
            try
            {
                var Validation = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string>{_configuration.GetSection("GoogleAuth:ClientId").Value!},
                };

                GoogleJsonWebSignature.Payload data = await GoogleJsonWebSignature.ValidateAsync(token, Validation);

                var userInfo = await _unitOfWork.Repository<User>().GetByConditionAsync(x => x.Email == data.Email);

                if (userInfo == null) 
                {
                    return ServiceResult<User>.Failed(ServiceError.NotFound(""));
                }

                return ServiceResult<User>.Success(userInfo);
            }
            catch (InvalidJwtException exception)
            {
                return ServiceResult<User>.Failed(ServiceError.ValidationFailed(exception.Message));
            }
        }

        public async Task<ServiceResult> VerifyUserAccount(string id)
        {
            if (Guid.TryParse(id, out var userId))
            {
                var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);

                if (user != null && user.Status == DAL.Enum.UserStatus.Inactive)
                {
                    user.Status = DAL.Enum.UserStatus.Active;
                    _unitOfWork.Repository<User>().Update(user);
                    await _unitOfWork.Complete();
                    return ServiceResult.Success("Successfully verified user account.");
                }
            }

            return ServiceResult.Failed(ServiceError.ValidationFailed("Can not find action linked to this token"));
        }
    }
}
