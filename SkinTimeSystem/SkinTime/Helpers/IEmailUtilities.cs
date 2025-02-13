namespace SkinTime.Helpers
{
    public interface IEmailUtilities
    {
        Task<bool> SendEmailAsync(string recipientAddress, string subject, string body, string smtpServer, int smtpPort, string senderAdress, string senderKey);

        Task<bool> SendGoogleEmailAsync(string recipient, string subject, string body, string sender, string credential);

        Task<bool> SendGoogleEmailAsync(string recipient, string subject, string body);
    }
}
