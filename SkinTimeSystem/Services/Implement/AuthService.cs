using Azure.Core;
using BusinessObject.Entities;
using BusinessObject.Enum;
using Castle.Core.Smtp;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Authentication;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenUtilities _tokenUtils;
        private readonly IConfiguration _configuration;
        private readonly IEmailUtilities _emailUtils;

        public AuthService(IUnitOfWork unitOfWork, ITokenUtilities tokenUtilities, IEmailUtilities emailUtilities, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _tokenUtils = tokenUtilities;
            _configuration = configuration;
            _emailUtils = emailUtilities;
        }

        public async Task<ServiceResult> AuthenUserWithCredential(string account, string password)
        {
            ServiceResult result = await GetUserWithCredential(account, password);

            if (result.IsFailed)
            {
                return result;
            }

            Dictionary<string, string> userObject = new Dictionary<string, string>
                    {
                        {"id", (result.Data as User)!.Id.ToString()},
                        {"role", (result.Data as User)!.Role.ToString() },
                    };

            AuthenticationTokens tokens = new()
            {
                AccessToken = _tokenUtils.CreateJwtFromDictionary(userObject),
                RefreshToken = _tokenUtils.CreateBase64RefreshToken(userObject["id"])
            };

            return ServiceResult<AuthenticationTokens>.Success(tokens);

        }

        public async Task<ServiceResult> AuthenUserWithGoogleWebToken(string token)
        {
            ServiceResult result = await GetUserWithGoogleWebToken(token);

            if (result.IsFailed)
            {
                if (result.Error.Code == ServiceError._NotFound)
                {
                    result =  await CreateUserWithGoogleWebToken(token);

                    if (result.IsFailed)
                    {
                        return result;
                    }

                    var content = File.ReadAllText(".\\StaticResoucres\\register_email.html");
                    await _emailUtils.SendGoogleEmailAsync((result.Data as User)!.Email, "SkinTime - New Registration Notice", content.Replace("[0]", (result.Data as User)!.FullName));
                }
                else
                {
                    return result;
                }
            }

            IDictionary<string, string> userData = new Dictionary<string, string>()
            {
                {"id", (result.Data as User)!.Id.ToString() },
                {"role", (result.Data as User)!.Role.ToString()}
            };

            AuthenticationTokens tokens = new AuthenticationTokens
            {
                AccessToken = _tokenUtils.CreateJwtFromDictionary(userData),
                RefreshToken = _tokenUtils.CreateBase64RefreshToken(userData["id"])
            };

            return ServiceResult.Success(tokens);
        }

        public async Task<ServiceResult> VerifyUserAccount(string id)
        {
            if (Guid.TryParse(id, out var userId))
            {
                var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);

                if (user != null && user.Status == UserStatus.Inactive)
                {
                    user.Status = UserStatus.Active;
                    _unitOfWork.Repository<User>().Update(user);
                    await _unitOfWork.Complete();
                    return ServiceResult.Success("Successfully verified user account.");
                }
            }

            return ServiceResult.Failed(ServiceError.ValidationFailed("Can not find action linked to this token"));
        }

        public async Task<ServiceResult> RegenerateToken(AuthenticationTokens tokens)
        {
            // Validate refreshtoken.
            string? userIdString = _tokenUtils.ValidateBase64RefreshToken(tokens.RefreshToken);

            if (userIdString == null)
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("invalid refresh token"));
            }

            // Get data from old token.
            Dictionary<string, string> oldInformation = _tokenUtils.GetDataDictionaryFromJwt(tokens.AccessToken);

            if (oldInformation.TryGetValue("error", out var error))
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed(error));
            }

            return ServiceResult<AuthenticationTokens>.Success(new()
            {
                AccessToken = _tokenUtils.CreateJwtFromDictionary(oldInformation),
                RefreshToken = _tokenUtils.CreateBase64RefreshToken(userIdString)
            });
        }

        private async Task<ServiceResult<User>> CreateUserWithGoogleWebToken(string token)
        {
            var repository = _unitOfWork.Repository<User>();

            var Validation = new GoogleJsonWebSignature.ValidationSettings { };

            GoogleJsonWebSignature.Payload data;
            try
            {
                data = await GoogleJsonWebSignature.ValidateAsync(token, Validation);
            }
            catch (InvalidJwtException)
            {
                return ServiceResult<User>.Failed(ServiceError.ValidationFailed("Invalid token."));
            }
            
            User userInformation = new()
            {
                Id = Guid.NewGuid(),
                Username = data.Email,
                FullName = data.Name,
                Avatar = data.Picture,
                Gender = Gender.Other,
                Phone = "",
                Password = _tokenUtils.HashPassword(data.Email), // Unsecured, requires user to take action after login !
                Email = data.Email,
                Role = UserRole.Customer,
                Status = UserStatus.Active, // Automatically verify user if sign in using google account ?
            };
            userInformation.Password = _tokenUtils.HashPassword(userInformation.Password);

            userInformation = await repository.AddAsync(userInformation);

            await _unitOfWork.Complete();

            return ServiceResult<User>.Success(userInformation);
        }

        private async Task<ServiceResult<User>> GetUserWithCredential(string account, string password)
        {
            /******* Data Retrieval *******/
            var userAccount = await _unitOfWork.Repository<User>().FindAsync(x => x.Username == account || x.Email == account);

            if (userAccount == null)
            {
                return ServiceResult<User>.Failed(ServiceError.ValidationFailed("No account match for given credentials"));
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
            return ServiceResult<User>.Failed(ServiceError.ValidationFailed("No account match for given credentials"));
        }

        private async Task<ServiceResult<User>> GetUserWithGoogleWebToken(string token)
        {
            try
            {
                var Validation = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _configuration.GetSection("GoogleAuth:ClientId").Value! },
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

    }
}
