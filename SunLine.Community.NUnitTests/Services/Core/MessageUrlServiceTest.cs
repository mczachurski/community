using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Rhino.Mocks;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;
using BitlyDotNET.Interfaces;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class MessageUrlServiceTest : BaseTest
    {
        private IMessageUrlService _messageUrlService;
        private IMessageUrlRepository _messageUrlRepository;
        private IErrorService _errorService;
        private IUnitOfWork _unitOfWork;
        private IBitlyService _bitlyService;

        public override void SetUp()
        {
            base.SetUp();

            _errorService = MockRepository.GenerateStub<IErrorService>();

            _bitlyService = ServiceLocator.Current.GetInstance<IBitlyService>();
            _messageUrlRepository = ServiceLocator.Current.GetInstance<IMessageUrlRepository>();
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();

            _messageUrlService = new MessageUrlService(_messageUrlRepository, _errorService, _bitlyService);
        }

        [Test]
        public void all_urls_must_be_recognized_in_message()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, mind: "This is url http://en.wikipedia.org/wiki/List_of_films_considered_the_best. And " +
                      "http://en.wikipedia.org/wiki/List_of_films_considered_the_best or http://rateyourmusic.com/list/morre/top_500_best_songs_ever/.");
            _unitOfWork.Commit();

            IList<MessageUrl> parsedUrls;
            message.Mind = _messageUrlService.ParseMindUrl(message.Mind, out parsedUrls);
            _messageUrlService.CreateMessageUrls(message, parsedUrls);
            _unitOfWork.Commit();

            Assert.AreEqual("This is url http://bit.ly/16lmX8q. And http://bit.ly/16lmX8q or http://bit.ly/16llJdo.", message.Mind, "Message after shorting url is incorrect");
            var messageUrls = _messageUrlRepository.FindAll().Where(x => x.Message.Id == message.Id);
            Assert.AreEqual(3, messageUrls.Count(), "Number of shortened urls is incorrect");
            var url1 = messageUrls.OrderBy(x => x.Index).FirstOrDefault(x => x.OriginalUrl == "http://en.wikipedia.org/wiki/List_of_films_considered_the_best");
            var url2 = messageUrls.OrderByDescending(x => x.Index).FirstOrDefault(x => x.OriginalUrl == "http://en.wikipedia.org/wiki/List_of_films_considered_the_best");
            var url3 = messageUrls.OrderBy(x => x.Index).FirstOrDefault(x => x.OriginalUrl == "http://rateyourmusic.com/list/morre/top_500_best_songs_ever/");
            Assert.IsNotNull(url1, "url1 was not recognized");
            Assert.IsNotNull(url2, "url2 mention was not recognized");
            Assert.IsNotNull(url3, "url3 mention was not recognized");
            Assert.AreEqual(12, url1.Index, "Index of url1 was not recognized correctly");
            Assert.AreEqual(21, url1.Length, "Length of url1 mention was not recognized correctly");
            Assert.AreEqual(39, url2.Index, "Index of url2 was not recognized correctly");
            Assert.AreEqual(21, url2.Length, "Length of url2 mention was not recognized correctly");
            Assert.AreEqual(64, url3.Index, "Index of url3 was not recognized correctly");
            Assert.AreEqual(21, url3.Length, "Length of url3 mention was not recognized correctly");
        }

        [Test]
        public void service_must_return_not_changed_mind_when_mind_dont_have_urls()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, mind: "This is message without urls @usertest #test.");
            _unitOfWork.Commit();

            IList<MessageUrl> parsedUrls;
            message.Mind = _messageUrlService.ParseMindUrl(message.Mind, out parsedUrls);
            _messageUrlService.CreateMessageUrls(message, parsedUrls);
            _unitOfWork.Commit();

            Assert.AreEqual("This is message without urls @usertest #test.", message.Mind, "Message after shorting url when mind dont have urls is incorrect");
            var messageUrls = _messageUrlRepository.FindAll().Where(x => x.Message.Id == message.Id);
            Assert.AreEqual(0, messageUrls.Count(), "Number of recognized urls is incorrect");
        }

        [Test]
        public void service_must_not_change_url_when_is_shorter_then_21_chars()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, mind: "This is message with short url http://community.com.");
            _unitOfWork.Commit();

            IList<MessageUrl> parsedUrls;
            message.Mind = _messageUrlService.ParseMindUrl(message.Mind, out parsedUrls);
            _messageUrlService.CreateMessageUrls(message, parsedUrls);
            _unitOfWork.Commit();

            Assert.AreEqual("This is message with short url http://community.com.", message.Mind, "Service must not change url shorted then 21 chars.");
            var messageUrls = _messageUrlRepository.FindAll().Where(x => x.Message.Id == message.Id);
            Assert.AreEqual(1, messageUrls.Count(), "Number of recognized urls is incorrect"); 
        }
    }
}
