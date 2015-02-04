using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface IMessageService
    {
        Message CreateQuote(string mind, Guid userId, Guid quotedMessageId);
        Message CreateComment(string mind, Guid userId, Guid commentedMessageId);
        Message CreateMind(string mind, Guid userId, IList<Guid> fileIds = null);
        Message CreateSpeech(string mind, string speech, Guid userId);
        Message UpdateSpeech(Guid messageId, string mind, string speech);

        Message FindById(Guid id);
        IList<Message> FindLastCommentsToMessage(Guid messageId);
        void Delete(Guid messageId); 
        bool HasRightToMessage(Guid userId, Guid messageId);
        bool WasMessageQuotedByUser(Guid messageId, Guid userId);
        bool WasMessageCommentedByUser(Guid messageId, Guid userId);
        int AmountOfMessages(Guid userId, bool excludeComments);
        void ReloadMessage(Message message);
        IList<Message> FindAllSpeeches(Guid userId);
    }
}
