using Microsoft.EntityFrameworkCore;
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
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateUser(User user)
        {
            var userRepository = _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.Complete();
        }

        public async Task DeleteUser(string id)
        {
            if (Guid.TryParse(id, out var parsedGuid))
            {
                _unitOfWork.Repository<User>().Delete(parsedGuid);
                await _unitOfWork.Complete();
            }
        }

        public async Task<List<User>> GetAllUsers() => await _unitOfWork.Repository<User>().GetAll().ToListAsync();

        public async Task<User?> GetUser(string id)
        {
            if (Guid.TryParse(id, out var parsedGuid))
            {
                var user = await _unitOfWork.Repository<User>().GetEntityByIdAsync(parsedGuid);
                return user;
            }
            return null;
        }

        public async Task UpdateUser(string id, User user)
        {
            if (Guid.TryParse(id, out var parsedGuid))
            {
                var existingUser = _unitOfWork.Repository<User>().GetById(parsedGuid);

                if (existingUser == null)
                {
                    throw new Exception("Unknown user with provied Id.");
                }

                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;

                _unitOfWork.Repository<User>().Update(existingUser);
                await _unitOfWork.Complete();
            }
        }

        public async Task<IReadOnlyCollection<User>> GetUsersAsReadOnly()
        {
            return await _unitOfWork.Repository<User>().ListAllAsync();
        }

        public async Task<User> CreateUserAccount(User userInformation)
        {
            var repository = _unitOfWork.Repository<User>();

            /******* Data Validation ********/

            // Check for new email address.
            if (repository.Find(user => user.Email == userInformation.Email) != null)
            {
                throw new Exception("An exisitng account using this email has been registered");
            }

            // Check for existing username.
            if (repository.Find(user => user.UserName == userInformation.UserName) != null)
            {
                throw new Exception("The username has been taken");
            }

            // Validate password.
            Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            if (!passwordRegex.IsMatch(userInformation.Password))
            {
                throw new Exception("The password must has 8 character minimum, containing at least one letter and one number");
            }

            /******* Data Processing ********/

            byte[] saltBytes;
            RandomNumberGenerator.Fill(saltBytes = new byte[16]); // Generate new salt each time.

            Rfc2898DeriveBytes hashingFunction = new Rfc2898DeriveBytes(userInformation.Password, saltBytes, 10000, HashAlgorithmName.SHA256);
            byte[] hashedPasswordBytes = hashingFunction.GetBytes(40);

            byte[] savedPasswordHash = new byte[saltBytes.Length + hashedPasswordBytes.Length];
            Array.Copy(saltBytes, 0, savedPasswordHash, 0, 16);
            Array.Copy(hashedPasswordBytes, 0, savedPasswordHash, 16, hashedPasswordBytes.Length);

            userInformation.Password = Convert.ToBase64String(savedPasswordHash); // Set user password using the newly created hashed password string.
            /******* Data Storage ********/

            await repository.AddAsync(userInformation);
            await _unitOfWork.Complete();

            return userInformation;
        }

        public async Task<User?> GetUserWithCredential(string account, string password)
        {
            /******* Data Retrieval *******/
            var userAccount = await _unitOfWork.Repository<User>().FindAsync(x => x.UserName == account || x.Email == account);

            if (userAccount == null)
            {
                return null;
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
            return Convert.ToBase64String(recreatedHash) == userAccount.Password ? userAccount : null;
        }
    }
}
