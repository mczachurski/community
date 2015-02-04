using System;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public IList<Setting> FindAll()
        {
            return _settingRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public Setting FindById(Guid id)
        {
            return _settingRepository.FindById(id);
        }

        public void Update(Setting setting)
        {
            Setting settingFromDb = _settingRepository.FindById(setting.Id);

            if (settingFromDb != null)
            {
                settingFromDb.Value = setting.Value;
                _settingRepository.Update(settingFromDb);
            }
        }

        public string RecaptchaPrivateKey
        {
            get { return GetSettingValue("RecaptchaPrivateKey"); }
        }

        public string RecaptchaPublicKey
        {
            get { return GetSettingValue("RecaptchaPublicKey"); }
        }

        public string SmtpServer
        {
            get { return GetSettingValue("SmtpServer"); }
        }

        public int SmtpPort
        {
            get { return GetSettingInt("SmtpPort"); }
        }

        public string SmtpLogin
        {
            get { return GetSettingValue("SmtpLogin"); }
        }

        public string SmtpPassword
        {
            get { return GetSettingValue("SmtpPassword"); }
        }

        public string SmtpSenderName
        {
            get { return GetSettingValue("SmtpSenderName"); }
        }

        public string SmtpSenderAddress
        {
            get { return GetSettingValue("SmtpSenderAddress"); }
        }

        public string AttachmentsDirectory
        {
            get { return GetSettingValue("AttachmentsDirectory"); }
        }

        public string BitlyUserName
        {
            get { return GetSettingValue("BitlyUserName"); }
        }

        public string BitlyPrivateKey
        {
            get { return GetSettingValue("BitlyPrivateKey"); }
        }

        public int MaxCoverNameCounter
        {
            get { return GetSettingInt("MaxCoverNameCounter"); }
        }

        public string StorageConnectionString
        {
            get { return GetSettingValue("StorageConnectionString"); }
        }

        private int GetSettingInt(string key)
        {
            string value = GetSettingValue(key);
            if(string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            int intValue;
            Int32.TryParse(value, out intValue);
            return intValue;
        }

        private string GetSettingValue(string key)
        {
            Setting setting = _settingRepository.Find(x => x.Key == key).FirstOrDefault();
            return setting != null ? setting.Value : null;
        }
    }
}
