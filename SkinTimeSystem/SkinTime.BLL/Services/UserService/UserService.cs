using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
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

        public async Task CrateUser(User user)
        {
            user.Id = Guid.NewGuid();
            user.Role = Role.Custommer.ToString();
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
      //  public async Task<List<User>> GetAllUsers() => _unitOfWork.Repository<User>().GetAll().ToList();
        

        public Task<User>? GetUser(string id)
        {
            if (Guid.TryParse(id, out var parsedGuid))
            {
                var user = _unitOfWork.Repository<User>().GetEntityByIdAsync(parsedGuid);
                return user;
            }
            return null;
        }

        public async Task UpadteUser(string id, User user)
        {
            if (Guid.TryParse(id, out var parsedGuid))
            {
                var existingUser = _unitOfWork.Repository<User>().GetById(parsedGuid);
                existingUser.UseName = user.UseName;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                _unitOfWork.Repository<User>().Update(existingUser);
                await _unitOfWork.Complete();
            }
        }
    }
}
