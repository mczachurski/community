using System;
using System.Collections.Generic;
using SunLine.Community.Web.ViewModels.Files;

namespace SunLine.Community.Web.ViewModels.Messages
{
    public class MessageViewModel
    {
        public MessageViewModel()
        {
            Comments = new List<MessageViewModel>();
            Files = new List<FileViewModel>();
        }

        public Guid MessageId { get; set; }
        public string AuthorFullName { get; set; }
        public string AuthorUserName { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorGravatarHash { get; set; }
        public string Mind { get; set; }
        public string Speech { get; set; }
        public bool IsTransmittedToUser { get; set; }
        public Guid TransmittedUserId { get; set; }
        public string TransmittedUserName { get; set; }
        public string TransmittedUserFullName { get; set; }
        public DateTime CreationDate { get; set; }

        public int AmountOfQuotes { get; set; }
        public int AmountOfFavourites { get; set; }
        public int AmountOfTransmitted { get; set; }
        public int AmountOfComments { get; set; }

        public MessageViewModel QuotedMessage { get; set; }
        public IList<MessageViewModel> Comments { get; set; }
        public bool WasMessageQuotedByUser { get; set; }
        public bool WasMessageCommentedByUser { get; set; }
        public bool IsUserFavouriteMessage { get; set; }
        public bool WasMessageTransmittedByUser { get; set; }
        public bool IsAuthorOfMessage { get; set; }
        public bool HasSpeech { get; set; }
        public int SpeechReadingTime { get; set; }
        public bool UpdateSortingDateOnNewComment { get; set; }
        public bool IsMassageVisibleOnWatcherTimeline { get; set; }
        public IList<FileViewModel> Files { get; set;}
    }
}
