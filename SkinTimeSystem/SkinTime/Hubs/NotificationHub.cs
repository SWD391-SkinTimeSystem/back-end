using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace SkinTime.Hubs
{
    public class NotificationHub : Hub
    {
        private static ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();
        public async Task RegisterUser(string userId)
        {
            UserConnections[Context.ConnectionId] = userId;
            Console.WriteLine($"✅ User {userId} connected with ConnectionId: {Context.ConnectionId}");

        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"✅ Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        
        {
            Console.WriteLine($"❌ Client disconnected: {Context.ConnectionId}, Error: {exception?.Message}");
            await base.OnDisconnectedAsync(exception ?? new Exception("No specific error provided"));
        }

        public async Task SendNotification(string message)
        {
            Console.WriteLine($"📢 Sending notification: {message}");
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
        public async Task SendNotificationToUser(string userId, string message)
        {
            var connectionId = UserConnections.FirstOrDefault(x => x.Value == userId).Key;
            if (!string.IsNullOrEmpty(connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
                Console.WriteLine($"📢 Sent notification to user {userId}: {message}");
            }
        }
    }
}
