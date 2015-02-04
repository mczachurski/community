using NUnit.Framework;
using Microsoft.Practices.ServiceLocation;
using SunLine.Community.Services.Core;
using SunLine.Community.Services.Search;
using SunLine.Community.Entities.Search;
using System.Linq;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.NUnitTests.Services.Search
{
    [TestFixture]
    public class SearchServiceTest : BaseTest
    {
        public override void SetUp()
        {
            base.SetUp();
            PrepareData();
        }

        [Test]
        public void search_service_must_return_expected_users_when_we_search_by_username()
        {
            var searchService = ServiceLocator.Current.GetInstance<ISearchService>();

            UserSearchResult result = searchService.SearchUsers("usertest", 0, 100);

            Assert.IsNotNull(result, "Service must return results with user list");
            Assert.AreEqual(10, result.AllResults, "Service must return proper amount of users when we search by user name");
            Assert.AreEqual(10, result.Users.Count(x => x.UserName.Contains("usertest")), "Service must return all expected users when we search by user name");
        }

        [Test]
        public void search_service_must_return_expected_users_when_we_search_by_first_name()
        {
            var searchService = ServiceLocator.Current.GetInstance<ISearchService>();

            UserSearchResult result = searchService.SearchUsers("Arnold", 0, 100);

            Assert.IsNotNull(result, "Service must return results with user list");
            Assert.AreEqual(1, result.AllResults, "Service must return proper amount of users when we search by first name");
            Assert.AreEqual(1, result.Users.Count(x => x.FirstName.Contains("Arnold")), "Service must return all expected users when we search by first name");
        }

        [Test]
        public void search_service_must_return_expected_users_when_we_search_by_last_name()
        {
            var searchService = ServiceLocator.Current.GetInstance<ISearchService>();

            UserSearchResult result = searchService.SearchUsers("Test5", 0, 100);

            Assert.IsNotNull(result, "Service must return results with user list");
            Assert.AreEqual(1, result.AllResults, "Service must return proper amount of users when we search by last name");
            Assert.AreEqual(1, result.Users.Count(x => x.LastName.Contains("Test5")), "Service must return all expected users when we search by last name");
        }

        [Test]
        public void search_service_must_return_expected_amount_of_users_when_we_specify_page()
        {
            var searchService = ServiceLocator.Current.GetInstance<ISearchService>();

            UserSearchResult result = searchService.SearchUsers("usertest", 1, 3);

            Assert.IsNotNull(result, "Service must return results with user list");
            Assert.AreEqual(10, result.AllResults, "Service must return proper amount of users");
            Assert.AreEqual(3, result.Users.Count(x => x.UserName.Contains("usertest")), "Service must return all expected users");
        }

        [Test]
        public void search_service_must_return_expected_messages_when_we_search_by_mind_text()
        {
            var searchService = ServiceLocator.Current.GetInstance<ISearchService>();

            UserMessageSearchResult result = searchService.SearchMessages("some mind", 0, 100);

            Assert.IsNotNull(result, "Service must return results with message list");
            Assert.AreEqual(7, result.AllResults, "Service must return proper amount of messages when we search by mind");
            Assert.AreEqual(7, result.UserMessages.Count(x => x.Message.Mind.Contains("some mind")), "Service must return all expected user messages when we search by mind");
        }

        [Test]
        public void search_service_must_return_expected_messages_when_we_search_by_speach_text()
        {
            var searchService = ServiceLocator.Current.GetInstance<ISearchService>();

            UserMessageSearchResult result = searchService.SearchMessages("fifth message", 0, 100);

            Assert.IsNotNull(result, "Service must return results with message list");
            Assert.AreEqual(1, result.AllResults, "Service must return proper amount of messages when we search by speech");
            Assert.AreEqual(1, result.UserMessages.Count(x => x.Message.Speech.Contains("fifth message")), "Service must return all expected user messages when we search by speech");
        }

        [Test]
        public void search_service_must_return_expected_amount_of_messages_when_we_specify_page()
        {
            var searchService = ServiceLocator.Current.GetInstance<ISearchService>();

            UserMessageSearchResult result = searchService.SearchMessages("some mind", 1, 3);

            Assert.IsNotNull(result, "Service must return results with message list");
            Assert.AreEqual(7, result.AllResults, "Service must return proper amount of messages");
            Assert.AreEqual(3, result.UserMessages.Count(x => x.Message.Mind.Contains("some mind")), "Service must return all expected user messages");
        }

        private void PrepareData()
        {
            var userMessageService = ServiceLocator.Current.GetInstance<IUserMessageService>();
            var messageRepository = ServiceLocator.Current.GetInstance<IMessageRepository>();
            var messageService = ServiceLocator.Current.GetInstance<IMessageService>();
            var messageStateRepository = ServiceLocator.Current.GetInstance<IMessageStateRepository>();
            var unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
            var usertest4 = DatabaseHelper.GetUserByUserName("usertest4");

            var message1 = messageService.CreateSpeech("This is text with some mind", "This is first message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message1);

            var message2 = messageService.CreateSpeech("This is text with some mind", "This is second message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message2);

            var message3 = messageService.CreateSpeech("This is text with some mind", "This is third message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message3);

            var message4 = messageService.CreateSpeech("This is text with some mind", "This is fourth message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message4);

            var message5 = messageService.CreateSpeech("This is text with some mind", "This is fifth message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message5);

            var message6 = messageService.CreateSpeech("This is text with some mind", "This is sixth message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message6);

            var message7 = messageService.CreateSpeech("This is text with some mind", "This is seventh message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message7);

            var message8 = messageService.CreateSpeech("This is diffrent text", "This is eight message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message8);

            var message9 = messageService.CreateSpeech("This is some text", "This is ninith message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message9);

            var message10 = messageService.CreateSpeech("This is nothing", "This is next message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message10);

            var message11 = messageService.CreateSpeech("This is text with some mind", "This is extra message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message11);

            messageRepository.ReloadEntity(message11);
            message11.MessageState = messageStateRepository.FindByEnum(MessageStateEnum.Deleted);
            messageRepository.Update(message11);
            unitOfWork.Commit();

            var message12 = messageService.CreateSpeech("This is text with some mind", "This is next message", usertest4.Id);
            unitOfWork.Commit();
            userMessageService.PublishMessage(message12);

            messageRepository.ReloadEntity(message12);
            message12.MessageState = messageStateRepository.FindByEnum(MessageStateEnum.Draft);
            messageRepository.Update(message12);
            unitOfWork.Commit();
        }
    }
}
