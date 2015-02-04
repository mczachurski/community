using System;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public interface IUserMessageRepository : IEntityRepository<UserMessage>
    {
        void SendTransmit(Guid userId, Guid messageId);
        void PublishMessage(Guid messageId, string userNameToReply, IList<string> mentionedUserNames);
        bool IsUserHaveMessage(Guid userId, Guid messageId);

        UserMessage FindUserMessageWithMarker(Guid userId);
        UserMessage FindUserMessage(Guid userId, Guid messageId);
        IQueryable<UserMessage> FindAll(Guid userId);

        IQueryable<UserMessage> FindAllMentions(Guid userId);
        IQueryable<UserMessage> FindAllMessages(Guid userId);

        IQueryable<UserMessage> FindUserFeedMessagesNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
        IQueryable<UserMessage> FindUserFeedMessagesOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);

        IQueryable<UserMessage> FindMentionsNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
        IQueryable<UserMessage> FindMentionsOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);

        IQueryable<UserMessage> FindMessagesCreatedByUserNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
        IQueryable<UserMessage> FindMessagesCreatedByUserOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
    }
}