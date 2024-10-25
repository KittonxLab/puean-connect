namespace PuanConnect.Interfaces;

public interface IEmailService
{
    public Task SendCustomSubject(string receiverEmail,string sendMessage,string subject);
    public Task SendNotification(string receiverEmail,string sendMessage);
    public Task SendReminder(string receiverEmail,DateTime eventDate);

}