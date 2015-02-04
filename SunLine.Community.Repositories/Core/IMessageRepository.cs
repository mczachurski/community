using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public interface IMessageRepository : IEntityRepository<Message>
    {
        IList<Message> FindLastCommentsToMessage(Guid messageId);
        bool WasMessageQuotedByUser(Guid messageId, Guid userId);
        bool WasMessageCommentedByUser(Guid messageId, Guid userId);
        int AmountOfMessages(Guid userId, bool excludeComments);
        void DeleteMessage(Guid messageId);
    }
}