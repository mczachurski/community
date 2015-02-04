using System;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Common;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Services.Core;
using SunLine.Community.Services.Dict;
using SunLine.Community.Web.Common;
using SunLine.Community.Web.ViewModels.Messages;
using SunLine.Community.Web.ViewModels.Files;

namespace SunLine.Community.Web.ViewModelsServices
{
    [ViewModelService]
    public class MessagesViewModelService : IMessagesViewModelService
    {
        private readonly IUserMessageService _userMessageService;
        private readonly IMessageService _messageService;
        private readonly IMessageFavouriteService _messageFavouriteService;
        private readonly IFileService _fileService;

        public MessagesViewModelService(
            IUserMessageService userMessageService, 
            IMessageService messageService,
            IMessageFavouriteService messageFavouriteService,
            IFileService fileService)
        {
            _userMessageService = userMessageService;
            _messageService = messageService;
            _messageFavouriteService = messageFavouriteService;
            _fileService = fileService;
        }

        public TimelineViewModel CreateUserTimelineViewModel(Guid timelineUserId, Guid watcherUserId)
        {
            const int numberOfMessages = 20;
            var userMessages = _userMessageService.FindUserFeedMessagesOldestThan(timelineUserId, DateTime.UtcNow, numberOfMessages);
            return CreateTimelineViewModel(timelineUserId, watcherUserId, userMessages.ToList());
        }

        public TimelineViewModel CreateMentionTimelineViewModel(Guid timelineUserId, Guid watcherUserId)
        {
            const int numberOfMessages = 20;
            var userMessages = _userMessageService.FindMentionsOldestThan(timelineUserId, DateTime.UtcNow, numberOfMessages);
            return CreateTimelineViewModel(timelineUserId, watcherUserId, userMessages.ToList());
        }

        public TimelineViewModel CreateProfileTimelineViewModel(Guid timelineUserId, Guid watcherUserId)
        {
            const int numberOfMessages = 20;
            var userMessages = _userMessageService.FindMessagesCreatedByUserOldestThan(timelineUserId, DateTime.UtcNow, numberOfMessages);
            return CreateTimelineViewModel(timelineUserId, watcherUserId, userMessages.ToList());
        }

        public TimelineViewModel CreateTimelineViewModel(Guid? timelineUserId, Guid watcherUserId, IList<UserMessage> userMessages)
        {
            var timelineViewModel = new TimelineViewModel();
            foreach (var userMessage in userMessages)
            {
                var messageViewModel = CreateMessageViewModel(userMessage.Message, timelineUserId, watcherUserId, true);
                timelineViewModel.MessageViewModels.Add(messageViewModel);

                IList<Message> lastComments = _messageService.FindLastCommentsToMessage(userMessage.Message.Id);
                foreach (var lastComment in lastComments)
                {
                    var commentMessageViewModel = CreateMessageViewModel(lastComment, timelineUserId, watcherUserId);
                    messageViewModel.Comments.Add(commentMessageViewModel);
                }
            }

            return timelineViewModel;
        }

        public MessageViewModel CreateMessageViewModel(Message message, Guid? timelineUserId, Guid watcherUserId, bool includeQutedMessage = false)
        {
            var author = message.User;
            var messageViewModel = new MessageViewModel
                {
                    MessageId = message.Id,
                    AuthorFullName = author.FullName,
                    AuthorGravatarHash = author.GravatarHash,
                    AuthorId = author.Id,
                    AuthorUserName = author.UserName,
                    Mind = message.Mind,
                    Speech = message.Speech,
                    CreationDate = message.CreationDate,
                    IsAuthorOfMessage = message.User.Id == watcherUserId
                };

            if (includeQutedMessage && message.QuotedMessage != null)
            {
                messageViewModel.QuotedMessage = CreateMessageViewModel(message.QuotedMessage, timelineUserId, watcherUserId);
            }

            if (!string.IsNullOrWhiteSpace(message.Speech))
            {
                messageViewModel.HasSpeech = true;
                messageViewModel.SpeechReadingTime = message.Speech.ReadingTime();
            }

            messageViewModel.AmountOfQuotes = message.AmountOfQuotes;
            messageViewModel.AmountOfFavourites = message.AmountOfFavourites;
            messageViewModel.AmountOfTransmitted = message.AmountOfTransmitted;
            messageViewModel.AmountOfComments = message.AmountOfComments;
            messageViewModel.WasMessageQuotedByUser = _messageService.WasMessageQuotedByUser(message.Id, watcherUserId);
            messageViewModel.WasMessageCommentedByUser = _messageService.WasMessageCommentedByUser(message.Id, watcherUserId);
            messageViewModel.IsUserFavouriteMessage = _messageFavouriteService.IsUserFavouriteMessage(watcherUserId, message.Id);

            UserMessage watcherUserMessage = _userMessageService.FindUserMessage(watcherUserId, message.Id);
            if (watcherUserMessage != null)
            {
                messageViewModel.IsMassageVisibleOnWatcherTimeline = true;
                messageViewModel.WasMessageTransmittedByUser = watcherUserMessage.WasTransmitted;
                messageViewModel.UpdateSortingDateOnNewComment = watcherUserMessage.UpdateSortingDateOnNewComment;
            }

            if (timelineUserId.HasValue)
            {
                UserMessage timelineUserMessage = _userMessageService.FindUserMessage(timelineUserId.Value, message.Id);
                if (timelineUserMessage != null && timelineUserMessage.TransmittedUserMessage != null)
                {
                    messageViewModel.IsTransmittedToUser = true;
                    messageViewModel.TransmittedUserId = timelineUserMessage.TransmittedUserMessage.User.Id;
                    messageViewModel.TransmittedUserFullName = timelineUserMessage.TransmittedUserMessage.User.FullName;
                    messageViewModel.TransmittedUserName = timelineUserMessage.TransmittedUserMessage.User.UserName;
                }
            }

            if(message.Files != null && message.Files.Count > 0)
            {
                foreach (var file in message.Files)
                {
                    var fileViewModel = new FileViewModel {
                        FileUrl = _fileService.GetUrlToFile(file),
                        ThumbnailUrl = _fileService.GetUrlToFileThumbnail(file)
                    };

                    messageViewModel.Files.Add(fileViewModel);
                }
            }

            return messageViewModel;
        }
    }
}

