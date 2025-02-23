using Microsoft.EntityFrameworkCore;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly ITokenUtilities _tokenUtils;
        public UserService(IUnitOfWork unitOfWork, ITokenUtilities tokenUtilities)
        {
            _unitOfWork = unitOfWork;
            _tokenUtils = tokenUtilities;
        }

        public async Task CreateUser(User user)
        {
            var userRepository = _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.Complete();
        }

        public async Task<ServiceResult<User>> DeleteUser(string id)
        {
            if (Guid.TryParse(id, out var parsedGuid))
            {
                var result = await _unitOfWork.Repository<User>().GetByIdAsync(parsedGuid);

                if (result != null)
                {
                    result.Status = UserStatus.Deleted;
                    _unitOfWork.Repository<User>().Update(result);
                    
                    await _unitOfWork.Complete();

                    return ServiceResult<User>.Success(result);
                }

                return ServiceResult<User>.Failed(ServiceError.NotFound("Can not find user entity with provided id"));
            }

            return ServiceResult<User>.Failed(ServiceError.ValidationFailed("The provided id is not in the right format"));
        }

        public async Task<ServiceResult<ICollection<User>>> GetAllUsers()
        { 
            return ServiceResult<ICollection<User>>.Success(await _unitOfWork.Repository<User>().GetAllAsync()); 
        }

        public async Task<ServiceResult<User>> GetUser(string id)
        {
            if (Guid.TryParse(id, out var parsedGuid))
            {
                var user = await _unitOfWork.Repository<User>().GetByIdAsync(parsedGuid);

                if (user != null)
                {
                    return ServiceResult<User>.Success(user);
                }
                return ServiceResult<User>.Failed(ServiceError.NotFound("Can not find the user entity with provided id."));
            }
            return ServiceResult<User>.Failed(ServiceError.ValidationFailed("The provided user id format does not match."));
        }

        public async Task UpdateUser(string id, User user)
        {
            if (Guid.TryParse(id, out var parsedGuid))
            {
                var existingUser = _unitOfWork.Repository<User>().GetById(parsedGuid);

                if (existingUser == null)
                {
                    throw new Exception("Unknown user with provided Id.");
                }
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.Avatar = user.Avatar;
                existingUser.FullName = user.FullName;
                existingUser.Password = _tokenUtils.HashPassword(user.Password);

                _unitOfWork.Repository<User>().Update(existingUser);
                await _unitOfWork.Complete();
            }
        }

        public async Task<ServiceResult<IReadOnlyCollection<User>>> GetUsersAsReadOnly()
        {
            var result = await _unitOfWork.Repository<User>().ToListAsReadOnly();

            return ServiceResult<IReadOnlyCollection<User>>.Success(result);
        }

        public async Task<ServiceResult<User>> CreateUserAccount(User userInformation)
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
    }
}
