using BusinessObject.Entities;
using Services.Commons;
using Services.Commons.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        /// Task<ServiceResult<User>> GetUserWithCredential(string account, string password);

        //Task<ServiceResult<User>> CreateUserWithGoogleWebToken(string token);

        //Task<ServiceResult<User>> GetUserWithGoogleWebToken(string token);

        Task<ServiceResult> VerifyUserAccount(string id);

        Task<ServiceResult> AuthenUserWithCredential(string account, string password);

        Task<ServiceResult> AuthenUserWithGoogleWebToken(string token);

        Task<ServiceResult> RegenerateToken(AuthenticationTokens tokens);
    }
}
