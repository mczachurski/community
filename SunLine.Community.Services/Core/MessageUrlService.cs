using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BitlyDotNET.Implementations;
using BitlyDotNET.Interfaces;
using SunLine.Community.Common;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class MessageUrlService : IMessageUrlService
    {
        private readonly IMessageUrlRepository _messageUrlRepository;
        private readonly IErrorService _errorService;
        private readonly IBitlyService _bitlyService;

        public MessageUrlService(
            IMessageUrlRepository messageUrlRepository, 
            IErrorService errorService, 
            IBitlyService bitlyService)
        {
            _messageUrlRepository = messageUrlRepository;
            _errorService = errorService;
            _bitlyService = bitlyService;
        }

        public void CreateMessageUrls(Message message, IList<MessageUrl> messageUrls)
        {
            if(message.MessageUrls != null && message.MessageUrls.Count > 0)
            {
                IList<MessageUrl> toDelete = new List<MessageUrl>(message.MessageUrls);
                foreach(var item in toDelete)
                {
                    _messageUrlRepository.Delete(item);
                }
            }

            foreach (var messageUrl in messageUrls)
            {
                messageUrl.Message = message;
                _messageUrlRepository.Create(messageUrl);
            }
        }

        public string ParseMindUrl(string message, out IList<MessageUrl> messageUrls)
        {
            IList<KeyValuePair<String, String>> shorteneds;
            string changedMessages = RecognizeUrlsInMessage(message, out shorteneds);

            messageUrls = new List<MessageUrl>();
            foreach (var shortened in shorteneds)
            {
                var matchedShortenedUrls = Regex.Matches(changedMessages, shortened.Value);
                foreach (Match shortenedUrl in matchedShortenedUrls)
                {
                    var messageUrl = new MessageUrl
                    {
                        OriginalUrl = shortened.Key,
                        ShortenedlUrl = shortened.Value,
                        Index = shortenedUrl.Index,
                        Length = shortenedUrl.Length
                    };

                    messageUrls.Add(messageUrl);
                }
            }

            return changedMessages;
        }

        private string RecognizeUrlsInMessage(string message, out IList<KeyValuePair<String, String>> recognized)
        {
            recognized = new List<KeyValuePair<String, String>>();
            var urls = message.GetUrls();
            if (urls.Count == 0)
            {
                return message;
            }

            var uniqueUrls = urls.Distinct().ToList();
            foreach (var url in uniqueUrls)
            {
                if (ShouldWeChangeToShortUrl(url))
                {
                    var shortUrl = GenerateShortUrl(url);
                    message = message.Replace(url, shortUrl);
                    recognized.Add(new KeyValuePair<string, string>(url, shortUrl));
                }
                else
                {
                    recognized.Add(new KeyValuePair<string, string>(url, url));
                }
            }

            return message;
        }

        private static bool ShouldWeChangeToShortUrl(string url)
        {
            return url.Length > 21;
        }

        private String GenerateShortUrl(String url)
        {
            var shortUrl = url;
            try
            {
                var shortened = _bitlyService.Shorten(url);
                if (shortUrl != null)
                {
                    shortUrl = shortened;
                }
            }
            catch (Exception exception)
            {
                _errorService.Create(exception.ToString());
            }

            return shortUrl;
        }
    }
}