using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class MessageMentionServiceTest : BaseTest
    {
        IMessageMentionService _messageMentionService;
        IMessageMentionRepository _messageMentionRepository;
        IMessageRepository _messageRepository;
        IUnitOfWork _unitOfWork;

        public override void SetUp()
        {
            base.SetUp();

            _messageMentionService = ServiceLocator.Current.GetInstance<IMessageMentionService>();
            _messageRepository = ServiceLocator.Current.GetInstance<IMessageRepository>();
            _messageMentionRepository = ServiceLocator.Current.GetInstance<IMessageMentionRepository>();
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
        }

        [Test]
        public void all_mentions_must_be_recognized_in_message()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, mind: "This is @usertest4 mention. And @usertest3 mention. This is @unknown user.");
            _messageRepository.Create(message);
            _unitOfWork.Commit();

            _messageMentionService.CreateMentions(message);
            _unitOfWork.Commit();

            var messageMentionsList = _messageMentionRepository.FindAll();
            Assert.AreEqual(2, messageMentionsList.Count(), "Numer of recognized mentions and connected with message is not good");
            var usertest4Mention = messageMentionsList.OrderBy(x => x.Index).FirstOrDefault(x => x.User.UserName == "usertest4");
            var usertest3Mention = messageMentionsList.OrderBy(x => x.Index).FirstOrDefault(x => x.User.UserName == "usertest3");
            Assert.IsNotNull(usertest4Mention, "@usertest4 mention was not recognized");
            Assert.IsNotNull(usertest3Mention, "@usertest3 mention was not recognized");
            Assert.AreEqual(8, usertest4Mention.Index, "Index of @usertest4 mention was not recognized correctly");
            Assert.AreEqual(10, usertest4Mention.Length, "Length of @usertest4 mention was not recognized correctly");
            Assert.AreEqual(32, usertest3Mention.Index, "Index of @usertest3 mention was not recognized correctly");
            Assert.AreEqual(10, usertest3Mention.Length, "Length of @usertest3 mention was not recognized correctly");
        }

        [Test]
        public void service_must_not_add_any_mentions_when_message_dont_have_mentions()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, mind: "This is message dont have mentions #test.");
            _messageRepository.Create(message);
            _unitOfWork.Commit();

            _messageMentionService.CreateMentions(message);
            _unitOfWork.Commit();

            var messageMentionsList = _messageMentionRepository.FindAll().Where(x => x.Message.Id == message.Id);
            Assert.AreEqual(0, messageMentionsList.Count(), "Service must not add any mentions when message dont have mentions");
        }
    }
}
