using SkinTime.BLL.Commons;
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
        Task<ServiceResult<ICollection<User>>> GetAllUsers();
        Task<ServiceResult<User>> GetUser(string id);
        Task CreateUser(User user);
        Task<ServiceResult<User>> DeleteUser(string user);
        Task UpdateUser(string id,User user);

        Task<ServiceResult<IReadOnlyCollection<User>>> GetUsersAsReadOnly();
        Task<ServiceResult<User>> CreateUserAccount(User customerInformation);
    }
}
