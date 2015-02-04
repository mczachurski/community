using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Core;
using SunLine.Community.Web.ViewModels.Messages;

namespace SunLine.Community.Web.ViewModelsServices
{
    public interface IMessagesViewModelService
    {
        TimelineViewModel CreateUserTimelineViewModel(Guid timelineUserId, Guid watcherUserId);
        TimelineViewModel CreateMentionTimelineViewModel(Guid timelineUserId, Guid watcherUserId);
        TimelineViewModel CreateProfileTimelineViewModel(Guid timelineUserId, Guid watcherUserId);
        TimelineViewModel CreateTimelineViewModel(Guid? timelineUserId, Guid watcherUserId, IList<UserMessage> userMessages);
        MessageViewModel CreateMessageViewModel(Message message, Guid? timelineUserId, Guid watcherUserId, bool includeQutedMessage = false);
    }
}
