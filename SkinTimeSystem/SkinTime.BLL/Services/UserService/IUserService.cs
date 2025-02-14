using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User?> GetUser(string id);
        Task CreateUser(User user);
        Task DeleteUser(string user);
        Task UpdateUser(string id,User user);

        Task<IReadOnlyCollection<User>> GetUsersAsReadOnly();
        Task<User> CreateUserAccount(User customerInformation);
        Task<User?> GetUserWithCredential(string account, string password);
    }
}
