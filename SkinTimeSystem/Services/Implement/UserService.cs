using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.User;
using Services.Commons.DTOs.Users;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenUtilities _tokenUtils;
        private readonly IEmailUtilities _emailUtils;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, ITokenUtilities tokenUtilities, IMapper mapper, IEmailUtilities emailUtilities)
        {
            _unitOfWork = unitOfWork;
            _tokenUtils = tokenUtilities;
            _emailUtils = emailUtilities;
            _mapper = mapper;
        }

        private async Task<ServiceResult<User>> CreateUserAccount(User userInformation)
        {
            var repository = _unitOfWork.Repository<User>();

            /******* Data Validation ********/

            // Check for new email address.
            if (repository.Find(user => user.Email == userInformation.Email) != null)
            {
                return ServiceResult<User>
                    .Failed(ServiceError
                        .ValidationFailed("An exisitng account using this email has been registered"));
            }

            // Check for existing username.
            if (repository.Find(user => user.Username == userInformation.Username) != null)
            {
                return ServiceResult<User>
                    .Failed(ServiceError.ValidationFailed("The username has been taken"));
            }

            // Validate password.
            Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            if (!passwordRegex.IsMatch(userInformation.Password))
            {
                return ServiceResult<User>
                    .Failed(ServiceError
                        .ValidationFailed("The password must has 8 character minimum, containing at least one letter and one number"));
            }

            /******* Data Processing ********/

            userInformation.Password = _tokenUtils.HashPassword(userInformation.Password); // Set user password using the newly created hashed password string.
            /******* Data Storage ********/

            userInformation = await repository.AddAsync(userInformation);
            await _unitOfWork.Complete();

            return ServiceResult<User>.Success(userInformation);
        }

        public async Task<PaginationResult<AccountInformation>> GetAllUser(int page, int page_size)
        {
            var result = await _unitOfWork.UserRepository.AsPaginated(page, page_size);

            ICollection<AccountInformation> information = _mapper.Map<ICollection<AccountInformation>>(result.Content);

            return new PaginationResult<AccountInformation>
            {
                Content = information,
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = page_size,
            };
        }

        public async Task<PaginationResult<AccountInformation>> GetAllUser(int page, int page_size, UserStatus status)
        {
            var result = await _unitOfWork.UserRepository.GetUserWithStatusPaginated(page, page_size, status, x => x.OrderByDescending(x => x.CreatedTime));

            ICollection<AccountInformation> information = _mapper.Map<ICollection<AccountInformation>>(result.Content);

            return new PaginationResult<AccountInformation>
            {
                Content = information,
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = page_size,
            };
        }

        public async Task<ServiceResult> GetUserById(Guid id)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);

            if (user != null)
            {
                return ServiceResult.Success(_mapper.Map<AccountInformation>(user));
            }
            return ServiceResult.Failed(ServiceError.NotFound("Can not find the user entity with provided id."));
        }

        public async Task<ServiceResult> CreateAccount(AccountRegistration account)
        {
            User userInformation = _mapper.Map<User>(account);
            return await CreateUserAccount(userInformation);
        }

        public async Task<ServiceResult> CreateCustomerAccount(CustomerRegistration account)
        {
            ServiceResult<User> result = await CreateUserAccount(_mapper.Map<User>(account));

            if (result.IsFailed)
            {
                return result;
            }

            var content = System.IO.File.ReadAllText(".\\StaticResoucres\\register_email.html");
            await _emailUtils.SendGoogleEmailAsync(account.Email, "SkinTime - New Registration Notice", content.Replace("[0]", account.Fullname));

            return ServiceResult.Success(result.Data!.Id);
        }

        public async Task<ServiceResult> UpdateUserInformation(Guid id, AccountUpdateInformation account)
        {
            var existingUser = _unitOfWork.UserRepository.GetById(id);

            if (existingUser == null)
            {
                return ServiceResult.Failed(ServiceError.NotFound("Unknown user with provided Id."));
            }

            existingUser.Username = account.Username;
            existingUser.Email = account.Email;
            existingUser.Phone = account.Phone;
            existingUser.DateOfBirth = account.DateOfBirth;
            existingUser.FullName = account.FullName;

            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.Complete();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> UpdateUserPassword(Guid id, string oldPassword, string newPassword)
        {
            var existingUser = _unitOfWork.UserRepository.GetById(id);

            if (existingUser == null)
            {
                return ServiceResult.Failed(ServiceError.NotFound("Unknown user with provided Id."));
            }

            byte[] userHashedPassword = Convert.FromBase64String(existingUser.Password);

            byte[] saltBytes = new byte[16];
            Array.Copy(userHashedPassword, 0, saltBytes, 0, 16);

            // Regenerating the password hash with the given password.
            Rfc2898DeriveBytes hashingFunction = new Rfc2898DeriveBytes(oldPassword, saltBytes, 10000, HashAlgorithmName.SHA256);
            byte[] hashedPasswordBytes = hashingFunction.GetBytes(40);

            byte[] recreatedHash = new byte[saltBytes.Length + hashedPasswordBytes.Length];
            Array.Copy(saltBytes, 0, recreatedHash, 0, 16);
            Array.Copy(hashedPasswordBytes, 0, recreatedHash, 16, hashedPasswordBytes.Length);

            // Check if the recreated hash is the same as the password hash (in base 64 string).
            if (!(Convert.ToBase64String(recreatedHash) == existingUser.Password))
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("old password does not match"));
            }

            existingUser.Password = _tokenUtils.HashPassword(newPassword);

            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.Complete();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> UpdateUserStatus(Guid id, UserStatus status)
        {
            var existingUser = _unitOfWork.Repository<User>().GetById(id);

            if (existingUser == null)
            {
                return ServiceResult.Failed(ServiceError.NotFound("Unknown user with provided Id."));
            }

            existingUser.Status = status;

            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.Complete();

            return ServiceResult.Success();
        }
    }
}
