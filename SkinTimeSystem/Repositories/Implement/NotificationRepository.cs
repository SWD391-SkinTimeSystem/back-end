using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context) { }

        public async Task CreateNotification(Guid userId, Guid toUserId, string message, Guid? aboutId = null)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Content = message,
                ToUserId = toUserId,
                AboutId = aboutId,
                UserId = userId,
                IsRead = false,
                CreatedTime = DateTime.Now
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginationResult<Notification>> GetAllNotification(Guid userId, int page, int pageSize)
        {
            Expression<Func<Notification, bool>> filter = x => x.ToUserId == userId;
            return await AsPaginated(
                page,
                pageSize,
                filter,
                includes: null,
                order: x => x.OrderBy(x => x.CreatedTime)
            );
        }
        public async Task MarkMultipleAsRead(List<Guid> notificationIds)
        {
            var notifications = await _context.Notifications
                .Where(n => notificationIds.Contains(n.Id) && !n.IsRead)
                .ToListAsync();

            if (notifications.Any())
            {
                notifications.ForEach(n => n.IsRead = true);
                notifications.ForEach(n => n.LastUpdate = DateTime.Now);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SendSystemNotification(Guid userId, string message, Guid? aboutId = null)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Content = message,
                ToUserId = null,
                UserId = userId, 
                AboutId = aboutId,
                CreatedTime = DateTime.UtcNow,
                IsRead = false
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }


    }

}
