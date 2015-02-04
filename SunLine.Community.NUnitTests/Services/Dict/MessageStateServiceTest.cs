using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Services.Dict;

namespace SunLine.Community.NUnitTests.Services.Dict
{
    [TestFixture]
    public class MessageStateServiceTest : BaseTest
    {
        [Test]
        public void service_must_return_expected_message_state_by_enum()
        {
            var messageStateService = ServiceLocator.Current.GetInstance<IMessageStateService>();

            MessageState messageStete = messageStateService.FindByEnum(MessageStateEnum.Draft);

            Assert.IsNotNull(messageStete, "Service must return message state by enum");
            Assert.AreEqual(MessageStateEnum.Draft, messageStete.MessageStateEnum, "Service must return expected message stete by enum");
        }
    }
}

