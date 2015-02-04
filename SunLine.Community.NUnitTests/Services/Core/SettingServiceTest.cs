using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class SettingServiceTest : BaseTest
    {
        ISettingService _settingService;
        private IUnitOfWork _unitOfWork;

        public override void SetUp()
        {
            base.SetUp();

            _settingService = ServiceLocator.Current.GetInstance<ISettingService>();
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
        }
            
        [Test]
        public void service_must_return_correct_attachment_path()
        {
            var value = _settingService.AttachmentsDirectory;

            Assert.AreEqual(@"<<AttachmentsDirectory>>", value, "Service returned not correct attachment path");
        }

        [Test]
        public void service_must_return_correct_recaptcha_private_key()
        {
            var value = _settingService.RecaptchaPrivateKey;

            Assert.AreEqual(@"<<RecaptchaPrivateKey>>", value, "Service returned not correct recaptcha private key");
        }

        [Test]
        public void service_must_return_correct_recaptcha_public_key()
        {
            var value = _settingService.RecaptchaPublicKey;

            Assert.AreEqual(@"<<RecaptchaPublicKey>>", value, "Service returned not correct recaptcha public key");
        }

        [Test]
        public void service_must_return_correct_smtp_server_address()
        {
            var value = _settingService.SmtpServer;

            Assert.AreEqual(@"<<SmtpServer>>", value, "Service returned not correct smtp server address");
        }

        [Test]
        public void service_must_return_correct_smtp_server_port()
        {
            var value = _settingService.SmtpPort;

            Assert.AreEqual("<<SmtpPort>>", value, "Service returned not correct smtp server port");
        }

        [Test]
        public void service_must_return_correct_smtp_server_login()
        {
            var value = _settingService.SmtpLogin;

            Assert.AreEqual(@"<<SmtpLogin>>", value, "Service returned not correct smtp server login");
        }

        [Test]
        public void service_must_return_correct_smtp_server_password()
        {
            var value = _settingService.SmtpPassword;

            Assert.AreEqual(@"<<SmtpPassword>>", value, "Service returned not correct smtp server password");
        }

        [Test]
        public void service_must_return_correct_smtp_sender_name()
        {
            var value = _settingService.SmtpSenderName;

            Assert.AreEqual(@"<<SmtpSenderName>>", value, "Service returned not correct smtp sender name");
        }

        [Test]
        public void service_must_return_correct_smtp_sender_address()
        {
            var value = _settingService.SmtpSenderAddress;

            Assert.AreEqual(@"<<SmtpSenderAddress>>", value, "Service returned not correct smtp sender address");
        }

        [Test]
        public void service_must_return_correct_max_cover_name_counter()
        {
            var value = _settingService.MaxCoverNameCounter;

            Assert.AreEqual(37, value, "Service returned not correct max cover name counter");
        }
    }
}

