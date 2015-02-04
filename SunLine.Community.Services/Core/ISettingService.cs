using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface ISettingService
    {
        IList<Setting> FindAll();
        Setting FindById(Guid id);
        void Update(Setting setting);

        string RecaptchaPrivateKey { get; }       
        string RecaptchaPublicKey { get; }
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpLogin { get; }
        string SmtpPassword { get; }
        string SmtpSenderName { get; }
        string SmtpSenderAddress { get; }
        string AttachmentsDirectory { get; }
        string BitlyUserName { get; }
        string BitlyPrivateKey { get; }
        int MaxCoverNameCounter { get; }
        string StorageConnectionString { get; }
    }
}
