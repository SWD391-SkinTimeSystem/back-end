using Microsoft.AspNetCore.SignalR;

namespace SkinTime.Hubs
{
    public class NotificationHub : Hub
    {
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
    }
}
