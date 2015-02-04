using System;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Dict;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Infrastructure;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageStateRepository _messageStateRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageUrlService _messageUrlService;
        private readonly IFileRepository _fileRepository;
        private readonly IDatabaseTransactionService _databaseTransactionService;

        public MessageService(
            IMessageRepository messageRepository, 
            IMessageStateRepository messageStateRepository, 
            IUserRepository userRepository,
            IMessageUrlService messageUrlService, 
            IFileRepository fileRepository,
            IDatabaseTransactionService databaseTransactionService)
        {
            _messageRepository = messageRepository;
            _messageStateRepository = messageStateRepository;
            _userRepository = userRepository;
            _messageUrlService = messageUrlService;
            _fileRepository = fileRepository;
            _databaseTransactionService = databaseTransactionService;
        }

        public Message CreateQuote(string mind, Guid userId, Guid quotedMessageId)
        {
            Message quotedMessage = _messageRepository.FindById(quotedMessageId);
            if(quotedMessage == null)
            {
                throw new ArgumentException(string.Format("Quoted message: '{0}' not exists.", userId));
            }

            Message createdMessage = CreateMessage(mind, null, userId, quotedMessage, null);
            return createdMessage;
        }

        public Message CreateComment(string mind, Guid userId, Guid commentedMessageId)
        {
            Message commentedMessage = _messageRepository.FindById(commentedMessageId);
            if(commentedMessage == null)
            {
                throw new ArgumentException(string.Format("Commented message: '{0}' not exists.", userId));
            }

            Message createdMessage = CreateMessage(mind, null, userId, null, commentedMessage);
            return createdMessage;
        }

        public Message CreateMind(string mind, Guid userId, IList<Guid> fileIds = null)
        {
            Message createdMessage = CreateMessage(mind, null, userId, null, null);
            ConnectMessageWithFiles(createdMessage, fileIds);

            return createdMessage;
        }

        public Message CreateSpeech(string mind, string speech, Guid userId)
        {
            Message createdMessage = CreateMessage(mind, speech, userId, null, null);
            return createdMessage;
        }

        public Message UpdateSpeech(Guid messageId, string mind, string speech)
        {
            Message message = _messageRepository.FindById(messageId);
            if(message == null)
            {
                throw new ArgumentException(string.Format("Message '{0}' not exists", messageId));
            }

            IList<MessageUrl> messageUrls;
            mind = _messageUrlService.ParseMindUrl(mind, out messageUrls);
            if (!IsValidMind(mind))
            {
                return null;
            }

            message.Mind = mind;
            message.Speech = speech;

            _messageRepository.Update(message);
            _messageUrlService.CreateMessageUrls(message, messageUrls);

            return message;
        }

        private void ConnectMessageWithFiles(Message message, IList<Guid> fileIds)
        {
            if (fileIds != null)
            {
                foreach (var fileId in fileIds)
                {
                    File file = _fileRepository.FindById(fileId);
                    if (file != null)
                    {
                        file.Messages.Add(message);
                        message.Files.Add(file);

                        _fileRepository.Update(file);
                    }
                }
            }
        }

        private Message CreateMessage(string mind, string speech, Guid userId, Message quotedMessage, Message commentedMessage)
        {
            User user = _userRepository.FindById(userId);
            if(user == null)
            {
                throw new ArgumentException(string.Format("User: '{0}' not exists.", userId));
            }

            IList<MessageUrl> messageUrls;
            mind = _messageUrlService.ParseMindUrl(mind, out messageUrls);
            if (!IsValidMind(mind))
            {
                return null;
            }

            var message = new Message();
            message.Mind = mind;
            message.Speech = speech;
            message.User = user;
            message.Language = user.Language;
            message.QuotedMessage = quotedMessage;
            message.CommentedMessage = commentedMessage;
            message.MessageState = _messageStateRepository.FindByEnum(MessageStateEnum.Draft);

            message = _messageRepository.Create(message);
            _messageUrlService.CreateMessageUrls(message, messageUrls);

            return message;
        }

        public Message FindById(Guid id)
        {
            return _messageRepository.FindById(id);
        }

        public bool WasMessageQuotedByUser(Guid messageId, Guid userId)
        {
            return _messageRepository.WasMessageQuotedByUser(messageId, userId);
        }

        public bool WasMessageCommentedByUser(Guid messageId, Guid userId)
        {
            return _messageRepository.WasMessageCommentedByUser(messageId, userId);
        }

        public int AmountOfMessages(Guid userId, bool excludeComments)
        {
            return _messageRepository.AmountOfMessages(userId, excludeComments);
        }

        public IList<Message> FindLastCommentsToMessage(Guid messageId)
        {
            return _messageRepository.FindLastCommentsToMessage(messageId);
        }

        public bool HasRightToMessage(Guid userId, Guid messageId)
        {
            return _messageRepository.FindAll().Any(x => x.User.Id == userId && x.Id == messageId);
        }

        public void Delete(Guid messageId)
        {
            using (var databaseTransaction = _databaseTransactionService.BeginTransaction())
            {
                try
                {
                    _messageRepository.DeleteMessage(messageId);
                    databaseTransaction.Commit();
                }
                catch (Exception)
                {
                    databaseTransaction.Rollback();
                    throw;
                }
            } 
        }

        public void ReloadMessage(Message message)
        {
            _messageRepository.ReloadEntity(message);
        }

        public IList<Message> FindAllSpeeches(Guid userId)
        {
            return _messageRepository.FindAll().Where(x => x.User.Id == userId && x.MessageState.MessageStateEnum != MessageStateEnum.Deleted 
                && x.Speech != null).OrderByDescending(x => x.CreationDate).ToList();
        }

        private bool IsValidMind(string message)
        {
            return !string.IsNullOrWhiteSpace(message) && message.Length <= 200;
        }
    }
}