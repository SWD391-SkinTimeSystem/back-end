using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.AuthenticationService
{
   public interface IAuthService
    {
        Task<ServiceResult<User>> GetUserWithCredential(string account, string password);

        Task<ServiceResult<User>> CreateUserWithGoogleWebToken(string token);

        Task<ServiceResult<User>> GetUserWithGoogleWebToken(string token);

        Task<ServiceResult> VerifyUserAccount(string id);
    }
}
