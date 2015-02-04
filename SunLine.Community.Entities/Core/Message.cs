using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Entities.Core
{
    public class Message : BaseEntity
    {
        public Message()
        {
            Files = new List<File>();
        }

        [Required]
        [StringLength(200)]
        public virtual string Mind { get; set; }

        public virtual string Speech { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual Language Language { get; set; }

        public virtual IList<Category> Categories { get; set; }

        public virtual IList<UserMessage> UserMessages { get; set; }

        public virtual IList<File> Files { get; set; }

        [DefaultValue(0)]
        public virtual int AmountOfFavourites { get; set; }

        [Required]
        public virtual MessageState MessageState { get; set; }

        public virtual IList<MessageHashtag> MessageHashtags { get; set; }

        public virtual IList<MessageMention> MessageMentions { get; set; }

        public virtual IList<MessageUrl> MessageUrls { get; set; }

        [Required]
        [DefaultValue(0)]
        public virtual int AmountOfTransmitted { get; set; }

        public virtual Message CommentedMessage { get; set; }

        public virtual IList<Message> Comments { get; set; }

        public virtual Message QuotedMessage { get; set; }

        public virtual IList<Message> Quotes { get; set; }

        public virtual IList<MessageFavourite> FavouritesFromUsers { get; set; }

        [Required]
        [DefaultValue(0)]
        public virtual int AmountOfQuotes { get; set; }

        [Index]
        public override DateTime CreationDate { get; set; }

        [DefaultValue(0)]
        public virtual int AmountOfComments { get; set; }
    }
}
