using System;
using System.Linq;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Repositories.Migrations.TableData.Core
{
    public static class SettingTable
    {
        public static void Initialize(IDatabaseContext context)
        {
            AddSetting(context, "RecaptchaPrivateKey", "Recaptcha private key", "<<RecaptchaPrivateKey>>");
            AddSetting(context, "RecaptchaPublicKey", "Recaptcha public key", "<<RecaptchaPublicKey>>");
            AddSetting(context, "SmtpServer", "SMTP server address", "<<SmtpServer>>");
            AddSetting(context, "SmtpPort", "SMTP server port", "<<SmtpPort>>");
            AddSetting(context, "SmtpLogin", "Login to the mail server", "<<SmtpLogin>>");
            AddSetting(context, "SmtpPassword", "The password for the mail server", "<<SmtpPassword>>");
            AddSetting(context, "SmtpSenderName", "The name of the address is sent", "<<SmtpSenderName>>");
            AddSetting(context, "SmtpSenderAddress", "Address from which emails are sent", "<<SmtpSenderAddress>>");
            AddSetting(context, "MaxCoverNameCounter", "Max cover name counter", @"<<MaxCoverNameCounter>>");
            AddSetting(context, "StorageConnectionString", "Connection string to Azure Storage", @"<<StorageConnectionString>>");
        }

        private static void AddSetting(IDatabaseContext context, string key, string name, string value)
        {
            if (context.Settings.FirstOrDefault(x => x.Key == key) == null)
            {
                context.Settings.Add(new Setting { Key = key, Name = name, Value = value, CreationDate = DateTime.Now, Version = 1 });
            }
        }
    }
}
