using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;
using System.Linq;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class MessageServiceTest : BaseTest
    {
        private IMessageService _messageService;
        private IMessageRepository _messageRepository;
        private IUserMessageRepository _userMessageRepository;
        private IUnitOfWork _unitOfWork;

        public override void SetUp()
        {
            base.SetUp();

            _messageRepository = ServiceLocator.Current.GetInstance<IMessageRepository>();
            _messageService = ServiceLocator.Current.GetInstance<IMessageService>();
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
            _userMessageRepository = ServiceLocator.Current.GetInstance<IUserMessageRepository>();
        }

        [Test]
        public void new_message_must_have_draft_status()
        {
            Message createdMessage = _messageService.CreateMind("25e9c44f-508d-499d-b332-cb00538928d6", DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();

            Assert.AreEqual(MessageStateEnum.Draft, createdMessage.MessageState.MessageStateEnum, "New message must have draft state");
        }

        [Test]
        public void creating_comment_must_create_new_message()
        {
            var commentedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            _messageRepository.Create(commentedMessage);
            _unitOfWork.Commit();

            Message createdMessage = _messageService.CreateComment("6eff25a0-3305-49fb-a2bf-3d335f892cc0", DatabaseHelper.UserTest1.Id, commentedMessage.Id);
            _unitOfWork.Commit();

            Assert.IsNotNull(createdMessage, "Creating comment must create new message");
        }

        [Test]
        [ExpectedException(typeof( ArgumentException ))]
        public void creating_comment_must_fail_when_we_not_specify_commented_message()
        {
            _messageService.CreateComment("1b693030-0c2d-497a-b962-e72d297ec1c2", DatabaseHelper.UserTest1.Id, Guid.Empty);
        }

        [Test]
        public void creating_quote_must_create_new_message()
        {
            var quotedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            _messageRepository.Create(quotedMessage);
            _unitOfWork.Commit();

            Message createdMessage = _messageService.CreateQuote("99145be3-91c3-4361-a866-3e33e3ae7ad1", DatabaseHelper.UserTest1.Id, quotedMessage.Id);
            _unitOfWork.Commit();

            Assert.IsNotNull(createdMessage, "Creating quote must create new message");
        }

        [Test]
        [ExpectedException(typeof( ArgumentException ))]
        public void creating_quote_must_fail_when_we_not_specify_quoted_message()
        {
            _messageService.CreateQuote("32300351-d8d0-4dfe-b811-5f8c7fc3e487", DatabaseHelper.UserTest1.Id, Guid.Empty);
        }

        [Test]
        public void creating_mind_must_create_new_message()
        {
            var quotedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            _messageRepository.Create(quotedMessage);
            _unitOfWork.Commit();

            Message createdMessage = _messageService.CreateMind("7d2adc67-ec78-4d32-8f5d-ed2342a656dc", DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();

            Assert.IsNotNull(createdMessage, "Creating mind must create new message");
        }

        [Test]
        [ExpectedException(typeof( ArgumentException ))]
        public void creating_mind_must_fail_when_we_not_specify_author()
        {
            _messageService.CreateMind("fd503a58-bfa3-440f-9215-784c711a5e49", Guid.Empty);
        }

        [Test]
        public void creating_speech_must_create_new_message()
        {
            var quotedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            _messageRepository.Create(quotedMessage);
            _unitOfWork.Commit();

            Message createdMessage = _messageService.CreateSpeech("dac72971-4552-471f-8fb7-cf4d8d50e2ba", "speech", DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();

            Assert.IsNotNull(createdMessage, "Creating speech must create new message");
        }

        [Test]
        [ExpectedException(typeof( ArgumentException ))]
        public void creating_speech_must_fail_when_we_not_specify_author()
        {
            _messageService.CreateSpeech("8c9e1bcf-e238-4836-862f-68942d0d2835", "speech", Guid.Empty);
        }

        [Test]
        public void creating_message_must_fail_when_we_trying_to_create_very_long_mind()
        {
            var quotedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            _messageRepository.Create(quotedMessage);
            _unitOfWork.Commit();

            Message createdMessage = _messageService.CreateMind("Sed dictum blandit odio, quis egestas lectus viverra vitae. " +
                "Suspendisse nec risus nulla. Donec ligula elit, vestibulum eget venenatis a, pretium nec nisl. Ut quis laoreet " +
                "ipsum. Praesent pulvinar lectus finibus ullamcorper imperdiet. Mauris eget suscipit eros, et tristique arcu. " +
                "Etiam euismod laoreet odio. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia " +
                "Curae. Lorem", DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();

            Assert.IsNull(createdMessage, "Creating mind must fail when we try save very long mind (>200)");
        }

        [Test]
        public void deleting_message_must_decrease_amount_of_quotes_on_orginal_message()
        {
            var quotedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            quotedMessage.AmountOfQuotes = 5;
            var newMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, quotedMessage: quotedMessage);
            _messageRepository.Create(newMessage);
            _unitOfWork.Commit();

            _messageService.Delete(newMessage.Id);

            _messageService.ReloadMessage(quotedMessage);
            Assert.AreEqual(4, quotedMessage.AmountOfQuotes, "Quoted message have incorrect amount of quotas after delete quote");
        }

        [Test]
        public void after_deleted_message_message_must_have_deleted_state()
        {
            var newMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            _messageRepository.Create(newMessage);
            _unitOfWork.Commit();

            _messageService.Delete(newMessage.Id);

            bool isDeleted = _messageRepository.FindAll().Any(x => x.Id == newMessage.Id && x.MessageState.MessageStateEnum == MessageStateEnum.Deleted);
            Assert.IsTrue(isDeleted, "Message must have deleted state after deleting message");
        }

        [Test]
        public void after_deleted_message_all_connected_user_message_must_have_deleted_state()
        {
            var newMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest2A);
            var userMessage1 = DatabaseHelper.CreateValidUserMessage(newMessage, DatabaseHelper.UserTest2A, modeEnum: UserMessageCreationModeEnum.ByHimselfNew);
            var userMessage2 = DatabaseHelper.CreateValidUserMessage(newMessage, DatabaseHelper.UserTest1);
            _messageRepository.Create(newMessage);
            _userMessageRepository.Create(userMessage1);
            _userMessageRepository.Create(userMessage2);
            _unitOfWork.Commit();

            _messageService.Delete(newMessage.Id);
            _unitOfWork.Commit();

            int amountOfDeleted = _userMessageRepository.FindAll().Count(x => x.Message.Id == newMessage.Id && x.UserMessageState.UserMessageStateEnum == UserMessageStateEnum.Deleted);
            Assert.AreEqual(2, amountOfDeleted, "All user message must have delete state after deleted message");
        }

        [Test]
        public void service_must_return_false_when_message_is_not_quoted_by_user()
        {
            var quotedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            var newMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, stateEnum: MessageStateEnum.Deleted, quotedMessage: quotedMessage);
            _messageRepository.Create(quotedMessage);
            _messageRepository.Create(newMessage);
            _unitOfWork.Commit();

            bool wasQuotedByUser = _messageService.WasMessageQuotedByUser(quotedMessage.Id, DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();

            Assert.IsFalse(wasQuotedByUser, "Service must return false when message is not quoted by user (or quote was deleted)");
        }

        [Test]
        public void service_must_return_true_when_message_is_quoted_by_user()
        {
            var quotedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            var newMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, quotedMessage: quotedMessage);
            _messageRepository.Create(quotedMessage);
            _messageRepository.Create(newMessage);
            _unitOfWork.Commit();

            bool wasQuotedByUser = _messageService.WasMessageQuotedByUser(quotedMessage.Id, DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();

            Assert.IsTrue(wasQuotedByUser, "Service must return true when message is quoted by user");
        }

        [Test]
        public void service_must_return_false_when_message_is_not_commented_by_user()
        {
            var commentedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            var newMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, stateEnum: MessageStateEnum.Deleted, commentedMessage: commentedMessage);
            _messageRepository.Create(commentedMessage);
            _messageRepository.Create(newMessage);
            _unitOfWork.Commit();

            bool wasCommentedByUser = _messageService.WasMessageCommentedByUser(commentedMessage.Id, DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();

            Assert.IsFalse(wasCommentedByUser, "Service must return false when message is not commented by user (or comment was deleted)");
        }

        [Test]
        public void service_must_return_true_when_message_is_commented_by_user()
        {
            var commentedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            var newMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, commentedMessage: commentedMessage);
            _messageRepository.Create(commentedMessage);
            _messageRepository.Create(newMessage);
            _unitOfWork.Commit();

            bool wasCommentedByUser = _messageService.WasMessageCommentedByUser(commentedMessage.Id, DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();

            Assert.IsTrue(wasCommentedByUser, "Service must return true when message is commented by user");
        }

        [Test]
        public void service_must_return_last_three_comments_to_message()
        {
            var commentedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            var newMessage1 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, commentedMessage: commentedMessage);
            var newMessage2 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, commentedMessage: commentedMessage);
            var newMessage3 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, commentedMessage: commentedMessage);
            var newMessage4 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, commentedMessage: commentedMessage);
            var newMessage5 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1, commentedMessage: commentedMessage);
            _messageRepository.Create(commentedMessage);
            _messageRepository.Create(newMessage1);
            _messageRepository.Create(newMessage2);
            _messageRepository.Create(newMessage3);
            _messageRepository.Create(newMessage4);
            _messageRepository.Create(newMessage5);
            newMessage1.CreationDate = DateTime.UtcNow.AddDays(-5);
            newMessage2.CreationDate = DateTime.UtcNow.AddDays(-4);
            newMessage3.CreationDate = DateTime.UtcNow.AddDays(-3);
            newMessage4.CreationDate = DateTime.UtcNow.AddDays(-2);
            newMessage5.CreationDate = DateTime.UtcNow.AddDays(-1);
            _unitOfWork.Commit();

            IList<Message> lastComments = _messageService.FindLastCommentsToMessage(commentedMessage.Id);
            _unitOfWork.Commit();

            Assert.AreEqual(3, lastComments.Count, "Service must return three messages");
            Assert.AreEqual(newMessage3.Id, lastComments[0].Id, "Service must return comments in proper order (3)");
            Assert.AreEqual(newMessage4.Id, lastComments[1].Id, "Service must return comments in proper order (4)");
            Assert.AreEqual(newMessage5.Id, lastComments[2].Id, "Service must return comments in proper order (5)");
        }

        [Test]
        public void service_must_return_correct_amount_of_messages_include_comments()
        {
            var newMessage1 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest3);
            var newMessage2 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest3);
            var newMessage3 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest3, stateEnum: MessageStateEnum.Deleted);
            var newMessage4 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest2A);
            var newMessage5 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest2A, stateEnum: MessageStateEnum.Deleted);
            var newMessage6 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest3);
            _messageRepository.Create(newMessage1);
            _messageRepository.Create(newMessage2);
            _messageRepository.Create(newMessage3);
            _messageRepository.Create(newMessage4);
            _messageRepository.Create(newMessage5);
            newMessage6.CommentedMessage = newMessage4;
            _messageRepository.Create(newMessage6);
            _unitOfWork.Commit();

            int amountOfMessages = _messageService.AmountOfMessages(DatabaseHelper.UserTest3.Id, false);
            _unitOfWork.Commit();

            Assert.AreEqual(3, amountOfMessages, "Service must return correct amount of user messages");
        }

        [Test]
        public void service_must_return_correct_amount_of_messages_exclude_comments()
        {
            var newMessage1 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest5);
            var newMessage2 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest5);
            var newMessage3 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest5, stateEnum: MessageStateEnum.Deleted);
            var newMessage4 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest2A);
            var newMessage5 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest2A, stateEnum: MessageStateEnum.Deleted);
            var newMessage6 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest5);
            _messageRepository.Create(newMessage1);
            _messageRepository.Create(newMessage2);
            _messageRepository.Create(newMessage3);
            _messageRepository.Create(newMessage4);
            _messageRepository.Create(newMessage5);
            newMessage6.CommentedMessage = newMessage4;
            _messageRepository.Create(newMessage6);
            _unitOfWork.Commit();

            int amountOfMessages = _messageService.AmountOfMessages(DatabaseHelper.UserTest5.Id, true);
            _unitOfWork.Commit();

            Assert.AreEqual(2, amountOfMessages, "Service must return correct amount of user messages when we excluded comments");
        }

        [Test]
        public void user_must_not_have_rights_to_not_his_own_message()
        {
            var newMessage1 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            _messageRepository.Create(newMessage1);
            _unitOfWork.Commit();

            bool hasRights = _messageService.HasRightToMessage(DatabaseHelper.UserTest2A.Id, newMessage1.Id);
            _unitOfWork.Commit();

            Assert.IsFalse(hasRights, "User must not have rights to not his own message");
        }

        [Test]
        public void user_must_have_rights_to_his_own_message()
        {
            var newMessage1 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            _messageRepository.Create(newMessage1);
            _unitOfWork.Commit();

            bool hasRights = _messageService.HasRightToMessage(DatabaseHelper.UserTest4.Id, newMessage1.Id);
            _unitOfWork.Commit();

            Assert.IsTrue(hasRights, "User must have rights to his own message");
        }

        [Test]
        public void service_must_return_correct_message_by_id()
        {
            var newMessage1 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            _messageRepository.Create(newMessage1);
            _unitOfWork.Commit();

            Message message = _messageService.FindById(newMessage1.Id);
            _unitOfWork.Commit();

            Assert.AreEqual(newMessage1.Id, message.Id, "Service must return correct message by id");
        }

        [Test]
        public void service_must_proper_update_speech()
        {
            var newMessage1 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4, mind: "Mind", speech: "Speech");
            _messageRepository.Create(newMessage1);
            _unitOfWork.Commit();

            Message message = _messageService.UpdateSpeech(newMessage1.Id, "This is new mind", "This is new speech");
            _unitOfWork.Commit();

            Assert.AreEqual("This is new mind", message.Mind, "Service must proper update mind");
            Assert.AreEqual("This is new speech", message.Speech, "Service must proper update speech");
        }

        [Test]
        public void service_must_proper_parse_mind_during_update_speech()
        {
            var newMessage1 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4, mind: "Mind", speech: "Speech");
            _messageRepository.Create(newMessage1);
            _unitOfWork.Commit();

            Message message = _messageService.UpdateSpeech(newMessage1.Id, "This is url http://en.wikipedia.org/wiki/List_of_films_considered_the_best.", "This is new speech");
            _unitOfWork.Commit();

            Assert.AreEqual("This is url http://bit.ly/16lmX8q.", message.Mind, "Service must proper update mind");
            Assert.AreEqual("This is new speech", message.Speech, "Service must proper update speech");
        }

        [Test]
        public void update_speech_must_fail_when_user_input_too_long_mind()
        {
            var newMessage1 = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4, mind: "Mind", speech: "Speech");
            _messageRepository.Create(newMessage1);
            _unitOfWork.Commit();

            Message message = _messageService.UpdateSpeech(newMessage1.Id, "This is long mind. This is long mind. This is long mind. This is long mind. " +
                "This is long mind. This is long mind. This is long mind. This is long mind. This is long mind. This is long mind. Test. Test.", "This is new speech");

            Assert.IsNull(message, "Service must proper update mind");
        }
    }
}
