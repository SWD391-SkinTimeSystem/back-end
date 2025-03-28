using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<PaginationResult<Notification>> GetAllNotification(Guid toUserId, int page , int pageSize );
        Task CreateNotification(Guid userId, Guid toUserId, string message, Guid? aboutId = null);
        Task MarkMultipleAsRead(List<Guid> notificationIds);
        Task SendSystemNotification(Guid userId, string message, Guid? aboutId = null);
    }
}
