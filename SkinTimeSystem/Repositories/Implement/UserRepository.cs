using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context): base(context)
        {
        }

        public async Task<PaginationResult> GetMatchPaginated(int page, int pageSize, string? email, string? name, Gender? gender, UserRole? role, UserStatus? status, Func<IQueryable<User>, IOrderedQueryable<User>>? order)
        {
            // Inclusive searching
            Expression<Func<User, bool>> filter = user =>
                email != null ? user.Email.ToLower().Contains(email.ToLower()) : true &&
                name != null ? user.FullName.ToLower().Contains(name.ToLower()) : true &&
                role != null ? user.Role == role : true &&
                status != null ? user.Status == status : true &&
                gender != null ? user.Gender == gender : true;

            var result = await AsPaginated(page, pageSize, filter, x => x.Include(user => user.TherapistNavigation), order);

            Console.WriteLine(result.Content.Count());


            return new PaginationResult<User>
            {
                Content = result.Content,
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = pageSize
            };
        }

        public async Task<PaginationResult> GetUserWithRolePaginated(int page, int pageSize, UserRole role, Func<IQueryable<User>, IOrderedQueryable<User>>? order)
        {
            if (role == UserRole.Therapist)
            {
                return await AsPaginated(page, pageSize, x => x.Role == role, x => x.Include(x => x.TherapistNavigation), order);
            }

            return await AsPaginated(page, pageSize, x => x.Role == role, null, order);
        }

        public async Task<PaginationResult> GetUserWithStatusPaginated(int page, int pageSize, UserStatus status, Func<IQueryable<User>, IOrderedQueryable<User>>? order)
        {
            return await AsPaginated(page, pageSize, x => x.Status == status, null, order);
        }
    }
}
