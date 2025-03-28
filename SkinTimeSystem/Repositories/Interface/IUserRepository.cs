using BusinessObject.Entities;
using BusinessObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<PaginationResult> GetUserWithStatusPaginated(int page, int pageSize, UserStatus status, Func<IQueryable<User>, IOrderedQueryable<User>>? order);

        Task<PaginationResult> GetUserWithRolePaginated(int page, int pageSize, UserRole roles, Func<IQueryable<User>, IOrderedQueryable<User>>? order);

        Task<PaginationResult> GetMatchPaginated(int page, int pageSize, string? email, string? name, Gender? gender, UserRole? role, UserStatus? status, Func<IQueryable<User>, IOrderedQueryable<User>>? order);
    }
}
