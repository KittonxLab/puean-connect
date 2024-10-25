using PuanConnect.Interfaces;
using System.Net;
using System.Net.Mail;

namespace PuanConnect.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private async Task SendEmail(string receiverEmail, string sendMessage, string subject)
    {
        string smtpServer = _configuration["EmailSettings:SmtpServer"]!;
        int port = int.Parse(_configuration["EmailSettings:SmtpPort"]!);
        string senderEmail = _configuration["EmailSettings:SenderEmail"]!;
        string senderPassword = _configuration["EmailSettings:SenderPassword"]!;

        if (!IsValidEmail(senderEmail!))
            throw new ArgumentException("Invalid sender email address");

        if (string.IsNullOrEmpty(senderPassword))
            throw new ArgumentException("Sender password is required");

        if (!IsValidEmail(receiverEmail))
            throw new ArgumentException("Invalid recipient email address");

        using (SmtpClient client = new SmtpClient(smtpServer, port))
        {
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential(new MailAddress(senderEmail!).Address, senderPassword);

            using (MailMessage message = new MailMessage(new MailAddress(senderEmail!), new MailAddress(receiverEmail)))
            {
                message.Subject = $"{subject}";
                message.Body = $"{sendMessage}\n";

                try
                {
                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }

    public async Task SendNotification(string receiverEmail, string sendMessage)
    {
        await SendEmail(receiverEmail, sendMessage, "PuanConnect Notification");
    }

    public async Task SendReminder(string receiverEmail, DateTime eventDate)
    {
        string message = $"Your event is about to start at {eventDate.ToString("dddd, dd MMMM yyyy")}. Don't forget!"; ;
        await SendEmail(receiverEmail, message, "PuanConnect Reminder");
    }

    public async Task SendCustomSubject(string receiverEmail, string sendMessage, string subject)
    {
        await SendEmail(receiverEmail, sendMessage, subject);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }


}