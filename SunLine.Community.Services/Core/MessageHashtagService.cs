using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SunLine.Community.Common;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class MessageHashtagService : IMessageHashtagService
    {
        private readonly IHashtagRepository _hashtagRepository;
        private readonly IMessageHashtagRepository _messageHashtagRepository;

        public MessageHashtagService(IHashtagRepository hashtagRepository, IMessageHashtagRepository messageHashtagRepository)
        {
            _hashtagRepository = hashtagRepository;
            _messageHashtagRepository = messageHashtagRepository;
        }

        public void CreateHashtags(Message message)
        {
            var hashtags = message.Mind.GetMatchedHashtags();
            IList<Hashtag> createdHashtags = new List<Hashtag>();
            foreach (Match tag in hashtags)
            {
                CreateHashtag(message, tag, createdHashtags);
            }
        }

        private void CreateHashtag(Message message, Capture tag, IList<Hashtag> createdHashtags)
        {
            var value = tag.Value.Replace("#", "").ToLower();
            var hashtag = createdHashtags.FirstOrDefault(x => x.Name == value) ?? _hashtagRepository.FindByName(value);

            if (hashtag == null)
            {
                hashtag = new Hashtag { Name = value };
                _hashtagRepository.Create(hashtag);
                createdHashtags.Add(hashtag);
            }

            var messageHashtag = new MessageHashtag
            {
                Hashtag = hashtag,
                Message = message,
                Index = tag.Index,
                Length = tag.Length
            };

            _messageHashtagRepository.Create(messageHashtag);
        }
    }
}