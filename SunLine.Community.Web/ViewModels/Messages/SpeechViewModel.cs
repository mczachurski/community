using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Web.ViewModels.Messages
{
    public class SpeechViewModel
    {
        public Guid MessageId { get; set; }
        public string MessageStateName { get; set; }
        public MessageStateEnum MessageStateEnum { get; set; }

        [Required]
        [DisplayName("Mind")]
        public string Mind { get; set; }

        [DisplayName("Speech")]
        public virtual string Speech { get; set; }
    }
}

