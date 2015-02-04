using System;
using System.Net;
using System.Net.Mail;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class EmailService : IEmailService
    {
        private readonly ISettingService _settingService;
        private readonly IErrorService _errorService;

        public EmailService(ISettingService settingService, IErrorService errorService)
        {
            _settingService = settingService;
            _errorService = errorService;
        }
            
        public bool SendEmail(string receiverAddress, string receiverName, string subject, string body)
        {
            bool wasSended = false;

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = _settingService.SmtpServer;
                smtpClient.Port = _settingService.SmtpPort;

                if (!string.IsNullOrWhiteSpace(_settingService.SmtpLogin) &&
                    !string.IsNullOrWhiteSpace(_settingService.SmtpPassword))
                {
                    smtpClient.Credentials = new NetworkCredential(_settingService.SmtpLogin, _settingService.SmtpPassword);
                }
                
                using (var message = new MailMessage())
                {
                    try
                    {
                        message.IsBodyHtml = true;
                        message.From = new MailAddress(_settingService.SmtpSenderAddress, _settingService.SmtpSenderName);
                        message.To.Add(new MailAddress(receiverAddress, receiverName));
                        message.Body = body;
                        message.Subject = subject;

                        smtpClient.Send(message);
                        wasSended = true;
                    }
                    catch (Exception exception)
                    {
                        _errorService.Create(exception.ToString());
                    }
                }
            }

            return wasSended;
        }
    }
}
