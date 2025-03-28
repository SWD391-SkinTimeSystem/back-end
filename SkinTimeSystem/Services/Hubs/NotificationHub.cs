using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;
using System.Collections.Concurrent;

namespace SkinTime.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;
        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        private static ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();
        public async Task RegisterUser(string userId)
        {
            UserConnections[Context.ConnectionId] = userId;

        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        
        {
            await base.OnDisconnectedAsync(exception ?? new Exception("No specific error provided"));
        }
        public async Task MarkMultipleAsReadNotifications(List<Guid> notificationIds)=> await _notificationService.MarkMultipleAsRead(notificationIds);
        public async Task SendUpdatedNotifications(string userId)
        {
            var connectionId = UserConnections.FirstOrDefault(x => x.Value == userId).Key;
            var notifications = await _notificationService.GetAllNotification(Guid.Parse(userId), 1, 10); 
            await Clients.User(connectionId).SendAsync("ReceiveUpdatedNotifications", notifications);
        }
    }
}
