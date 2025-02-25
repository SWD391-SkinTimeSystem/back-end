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
        //Task<List<User>> GetAllUsers();
        Task<User> GetUser(string id);
        Task CrateUser(User user);
        Task DeleteUser(string user);
        Task UpadteUser(string id,User user);
        

    }
}
