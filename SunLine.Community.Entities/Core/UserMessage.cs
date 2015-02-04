using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Entities.Core
{
    public class UserMessage : BaseEntity
    {
        [Required]
        public virtual Message Message { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual UserMessageCreationMode UserMessageCreationMode { get; set; }

        [Required]
        public virtual UserMessageState UserMessageState { get; set; }

        [Required]
        public virtual bool IsMarkerSet { get; set; }

        public virtual UserMessage TransmittedUserMessage { get; set; }

        public virtual IList<UserMessage> TransmittedUserMessages { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool HaveMention { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool HaveMentionInComments { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool WasTransmitted { get; set; }

        [Index]
        [Required]
        public virtual DateTime SortingDate { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool UpdateSortingDateOnNewComment { get; set; }
    }
}
