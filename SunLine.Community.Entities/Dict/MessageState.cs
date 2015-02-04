using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Entities.Dict
{
    public class MessageState : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }

        [Required]
        public virtual MessageStateEnum MessageStateEnum { get; set; }

        public virtual IList<Message> Messages { get; set; }
    }
}
