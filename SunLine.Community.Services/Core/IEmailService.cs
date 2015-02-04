namespace SunLine.Community.Services.Core
{
    public interface IEmailService
    {
        bool SendEmail(string receiverAddress, string receiverName, string subject, string body);
    }
}
