using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class MessageRepository : EntityRepository<Message>, IMessageRepository
    {
        public MessageRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public IList<Message> FindLastCommentsToMessage(Guid messageId)
        {
            const int numberOfLastComments = 3;
            var lastComments = FindAll().Where(x => x.CommentedMessage != null && x.CommentedMessage.Id == messageId)
                .OrderByDescending(x => x.CreationDate).Take(numberOfLastComments).OrderBy(x => x.CreationDate);

            return lastComments.ToList();
        }

        public bool WasMessageQuotedByUser(Guid messageId, Guid userId)
        {
            return FindAll().Any(x => x.QuotedMessage != null && x.QuotedMessage.Id == messageId && x.User.Id == userId 
                && x.MessageState.MessageStateEnum != SunLine.Community.Entities.Dict.MessageStateEnum.Deleted);
        }

        public bool WasMessageCommentedByUser(Guid messageId, Guid userId)
        {
            return FindAll().Any(x => x.CommentedMessage != null && x.CommentedMessage.Id == messageId && x.User.Id == userId 
                && x.MessageState.MessageStateEnum != SunLine.Community.Entities.Dict.MessageStateEnum.Deleted);
        }

        public int AmountOfMessages(Guid userId, bool excludeComments)
        {
            var amountOfMessageQuery = FindAll().Where(x => x.User.Id == userId && x.MessageState.MessageStateEnum != SunLine.Community.Entities.Dict.MessageStateEnum.Deleted);
            if (excludeComments)
            {
                amountOfMessageQuery = amountOfMessageQuery.Where(x => x.CommentedMessage == null);
            }

            return amountOfMessageQuery.Count();
        }

        public void DeleteMessage(Guid messageId)
        {
            DbSession.Current.ExecuteSqlCommand("[dbo].[DeleteMessage] @messageId", 
                new SqlParameter("@messageId", messageId));
        }
    }
}