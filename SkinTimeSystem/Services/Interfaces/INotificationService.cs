using BusinessObject.Entities;
using Repositories;
using Services.Commons.DTOs.Event;
using Services.Commons.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationWithCountDTO> GetAllNotification(Guid toUserId, int page, int pageSize);
        Task CreateNotificationOfSystem(Guid userId, string message, Guid? aboutId = null);
        Task MarkMultipleAsRead(List<Guid> notificationIds);
    }
}
