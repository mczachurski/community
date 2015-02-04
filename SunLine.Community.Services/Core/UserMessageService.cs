using System;
using System.Linq;
using SunLine.Community.Common;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Infrastructure;
using SunLine.Community.Services.Search;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class UserMessageService : IUserMessageService
    {
        private readonly IUserMessageRepository _userMessageRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageHashtagService _messageHashtagService;
        private readonly IMessageMentionService _messageMentionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDatabaseTransactionService _databaseTransactionService;
        private readonly ISearchService _searchService;

        public UserMessageService(
            IUserMessageRepository userMessageRepository,
            IMessageRepository messageRepository,
            IMessageHashtagService messageHashtagService,
            IMessageMentionService messageMentionService, 
            IUnitOfWork unitOfWork,
            IDatabaseTransactionService databaseTransactionService,
            ISearchService searchService)
        {
            _userMessageRepository = userMessageRepository;
            _messageRepository = messageRepository;
            _messageHashtagService = messageHashtagService;
            _messageMentionService = messageMentionService;
            _unitOfWork = unitOfWork;
            _databaseTransactionService = databaseTransactionService;
            _searchService = searchService;
        }

        public IQueryable<UserMessage> FindUserFeedMessagesNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            return _userMessageRepository.FindUserFeedMessagesNewestThan(userId, fromDateTime, numberOfMessages);
        }

        public IQueryable<UserMessage> FindUserFeedMessagesOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            return _userMessageRepository.FindUserFeedMessagesOldestThan(userId, fromDateTime, numberOfMessages);
        }

        public IQueryable<UserMessage> FindMessagesCreatedByUserNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            return _userMessageRepository.FindMessagesCreatedByUserNewestThan(userId, fromDateTime, numberOfMessages);
        }

        public IQueryable<UserMessage> FindMessagesCreatedByUserOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            return _userMessageRepository.FindMessagesCreatedByUserOldestThan(userId, fromDateTime, numberOfMessages);
        }

        public IQueryable<UserMessage> FindMentionsNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            return _userMessageRepository.FindMentionsNewestThan(userId, fromDateTime, numberOfMessages);
        }

        public IQueryable<UserMessage> FindMentionsOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            return _userMessageRepository.FindMentionsOldestThan(userId, fromDateTime, numberOfMessages);
        }

        public UserMessage FindUserMessageWithMarker(Guid userId)
        {
            return _userMessageRepository.FindUserMessageWithMarker(userId);
        }

        public UserMessage FindUserMessage(Guid userId, Guid messageId)
        {
            return _userMessageRepository.FindUserMessage(userId, messageId);
        }

        public bool HasRightToUserMessage(Guid userId, Guid userMessageId)
        {
            return _userMessageRepository.FindAll(userId).Any(x => x.Id == userMessageId);
        }

        public void PublishMessage(Message message)
        {
            using (var databaseTransaction = _databaseTransactionService.BeginTransaction())
            {
                try
                {
                    string userNameToReply = message.Mind.GetUserNameToReply();
                    var mentionedUserNames = message.Mind.GetUserNames();
                    _userMessageRepository.PublishMessage(message.Id, userNameToReply, mentionedUserNames);

                    _messageHashtagService.CreateHashtags(message);
                    _messageMentionService.CreateMentions(message);
                    _searchService.AddMessageToIndex(message);
                    _messageRepository.Update(message);

                    _unitOfWork.Commit();
                    databaseTransaction.Commit();
                }
                catch (Exception)
                {
                    databaseTransaction.Rollback();
                    throw;
                }
            } 
        }
            
        public void SendTransmit(Guid userId, Guid messageId)
        {
            using (var databaseTransaction = _databaseTransactionService.BeginTransaction())
            {
                try
                {
                    _userMessageRepository.SendTransmit(userId, messageId);
                    databaseTransaction.Commit();
                }
                catch (Exception)
                {
                    databaseTransaction.Rollback();
                    throw;
                }
            } 
            
        }

        public bool ToggleBubble(Guid userId, Message message)
        {
            var userMessage = _userMessageRepository.FindAll().FirstOrDefault(x => x.User.Id == userId && x.Message.Id == message.Id);
            if (userMessage != null)
            {
                userMessage.UpdateSortingDateOnNewComment = !userMessage.UpdateSortingDateOnNewComment;
                _userMessageRepository.Update(userMessage);
                return userMessage.UpdateSortingDateOnNewComment;
            }

            return false;
        }
    }
}