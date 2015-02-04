using System.Text.RegularExpressions;
using SunLine.Community.Common;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class MessageMentionService : IMessageMentionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageMentionRepository _messageMentionRepository;

        public MessageMentionService(IUserRepository userRepository, IMessageMentionRepository messageMentionRepository)
        {
            _userRepository = userRepository;
            _messageMentionRepository = messageMentionRepository;
        }

        public void CreateMentions(Message message)
        {
            var hashtags = message.Mind.GetMatchedUserNames();
            foreach (Match tag in hashtags)
            {
                CreateMentions(message, tag);
            }
        }

        private void CreateMentions(Message message, Capture tag)
        {
            var value = tag.Value.Replace("@", "").ToLower();
            var user = _userRepository.FindByUserName(value);

            if (user == null)
            {
                return;
            }

            var messageHashtag = new MessageMention
            {
                User = user,
                Message = message,
                Index = tag.Index,
                Length = tag.Length
            };

            _messageMentionRepository.Create(messageHashtag);
        }
    }
}