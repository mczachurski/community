using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class UserMessageServiceTest : BaseTest
    {
        private IUserMessageService _userMessageService;
        private IUserMessageRepository _userMessageRepository;
        private IMessageRepository _messageRepository;
        private IMessageService _messageService;
        private IUnitOfWork _unitOfWork;

        public override void SetUp()
        {
            base.SetUp();

            _userMessageService = ServiceLocator.Current.GetInstance<IUserMessageService>();
            _userMessageRepository = ServiceLocator.Current.GetInstance<IUserMessageRepository>();
            _messageRepository = ServiceLocator.Current.GetInstance<IMessageRepository>();
            _messageService = ServiceLocator.Current.GetInstance<IMessageService>();
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();

            CreateUserMessagesLists();
        }

        [Test]
        public void message_after_publish_must_have_published_state()
        {
            Message createdMessage = _messageService.CreateMind("25e9c44f-508d-499d-b332-cb00538928d6", DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();

            _userMessageService.PublishMessage(createdMessage);

            _messageRepository.ReloadEntity(createdMessage);
            Assert.AreEqual(MessageStateEnum.Published, createdMessage.MessageState.MessageStateEnum, "Message after publish must have publisged state");
        }

        [Test]
        public void user_must_have_right_to_his_own_user_messages()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            var userMessage1 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest1);
            _userMessageRepository.Create(userMessage1);
            _unitOfWork.Commit();

            bool userHaveRight = _userMessageService.HasRightToUserMessage(DatabaseHelper.UserTest1.Id, userMessage1.Id);
            _unitOfWork.Commit();

            Assert.IsTrue(userHaveRight, "User must have rights to his own user message");
        }

        [Test]
        public void user_must_not_have_right_to_other_users_user_messages()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            var userMessage1 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest1);
            _userMessageRepository.Create(userMessage1);
            _unitOfWork.Commit();

            bool userHaveRight = _userMessageService.HasRightToUserMessage(DatabaseHelper.UserTest2A.Id, userMessage1.Id);
            _unitOfWork.Commit();

            Assert.IsFalse(userHaveRight, "User must not have rights to not his own user message");
        }
            
        [Test]
        public void message_must_appears_in_direct_observers_main_feed_when_observed_user_add_message()
        {
            const string mindText = "acb99b89-22f4-4290-b1ae-beae5ff377c0";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest2A.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var numberOfAllUser2AMessages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest2A.Id && x.Message.Mind == mindText);
            var numberOfAllUser2BMessages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest2B.Id && x.Message.Mind == mindText);
            var numberOfAllUser1Messages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest1.Id && x.Message.Mind == mindText);
            Assert.AreEqual(1, numberOfAllUser2AMessages, "Message must appear on usertest2A feed when usertest2A adding a mesage (himself)");
            Assert.AreEqual(1, numberOfAllUser2BMessages, "Message must appear on usertest2B feed when usertest2A adding a mesage (observer)");
            Assert.AreEqual(1, numberOfAllUser1Messages, "Message must appear on usertest1 feed when usertest2A adding a mesage (observer)");
        }

        [Test]
        public void message_must_not_appears_in_direct_observers_main_feed_when_not_observed_user_add_message()
        {
            const string mindText = "d16d0631-5a4e-4089-9e7e-788141f850d7";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest4.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var numberOfAllUser2AMessages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest2A.Id && x.Message.Mind == mindText);
            var numberOfAllUser4Messages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest4.Id && x.Message.Mind == mindText);
            Assert.AreEqual(0, numberOfAllUser2AMessages, "Message must not appear on usertest2A feed when usertest4 adding a mesage (observer)");
            Assert.AreEqual(1, numberOfAllUser4Messages, "Message must appear on usertest4 feed when usertest4 adding a mesage (himself)");
        }

        [Test]
        public void message_must_not_appers_on_other_user_feeds_when_user_dont_have_observers()
        {
            const string mindText = "31269286-49cd-463d-b88c-2a4e1a9606b3";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest1.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var numberOfAllUser1Messages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest1.Id && x.Message.Mind == mindText);
            var numberOfOtherUserMessages = _userMessageRepository.FindAll().Count(x => x.User.Id != DatabaseHelper.UserTest1.Id && x.Message.Mind == mindText);
            Assert.AreEqual(1, numberOfAllUser1Messages, "Message must appear on usertest1 feed when usertest1 adding a mesage (himself)");
            Assert.AreEqual(0, numberOfOtherUserMessages, "Message must not appear on other users feeds when user don't have observers");
        }

        [Test]
        public void transmit_must_create_message_in_direct_observers_main_feed()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage3 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest3);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage3);
            _unitOfWork.Commit();

            _userMessageService.SendTransmit(DatabaseHelper.UserTest3.Id, message.Id);

            _userMessageRepository.ReloadEntity(userMessage3);
            var numberOfAllUser2Messages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest2A.Id && x.Message.Id == message.Id);
            var numberOfOtherUserMessages = _userMessageRepository.FindAll().Count(x => x.User.Id != DatabaseHelper.UserTest2A.Id && x.User.Id != DatabaseHelper.UserTest3.Id && x.Message.Id == message.Id);
            Assert.AreEqual(1, numberOfAllUser2Messages, "Message must retransmit to usertest2A feed when usertest3 retransmit");
            Assert.IsTrue(userMessage3.WasTransmitted, "Retransmitted user message must have set WasTransmitted as true");
            Assert.AreEqual(0, numberOfOtherUserMessages, "Message must not retransmit on other users feeds");
        }

        [Test]
        public void transmit_must_create_relation_beetween_user_messages()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage3 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest3);
            _userMessageRepository.Create(userMessage3);
            _unitOfWork.Commit();

            _userMessageService.SendTransmit(DatabaseHelper.UserTest3.Id, message.Id);

            var numberOfLinkedUserMessages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest2A.Id
                && x.Message.Id == message.Id && x.TransmittedUserMessage.Id == userMessage3.Id);
            Assert.AreEqual(1, numberOfLinkedUserMessages, "Transmitted user message must be connectec with orginal user message");
        }

        [Test]
        public void transmit_must_not_be_possible_for_transmitted_user_message()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage3 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest3, wasTransmitted: true);
            _userMessageRepository.Create(userMessage3);
            _unitOfWork.Commit();

            _userMessageService.SendTransmit(DatabaseHelper.UserTest3.Id, message.Id);

            var numberOfAllUserMessages = _userMessageRepository.FindAll().Count(x => x.Message.Id == message.Id && x.User.Id != DatabaseHelper.UserTest3.Id);
            Assert.AreEqual(0, numberOfAllUserMessages, "Message must not retransmit whe it was retransmitted");
        }

        [Test]
        public void transmit_must_not_create_user_message_when_user_dont_have_observers()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest2A);
            var userMessage1 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest1, wasTransmitted: true);
            _userMessageRepository.Create(userMessage1);
            _unitOfWork.Commit();

            _userMessageService.SendTransmit(DatabaseHelper.UserTest1.Id, message.Id);

            var numberOfAllUserMessages = _userMessageRepository.FindAll().Count(x => x.Message.Id == message.Id && x.User.Id != DatabaseHelper.UserTest1.Id);
            Assert.AreEqual(0, numberOfAllUserMessages, "Message must not retransmitted when user don't have observers");
        }

        [Test]
        public void transmit_must_increase_amoun_of_transmition_on_orginal_message()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage4 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            var userMessage3 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest3);
            var userMessage2A = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest2A);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage4);
            _userMessageRepository.Create(userMessage3);
            _userMessageRepository.Create(userMessage2A);
            _unitOfWork.Commit();

            _userMessageService.SendTransmit(DatabaseHelper.UserTest4.Id, message.Id);
            _userMessageService.SendTransmit(DatabaseHelper.UserTest3.Id, message.Id);
            _userMessageService.SendTransmit(DatabaseHelper.UserTest2A.Id, message.Id);

            _messageRepository.ReloadEntity(message);
            Assert.AreEqual(3, message.AmountOfTransmitted, "Transmition must increase amount of transmission on orginal message");
        }

        [Test]
        public void mentioned_and_transmitted_message_must_be_visible_on_main_feed()
        {
            string mindText = "@usertest1 fake message";
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest3, mind: mindText);
            var userMessage2A = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest2A);
            var userMessage1 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest1, haveMention: true, modeEnum: UserMessageCreationModeEnum.ByNotObserved);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage2A);
            _userMessageRepository.Create(userMessage1);
            _unitOfWork.Commit();

            _userMessageService.SendTransmit(DatabaseHelper.UserTest2A.Id, message.Id);

            bool isByObservedTransmit = _userMessageRepository.FindAll().Any(x => x.Message.Mind == mindText && x.User.Id == DatabaseHelper.UserTest1.Id && x.UserMessageCreationMode.UserMessageCreationModeEnum == UserMessageCreationModeEnum.ByObservedTransmit);
            var amountOfUserMessages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest1.Id && x.Message.Id == message.Id);
            bool hasSetTransmitedMessage = _userMessageRepository.FindAll().Any(x => x.Message.Mind == mindText && x.User.Id == DatabaseHelper.UserTest1.Id && x.TransmittedUserMessage.Id == userMessage2A.Id);
            Assert.AreEqual(1, amountOfUserMessages, "User have to have only one user message after mention and transmit message to him");
            Assert.IsTrue(hasSetTransmitedMessage, "After retransmit user message must be set correctly");
            Assert.IsTrue(isByObservedTransmit, "Mentioned user message must change to created by observers after transmitted");
        }

        [Test]
        public void mention_must_apper_in_main_feed_when_we_mention_user_that_observed_us()
        {
            const string mindText = "This is mention to @usertest3 user.";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest4.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var userMessage3 = _userMessageRepository.FindAll().FirstOrDefault(x => x.Message.Mind == mindText && x.User.Id == DatabaseHelper.UserTest3.Id);
            Assert.IsNotNull(userMessage3, "Message must appears on feed observed user");
            Assert.AreEqual(UserMessageCreationModeEnum.ByObservedNew, userMessage3.UserMessageCreationMode.UserMessageCreationModeEnum, "Message must have set information that is created by observerd user");
            Assert.IsTrue(userMessage3.HaveMention, "Message must have set information that have mention");
        }

        [Test]
        public void mention_must_apper_in_mentioned_user_feed_when_we_mention_user_that_not_observed_us()
        {
            const string mindText = "This is mention to @usertest2A user.";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest4.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var userMessage2A = _userMessageRepository.FindAll().FirstOrDefault(x => x.Message.Mind == mindText && x.User.Id == DatabaseHelper.UserTest2A.Id);
            Assert.IsNotNull(userMessage2A, "Message must appears on feed observed user");
            Assert.AreEqual(UserMessageCreationModeEnum.ByNotObserved, userMessage2A.UserMessageCreationMode.UserMessageCreationModeEnum, "Message must have set information that is created by not observerd user");
            Assert.IsTrue(userMessage2A.HaveMention, "Message must have set information that have mention");
        }

        [Test]
        public void when_user_create_message_with_and_mention_himself_mention_must_not_appears_on_his_mention_feed()
        {
            const string mindText = "This is mention to @usertest4 user.";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest4.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var userMessage2A = _userMessageRepository.FindAll().FirstOrDefault(x => x.Message.Mind == mindText && x.User.Id == DatabaseHelper.UserTest4.Id);
            Assert.IsNotNull(userMessage2A, "Message must appears on main feed");
            Assert.AreEqual(UserMessageCreationModeEnum.ByHimselfNew, userMessage2A.UserMessageCreationMode.UserMessageCreationModeEnum, "Message must have set information that user create message himself");
            Assert.IsFalse(userMessage2A.HaveMention, "Message must have set information that it don't have mention");
        }

        [Test]
        public void message_sended_to_user_that_not_observed_us_must_apper_in_his_mention_fedd()
        {
            const string mindText = "@usertest2A this is message sended to user.";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest4.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var userMessage2A = _userMessageRepository.FindAll().FirstOrDefault(x => x.Message.Mind == mindText && x.User.Id == DatabaseHelper.UserTest2A.Id);
            var amountOfUserMessages = _userMessageRepository.FindAll().Count(x => x.Message.Mind == mindText && x.User.Id != DatabaseHelper.UserTest4.Id);
            Assert.IsNotNull(userMessage2A, "Message must appears on feed mentioned user");
            Assert.AreEqual(1, amountOfUserMessages, "Message must appear only in @usertest2A feed");
            Assert.AreEqual(UserMessageCreationModeEnum.ByNotObserved, userMessage2A.UserMessageCreationMode.UserMessageCreationModeEnum, "Message must have set information that is created by not observerd user");
            Assert.IsTrue(userMessage2A.HaveMention, "Message must have set information that have mention");
        }

        [Test]
        public void message_sended_to_user_must_apper_in_his_mention_feed_and_in_all_connected_observers_fedd()
        {
            const string mindText = "@usertest2B this is message sended to user.";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest3.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var userMessage2A = _userMessageRepository.FindAll().FirstOrDefault(x => x.Message.Mind == mindText && x.User.Id == DatabaseHelper.UserTest2A.Id);
            var userMessage2B = _userMessageRepository.FindAll().FirstOrDefault(x => x.Message.Mind == mindText && x.User.Id == DatabaseHelper.UserTest2B.Id);
            var amountOfUserMessages = _userMessageRepository.FindAll().Count(x => x.Message.Mind == mindText && x.User.Id != DatabaseHelper.UserTest3.Id);
            Assert.IsNotNull(userMessage2B, "Message must appears on feed mentioned user");
            Assert.IsNotNull(userMessage2A, "Message must appears on feed observing user (user observing message author and mentioned user)");
            Assert.AreEqual(2, amountOfUserMessages, "Message must appear only in @usertest2A and @usertest2B feeds");
            Assert.AreEqual(UserMessageCreationModeEnum.ByNotObserved, userMessage2B.UserMessageCreationMode.UserMessageCreationModeEnum, "Message must have set information that is created by not observerd user");
            Assert.AreEqual(UserMessageCreationModeEnum.ByObservedNew, userMessage2A.UserMessageCreationMode.UserMessageCreationModeEnum, "Message must have set information that is created by observerd user");
            Assert.IsTrue(userMessage2B.HaveMention, "Message must have set information that have mention");
            Assert.IsFalse(userMessage2A.HaveMention, "Message must haven't set information that have mention");
        }

        [Test]
        public void message_sended_to_observing_user_must_appear_on_his_main_feed()
        {
            const string mindText = "@usertest3 this is message sended to user.";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest4.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var userMessage3 = _userMessageRepository.FindAll().FirstOrDefault(x => x.Message.Mind == mindText && x.User.Id == DatabaseHelper.UserTest3.Id);
            var amountOfUserMessages = _userMessageRepository.FindAll().Count(x => x.Message.Mind == mindText && x.User.Id != DatabaseHelper.UserTest4.Id);
            Assert.IsNotNull(userMessage3, "Message must appears on feed mentioned user");
            Assert.AreEqual(1, amountOfUserMessages, "Message must appear only in @usertest3 feed");
            Assert.AreEqual(UserMessageCreationModeEnum.ByObservedNew, userMessage3.UserMessageCreationMode.UserMessageCreationModeEnum, "Message must have set information that is created by observerd user");
            Assert.IsTrue(userMessage3.HaveMention, "Message must have set information that have mention");
        }

        [Test]
        public void created_comment_must_have_relation_with_commented_message()
        {
            const string mindText = "45deb013-4bd8-445f-9d32-eb61d79a1466";
            var commentedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage4 = DatabaseHelper.CreateValidUserMessage(commentedMessage, DatabaseHelper.UserTest4);
            _messageRepository.Create(commentedMessage);
            _userMessageRepository.Create(userMessage4);
            _unitOfWork.Commit();

            var createdMessage = _messageService.CreateComment(mindText, DatabaseHelper.UserTest4.Id, commentedMessage.Id);
            _unitOfWork.Commit();

            Assert.IsNotNull(createdMessage.CommentedMessage, "Comment must have relation with commented message");
        }

        [Test]
        public void created_comment_must_increment_amount_of_comments_on_message()
        {
            const string mindText = "dfcd9690-6836-45af-b9b1-484833dbc032";
            var commentedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            commentedMessage.AmountOfComments = 10;
            var userMessage4 = DatabaseHelper.CreateValidUserMessage(commentedMessage, DatabaseHelper.UserTest4);
            _messageRepository.Create(commentedMessage);
            _userMessageRepository.Create(userMessage4);
            _unitOfWork.Commit();

            var createdMessage = _messageService.CreateComment(mindText, DatabaseHelper.UserTest4.Id, commentedMessage.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            _messageRepository.ReloadEntity(commentedMessage);
            Assert.AreEqual(11, commentedMessage.AmountOfComments, "After add comment to message amout of comments was't incremented");
        }

        [Test]
        public void mention_in_comment_must_create_user_message_with_full_conversation_for_mentioned_user()
        {
            const string mindText = "Comment with mention @usertest4.";
            var commentedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest2A);
            var userMessage1 = DatabaseHelper.CreateValidUserMessage(commentedMessage, DatabaseHelper.UserTest2A);
            _messageRepository.Create(commentedMessage);
            _userMessageRepository.Create(userMessage1);
            _unitOfWork.Commit();

            var createdMessage = _messageService.CreateComment(mindText, DatabaseHelper.UserTest1.Id, commentedMessage.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            UserMessage mentionedUserMessage = _userMessageRepository.FindAll().FirstOrDefault(x => x.User.Id == DatabaseHelper.UserTest4.Id && x.Message.Id == commentedMessage.Id);
            Assert.IsNotNull(mentionedUserMessage, "Comment with mention must create user message with full conversation for mentioned user");
            Assert.IsTrue(mentionedUserMessage.Message.Id == commentedMessage.Id, "Created user message have to be releated with main conversation message");
            Assert.IsTrue(mentionedUserMessage.HaveMentionInComments, "Comment with mention must have set information that ii have mention in comment");
        }

        [Test]
        public void add_quoted_message_must_increase_amount_of_quotes_on_orginal_message()
        {
            const string mindText = "79e3f0e2-2c52-4b85-a1d5-a9d2ff33128c";
            var quotedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest1);
            quotedMessage.AmountOfQuotes = 5;
            _messageRepository.Create(quotedMessage);
            _unitOfWork.Commit();

            var createdMessage = _messageService.CreateQuote(mindText, DatabaseHelper.UserTest1.Id, quotedMessage.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            _messageRepository.ReloadEntity(quotedMessage);
            Assert.AreEqual(6, quotedMessage.AmountOfQuotes, "Quoted message have incorrect amount of quotas after add quote");
        }

        [Test]
        public void transmit_must_create_message_in_transmitted_user_feed_when_user_dont_have_message_on_feed()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage3 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage3);
            _unitOfWork.Commit();

            _userMessageService.SendTransmit(DatabaseHelper.UserTest2A.Id, message.Id);

            var numberOfAllUser2Messages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest2A.Id && x.Message.Id == message.Id);
            Assert.AreEqual(1, numberOfAllUser2Messages, "After transmit message from not own feed newuser message must create");
        }

        [Test]
        public void transmit_must_create_message_in_observers_feed_when_user_trensmit_message_not_from_his_own_feed()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage3 = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage3);
            _unitOfWork.Commit();

            _userMessageService.SendTransmit(DatabaseHelper.UserTest2A.Id, message.Id);

            var numberOfAllUser1Messages = _userMessageRepository.FindAll().Count(x => x.User.Id == DatabaseHelper.UserTest1.Id && x.Message.Id == message.Id);
            Assert.AreEqual(1, numberOfAllUser1Messages, "After transmit message from not own feed message must be visible on observers feed");
        }

        [Test]
        public void user_message_must_have_actual_sorting_date_after_add_new_message()
        {
            const string mindText = "2374d060-b948-4e77-b7f0-a77137b28312";

            var createdMessage = _messageService.CreateMind(mindText, DatabaseHelper.UserTest2A.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            var user1Message = _userMessageRepository.FindAll().FirstOrDefault(x => x.User.Id == DatabaseHelper.UserTest1.Id && x.Message.Mind == mindText);
            Assert.IsNotNull(user1Message, "Massege was't created");
            Assert.IsTrue(DateTime.UtcNow.AddMinutes(-1) < user1Message.SortingDate , "New user message must have set sorting date from creation date from message");
        }

        [Test]
        public void user_message_must_have_new_sorting_date_after_add_comment_when_user_have_enabled_updating_sorting_date()
        {
            const string mindText = "36ca4776-2f55-4ca5-8eb9-a2c71af4c1fd";
            var commentedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest2A);
            var userMessage1 = DatabaseHelper.CreateValidUserMessage(commentedMessage, DatabaseHelper.UserTest1);
            var userMessage2A = DatabaseHelper.CreateValidUserMessage(commentedMessage, DatabaseHelper.UserTest2A);
            userMessage2A.UpdateSortingDateOnNewComment = true;
            _messageRepository.Create(commentedMessage);
            _userMessageRepository.Create(userMessage1);
            _userMessageRepository.Create(userMessage2A);
            var fakeUserMessageSortingDate = DateTime.UtcNow.AddDays(-1);
            userMessage2A.SortingDate = fakeUserMessageSortingDate;
            _unitOfWork.Commit();

            var createdMessage = _messageService.CreateComment(mindText, DatabaseHelper.UserTest1.Id, commentedMessage.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            _userMessageRepository.ReloadEntity(userMessage2A);
            Assert.AreNotEqual(fakeUserMessageSortingDate, userMessage2A.SortingDate, "User message not have new sorting date after add comment when user have enabled updating sorting date");
        }

        [Test]
        public void user_message_must_not_have_new_sorting_date_after_add_comment_when_user_have_disabled_updating_sorting_date()
        {
            const string mindText = "6caf6cbc-25a0-4e3a-bf06-5cb8a16e2835";
            var commentedMessage = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest2A);
            var userMessage1 = DatabaseHelper.CreateValidUserMessage(commentedMessage, DatabaseHelper.UserTest2A);
            userMessage1.UpdateSortingDateOnNewComment = false;
            _messageRepository.Create(commentedMessage);
            _userMessageRepository.Create(userMessage1);
            var fakeUserMessageSortingDate = DateTime.UtcNow.AddDays(-1);
            userMessage1.SortingDate = fakeUserMessageSortingDate;
            _unitOfWork.Commit();

            var createdMessage = _messageService.CreateComment(mindText, DatabaseHelper.UserTest1.Id, commentedMessage.Id);
            _unitOfWork.Commit();
            _userMessageService.PublishMessage(createdMessage);

            Assert.AreEqual(fakeUserMessageSortingDate, userMessage1.SortingDate, "User message have new sorting date after add comment when user have disabled updating sorting date");
        }

        [Test]
        public void bubble_must_be_enabled_after_toggle()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            userMessage.UpdateSortingDateOnNewComment = false;
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            _unitOfWork.Commit();

            _userMessageService.ToggleBubble(userMessage.User.Id, userMessage.Message);
            _unitOfWork.Commit();

            Assert.IsTrue(userMessage.UpdateSortingDateOnNewComment, "User message must have enabled bubble after toggle");
        }

        [Test]
        public void bubble_must_be_disabled_after_toggle()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            userMessage.UpdateSortingDateOnNewComment = true;
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            _unitOfWork.Commit();

            _userMessageService.ToggleBubble(userMessage.User.Id, userMessage.Message);
            _unitOfWork.Commit();

            Assert.IsFalse(userMessage.UpdateSortingDateOnNewComment, "User message must have disabled bubble after toggle");
        }

        [Test]
        public void service_must_return_correct_user_message_by_message_id_for_user()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            _unitOfWork.Commit();

            UserMessage userMessageFromService = _userMessageService.FindUserMessage(DatabaseHelper.UserTest4.Id, message.Id);

            Assert.AreEqual(userMessage.Id, userMessageFromService.Id, "Service must return correct user message by message id");
        }

        [Test]
        public void service_must_not_return_user_message_when_user_not_see_message()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            _unitOfWork.Commit();

            UserMessage userMessageFromService = _userMessageService.FindUserMessage(DatabaseHelper.UserTest3.Id, message.Id);

            Assert.IsNull(userMessageFromService, "Service must not return user message when user not see message");
        }

        [Test]
        public void service_must_return_correct_user_message_with_marker_for_user()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            userMessage.IsMarkerSet = true;
            _unitOfWork.Commit();

            UserMessage userMessageFromService = _userMessageService.FindUserMessageWithMarker(DatabaseHelper.UserTest4.Id);

            Assert.AreEqual(userMessage.Id, userMessageFromService.Id, "Service must return correct user message with marker by user id");
        }

        [Test]
        public void service_must_not_return_user_message_when_marker_for_user_is_not_set()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest3);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest3);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            _unitOfWork.Commit();

            UserMessage userMessageFromService = _userMessageService.FindUserMessageWithMarker(DatabaseHelper.UserTest3.Id);

            Assert.IsNull(userMessageFromService, "Service must not return user message when marker is not set");
        }

        [Test]
        public void user_must_see_correct_amount_of_his_own_messages_newest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindMessagesCreatedByUserNewestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-8));
            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();

            Assert.AreEqual(3, userMessages.Count, "User must see correct amount of messages newest then specific date");
        }
            
        [Test]
        public void user_must_see_his_own_messages_in_correct_order_newest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindMessagesCreatedByUserNewestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-8));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.IsTrue(userMessages[0].SortingDate > userMessages[1].SortingDate, "User must see his own messages in correct order (1)");
            Assert.IsTrue(userMessages[1].SortingDate > userMessages[2].SortingDate, "User must see hiw own messages in correct order (2)");
        }

        [Test]
        public void user_must_see_correct_amount_of_his_own_messages_oldest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindMessagesCreatedByUserOldestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-2));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.AreEqual(3, userMessages.Count, "User must see correct amount of his own messages oldest then specific date");
        }

        [Test]
        public void user_must_see_his_own_messages_in_correct_order_oldest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindMessagesCreatedByUserOldestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-3));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.IsTrue(userMessages[0].SortingDate > userMessages[1].SortingDate, "User must see his own messages in correct order (1)");
            Assert.IsTrue(userMessages[1].SortingDate > userMessages[2].SortingDate, "User must see his own messages in correct order (2)");
        }

        [Test]
        public void user_must_see_correct_amount_of_messages_on_his_feed_newest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindUserFeedMessagesNewestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-8));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.AreEqual(4, userMessages.Count, "User must see correct amount of messages newest then specific date on his feed");
        }

        [Test]
        public void user_must_see_messages_on_his_feed_in_correct_order_newest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindUserFeedMessagesNewestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-8));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.IsTrue(userMessages[0].SortingDate > userMessages[1].SortingDate, "User must see messages on his feed in correct order (1)");
            Assert.IsTrue(userMessages[1].SortingDate > userMessages[2].SortingDate, "User must see messages on his feed in correct order (2)");
        }

        [Test]
        public void user_must_see_correct_amount_of_messages_on_his_feed_oldest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindUserFeedMessagesOldestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-2));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.AreEqual(4, userMessages.Count, "User must see correct amount of messages oldest then specific date on his feed");
        }

        [Test]
        public void user_must_see_messages_on_his_feed_in_correct_order_oldest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindUserFeedMessagesOldestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-3));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.IsTrue(userMessages[0].SortingDate > userMessages[1].SortingDate, "User must see messages on his feed in correct order (1)");
            Assert.IsTrue(userMessages[1].SortingDate > userMessages[2].SortingDate, "User must see messages on his feed in correct order (2)");
        }

        [Test]
        public void user_must_see_correct_amount_of_messages_with_mentions_newest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindMentionsNewestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-6));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.AreEqual(2, userMessages.Count, "User must see correct amount of messages with mentions newest then specific date on his feed");
        }

        [Test]
        public void user_must_see_messages_with_mentions_in_correct_order_newest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindMentionsNewestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-6));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.IsTrue(userMessages[0].SortingDate > userMessages[1].SortingDate, "User must see messages with mentions in correct order (1)");
        }

        [Test]
        public void user_must_see_correct_amount_of_messages_with_mentions_oldest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindMentionsOldestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-4));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.AreEqual(2, userMessages.Count, "User must see correct amount of messages with mention oldest then specific date on his feed");
        }

        [Test]
        public void user_must_see_messages_with_mentions_in_correct_order_oldest_then_specific_date()
        {
            var userMessagesQuery = _userMessageService.FindMentionsOldestThan(DatabaseHelper.UserTest4.Id, DateTime.UtcNow.AddDays(-4));

            var userMessages = userMessagesQuery.Where(x => x.Message.Mind.StartsWith("UserMessageRepositoryTest")).ToList();
            Assert.IsTrue(userMessages[0].SortingDate > userMessages[1].SortingDate, "User must see messages with mention in correct order (1)");
        }

        private void CreateUserMessagesLists()
        {
            var usertest4 = DatabaseHelper.UserTest4;
            var usertest3 = DatabaseHelper.UserTest3;
            var usertest2A = DatabaseHelper.UserTest2A;
            var newMessage1 = DatabaseHelper.CreateValidMessage(usertest4, MessageStateEnum.Published, "UserMessageRepositoryTest1");
            var newMessage2 = DatabaseHelper.CreateValidMessage(usertest4, MessageStateEnum.Published, "UserMessageRepositoryTest2");
            var newMessage3 = DatabaseHelper.CreateValidMessage(usertest4, MessageStateEnum.Published, "UserMessageRepositoryTest3");
            var newMessage4 = DatabaseHelper.CreateValidMessage(usertest3, MessageStateEnum.Deleted, "UserMessageRepositoryTest4");
            var newMessage5 = DatabaseHelper.CreateValidMessage(usertest3, MessageStateEnum.Published, "UserMessageRepositoryTest5");
            var newMessage6 = DatabaseHelper.CreateValidMessage(usertest4, MessageStateEnum.Deleted, "UserMessageRepositoryTest6");
            var newMessage7 = DatabaseHelper.CreateValidMessage(usertest4, MessageStateEnum.Published, "UserMessageRepositoryTest7");
            var newMessage8 = DatabaseHelper.CreateValidMessage(usertest2A, MessageStateEnum.Published, "UserMessageRepositoryTest7");
            var newMessage9 = DatabaseHelper.CreateValidMessage(usertest2A, MessageStateEnum.Published, "UserMessageRepositoryTest7");
            var newMessage10 = DatabaseHelper.CreateValidMessage(usertest2A, MessageStateEnum.Published, "UserMessageRepositoryTest7");

            _messageRepository.Create(newMessage1);
            _messageRepository.Create(newMessage2);
            _messageRepository.Create(newMessage3);
            _messageRepository.Create(newMessage4);
            _messageRepository.Create(newMessage5);
            _messageRepository.Create(newMessage6);
            _messageRepository.Create(newMessage7);
            _messageRepository.Create(newMessage8);
            _messageRepository.Create(newMessage9);
            _messageRepository.Create(newMessage10);

            var userMessage1A = DatabaseHelper.CreateValidUserMessage(newMessage1, usertest4, modeEnum: UserMessageCreationModeEnum.ByHimselfNew);
            var userMessage2A = DatabaseHelper.CreateValidUserMessage(newMessage2, usertest4, modeEnum: UserMessageCreationModeEnum.ByHimselfNew);
            var userMessage3A = DatabaseHelper.CreateValidUserMessage(newMessage3, usertest4, modeEnum: UserMessageCreationModeEnum.ByHimselfNew);
            var userMessage4A = DatabaseHelper.CreateValidUserMessage(newMessage4, usertest3, modeEnum: UserMessageCreationModeEnum.ByHimselfNew, stateEnum: UserMessageStateEnum.Deleted);
            var userMessage5A = DatabaseHelper.CreateValidUserMessage(newMessage5, usertest3, modeEnum: UserMessageCreationModeEnum.ByHimselfNew);
            var userMessage6A = DatabaseHelper.CreateValidUserMessage(newMessage6, usertest4, modeEnum: UserMessageCreationModeEnum.ByHimselfNew, stateEnum: UserMessageStateEnum.Deleted);
            var userMessage7A = DatabaseHelper.CreateValidUserMessage(newMessage7, usertest4, modeEnum: UserMessageCreationModeEnum.ByHimselfNew);
            var userMessage8A = DatabaseHelper.CreateValidUserMessage(newMessage8, usertest2A, modeEnum: UserMessageCreationModeEnum.ByHimselfNew);
            var userMessage9A = DatabaseHelper.CreateValidUserMessage(newMessage9, usertest2A, modeEnum: UserMessageCreationModeEnum.ByHimselfNew);
            var userMessage10A = DatabaseHelper.CreateValidUserMessage(newMessage10, usertest2A, modeEnum: UserMessageCreationModeEnum.ByHimselfNew);

            _userMessageRepository.Create(userMessage1A);
            _userMessageRepository.Create(userMessage2A);
            _userMessageRepository.Create(userMessage3A);
            _userMessageRepository.Create(userMessage4A);
            _userMessageRepository.Create(userMessage5A);
            _userMessageRepository.Create(userMessage6A);
            _userMessageRepository.Create(userMessage7A);
            _userMessageRepository.Create(userMessage8A);
            _userMessageRepository.Create(userMessage9A);
            _userMessageRepository.Create(userMessage10A);

            userMessage1A.SortingDate = DateTime.UtcNow.AddDays(-10);
            userMessage2A.SortingDate = DateTime.UtcNow.AddDays(-7);
            userMessage3A.SortingDate = DateTime.UtcNow.AddDays(-5);
            userMessage4A.SortingDate = DateTime.UtcNow.AddDays(-4);
            userMessage5A.SortingDate = DateTime.UtcNow.AddDays(-3);
            userMessage6A.SortingDate = DateTime.UtcNow.AddDays(-2);
            userMessage7A.SortingDate = DateTime.UtcNow.AddDays(-1);
            userMessage8A.SortingDate = DateTime.UtcNow.AddDays(-9);
            userMessage9A.SortingDate = DateTime.UtcNow.AddDays(-5);
            userMessage10A.SortingDate = DateTime.UtcNow.AddDays(-2);

            var userMessage1B = DatabaseHelper.CreateValidUserMessage(newMessage1, usertest3);
            var userMessage2B = DatabaseHelper.CreateValidUserMessage(newMessage2, usertest3);
            var userMessage3B = DatabaseHelper.CreateValidUserMessage(newMessage3, usertest3);
            var userMessage4B = DatabaseHelper.CreateValidUserMessage(newMessage4, usertest4, modeEnum: UserMessageCreationModeEnum.ByObservedNew, stateEnum: UserMessageStateEnum.Deleted);
            var userMessage5B = DatabaseHelper.CreateValidUserMessage(newMessage5, usertest4);
            var userMessage6B = DatabaseHelper.CreateValidUserMessage(newMessage6, usertest3, modeEnum: UserMessageCreationModeEnum.ByObservedNew, stateEnum: UserMessageStateEnum.Deleted);
            var userMessage7B = DatabaseHelper.CreateValidUserMessage(newMessage7, usertest3);
            var userMessage8B = DatabaseHelper.CreateValidUserMessage(newMessage8, usertest4, modeEnum: UserMessageCreationModeEnum.ByNotObserved);
            var userMessage9B = DatabaseHelper.CreateValidUserMessage(newMessage9, usertest4, modeEnum: UserMessageCreationModeEnum.ByNotObserved);
            var userMessage10B = DatabaseHelper.CreateValidUserMessage(newMessage10, usertest4, modeEnum: UserMessageCreationModeEnum.ByNotObserved);

            _userMessageRepository.Create(userMessage1B);
            _userMessageRepository.Create(userMessage2B);
            _userMessageRepository.Create(userMessage3B);
            _userMessageRepository.Create(userMessage4B);
            _userMessageRepository.Create(userMessage5B);
            _userMessageRepository.Create(userMessage6B);
            _userMessageRepository.Create(userMessage7B);
            _userMessageRepository.Create(userMessage8B);
            _userMessageRepository.Create(userMessage9B);
            _userMessageRepository.Create(userMessage10B);

            userMessage1B.SortingDate = DateTime.UtcNow.AddDays(-10);
            userMessage2B.SortingDate = DateTime.UtcNow.AddDays(-7);
            userMessage3B.SortingDate = DateTime.UtcNow.AddDays(-5);
            userMessage4B.SortingDate = DateTime.UtcNow.AddDays(-4);
            userMessage5B.SortingDate = DateTime.UtcNow.AddDays(-3);
            userMessage6B.SortingDate = DateTime.UtcNow.AddDays(-2);
            userMessage7B.SortingDate = DateTime.UtcNow.AddDays(-1);
            userMessage8B.SortingDate = DateTime.UtcNow.AddDays(-9);
            userMessage9B.SortingDate = DateTime.UtcNow.AddDays(-5);
            userMessage10B.SortingDate = DateTime.UtcNow.AddDays(-2);
            userMessage8B.HaveMention = true;
            userMessage9B.HaveMention = true;
            userMessage10B.HaveMention = true;
        }
    }
}
