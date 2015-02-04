using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class UserMessageRepository : EntityRepository<UserMessage>, IUserMessageRepository
    {
        public UserMessageRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public bool IsUserHaveMessage(Guid userId, Guid messageId)
        {
            return FindAll().Any(x => x.User.Id == userId && x.Message.Id == messageId);
        }

        public UserMessage FindUserMessageWithMarker(Guid userId)
        {
            return FindAllMessages(userId).FirstOrDefault(x => x.IsMarkerSet);
        }

        public UserMessage FindUserMessage(Guid userId, Guid messageId)
        {
            return FindAll().FirstOrDefault(x => x.User.Id == userId && x.Message.Id == messageId);
        }

        public IQueryable<UserMessage> FindAll(Guid userId)
        {
            return FindAll().Where(x => x.User.Id == userId && x.UserMessageState.UserMessageStateEnum != UserMessageStateEnum.Deleted).OrderByDescending(x => x.SortingDate);
        }

        public IQueryable<UserMessage> FindAllMentions(Guid userId)
        {
            return FindAll(userId).Where(x => x.HaveMention);
        }

        public IQueryable<UserMessage> FindAllMessages(Guid userId)
        {
            return FindAll(userId).Where(x => x.UserMessageCreationMode.UserMessageCreationModeEnum != UserMessageCreationModeEnum.ByNotObserved);
        }
            
        public IQueryable<UserMessage> FindUserFeedMessagesNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            var queryable = FindAllMessages(userId).Where(x => x.SortingDate > fromDateTime);
            if (numberOfMessages != Int32.MaxValue)
            {
                queryable = queryable.Take(numberOfMessages);
            }

            return queryable;
        }

        public IQueryable<UserMessage> FindUserFeedMessagesOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            var queryable = FindAllMessages(userId).Where(x => x.SortingDate < fromDateTime);
            if (numberOfMessages != Int32.MaxValue)
            {
                queryable = queryable.Take(numberOfMessages);
            }

            return queryable;
        }

        public IQueryable<UserMessage> FindMentionsNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            var queryable = FindAllMentions(userId).Where(x => x.SortingDate > fromDateTime);
            if (numberOfMessages != Int32.MaxValue)
            {
                queryable = queryable.Take(numberOfMessages);
            }

            return queryable;
        }

        public IQueryable<UserMessage> FindMentionsOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            var queryable = FindAllMentions(userId).Where(x => x.SortingDate < fromDateTime);
            if (numberOfMessages != Int32.MaxValue)
            {
                queryable = queryable.Take(numberOfMessages);
            }

            return queryable;
        }

        public IQueryable<UserMessage> FindMessagesCreatedByUserNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            var queryable = FindAllMessagesCreatedByUser(userId).Where(x => x.SortingDate > fromDateTime);
            if (numberOfMessages != Int32.MaxValue)
            {
                queryable = queryable.Take(numberOfMessages);
            }

            return queryable;
        }

        public IQueryable<UserMessage> FindMessagesCreatedByUserOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue)
        {
            var queryable = FindAllMessagesCreatedByUser(userId).Where(x => x.SortingDate < fromDateTime);
            if (numberOfMessages != Int32.MaxValue)
            {
                queryable = queryable.Take(numberOfMessages);
            }

            return queryable;
        }  

        private IQueryable<UserMessage> FindAllMessagesCreatedByUser(Guid userId)
        {
            return FindAll(userId).Where(x => x.Message.User.Id == userId && x.UserMessageState.UserMessageStateEnum != UserMessageStateEnum.Deleted).OrderByDescending(x => x.SortingDate);
        }

        public void SendTransmit(Guid userId, Guid messageId)
        {
            DbSession.Current.ExecuteSqlCommand("[dbo].[SendTransmit] @messageId, @userId", 
                new SqlParameter("@messageId", messageId), 
                new SqlParameter("@userId", userId));
        }

        public void PublishMessage(Guid messageId, string userNameToReply, IList<string> mentionedUserNames)
        {
            string mentionedUsers = String.Join("|", mentionedUserNames);

            DbSession.Current.ExecuteSqlCommand("[dbo].[PublishMessage] @messageId, @userNameToReply, @mentionedUserNames",
                new SqlParameter("@messageId", messageId),
                new SqlParameter("@userNameToReply", userNameToReply == null ? DBNull.Value : (object)userNameToReply),
                new SqlParameter("@mentionedUserNames", mentionedUsers));
        }
    }
}