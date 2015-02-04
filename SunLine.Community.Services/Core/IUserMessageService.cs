using System;
using SunLine.Community.Entities.Core;
using System.Linq;

namespace SunLine.Community.Services.Core
{
    public interface IUserMessageService
    {
        void PublishMessage(Message message);
        void SendTransmit(Guid userId, Guid messageId);
        bool ToggleBubble(Guid userId, Message message);
        bool HasRightToUserMessage(Guid userId, Guid userMessageId);

        UserMessage FindUserMessageWithMarker(Guid userId);
        UserMessage FindUserMessage(Guid userId, Guid messageId);

        IQueryable<UserMessage> FindUserFeedMessagesNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
        IQueryable<UserMessage> FindUserFeedMessagesOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
        IQueryable<UserMessage> FindMessagesCreatedByUserNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
        IQueryable<UserMessage> FindMessagesCreatedByUserOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
        IQueryable<UserMessage> FindMentionsNewestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
        IQueryable<UserMessage> FindMentionsOldestThan(Guid userId, DateTime fromDateTime, int numberOfMessages = Int32.MaxValue);
    }
}
