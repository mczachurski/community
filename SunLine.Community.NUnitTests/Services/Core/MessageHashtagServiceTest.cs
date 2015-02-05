using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class MessageHashtagServiceTest : BaseTest
    {
        [Test]
        public void all_hashtags_must_be_recognized_in_message()
        {
            var hashtagService = ServiceLocator.Current.GetInstance<IMessageHashtagService>();
            var messageHashtagRepository = ServiceLocator.Current.GetInstance<IMessageHashtagRepository>();
            var hashtagRepository = ServiceLocator.Current.GetInstance<IHashtagRepository>();
            var messageRepository = ServiceLocator.Current.GetInstance<IMessageRepository>();
            var unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4, mind: "This is #fake #test with a few #hash #tags. Excellent #fake with #Tags.");
            messageRepository.Create(message);
            unitOfWork.Commit();

            hashtagService.CreateHashtags(message);
            unitOfWork.Commit();

            var hashtagsList = hashtagRepository.FindAll();
            var messageHashtagsList = messageHashtagRepository.FindAll();
            Assert.AreEqual(4, hashtagsList.Count(), "Numer of recognized hashtags is not good");
            Assert.AreEqual(6, messageHashtagsList.Count(), "Numer of recognized hashtags and connected with message is not good");
            Assert.IsTrue(hashtagsList.Any(x => x.Name == "fake"), "Fake hashtag was not recognized");
            Assert.IsTrue(hashtagsList.Any(x => x.Name == "test"), "Test hashtag was not recognized");
            Assert.IsTrue(hashtagsList.Any(x => x.Name == "hash"), "Hash hashtag was not recognized");
            Assert.IsTrue(hashtagsList.Any(x => x.Name == "tags"), "Tags hashtag was not recognized");

            var fakeHashtag = messageHashtagsList.OrderBy(x => x.Index).FirstOrDefault(x => x.Hashtag.Name == "fake");
            var testHashtag = messageHashtagsList.OrderBy(x => x.Index).FirstOrDefault(x => x.Hashtag.Name == "test");
            var hashHashtag = messageHashtagsList.OrderBy(x => x.Index).FirstOrDefault(x => x.Hashtag.Name == "hash");
            var tagsHashtag = messageHashtagsList.OrderBy(x => x.Index).FirstOrDefault(x => x.Hashtag.Name == "tags");
            Assert.IsNotNull(fakeHashtag, "#fake hastag was not recognized");
            Assert.IsNotNull(testHashtag, "#test hastag was not recognized");
            Assert.IsNotNull(hashHashtag, "#hash hastag was not recognized");
            Assert.IsNotNull(tagsHashtag, "#tags hastag was not recognized");
            Assert.AreEqual(8, fakeHashtag.Index, "Index of #fake hashtag was not recognized correctly");
            Assert.AreEqual(5, fakeHashtag.Length, "Length of #fake hashtag was not recognized correctly");
            Assert.AreEqual(14, testHashtag.Index, "Index of #test hashtag was not recognized correctly");
            Assert.AreEqual(5, testHashtag.Length, "Length of #test hashtag was not recognized correctly");
            Assert.AreEqual(31, hashHashtag.Index, "Index of #hash hashtag was not recognized correctly");
            Assert.AreEqual(5, hashHashtag.Length, "Length of #hash hashtag was not recognized correctly");
            Assert.AreEqual(37, tagsHashtag.Index, "Index of #tags hashtag was not recognized correctly");
            Assert.AreEqual(5, tagsHashtag.Length, "Length of #tags hashtag was not recognized correctly");
        }
    }
}
