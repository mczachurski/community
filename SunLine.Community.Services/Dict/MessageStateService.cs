using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Dict;

namespace SunLine.Community.Services.Dict
{
    [BusinessLogic]
    public class MessageStateService : IMessageStateService
    {
        private readonly IMessageStateRepository _messageStateRepository;

        public MessageStateService(IMessageStateRepository messageStateRepository)
        {
            _messageStateRepository = messageStateRepository;
        }

        public MessageState FindByEnum(MessageStateEnum messageStateEnum)
        {
            return _messageStateRepository.FindByEnum(messageStateEnum);
        }
    }
}
