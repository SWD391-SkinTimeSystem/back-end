using BusinessObject.Entities;
using BusinessObject.Enum;
using Repositories;
using Services.Commons;
using Services.Commons.DTOs.User;
using Services.Commons.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<PaginationResult<AccountInformation>> GetAllUser(int page, int page_size);

        Task<PaginationResult<AccountInformation>> GetAllUser(int page, int page_size, UserStatus status);

        Task<ServiceResult> GetUserById(Guid id);

        Task<ServiceResult> CreateAccount(AccountRegistration account);

        Task<ServiceResult> CreateCustomerAccount(CustomerRegistration account);

        Task<ServiceResult> UpdateUserInformation(Guid id, AccountUpdateInformation account);

        Task<ServiceResult> UpdateUserPassword(Guid id, string oldPassword, string newPassword);

        Task<ServiceResult> UpdateUserStatus(Guid id, UserStatus status);
    }
}
