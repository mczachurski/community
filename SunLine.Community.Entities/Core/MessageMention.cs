using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Core
{
    public class MessageMention : BaseEntity
    {
        [Required]
        public virtual Message Message { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual int Index { get; set; }

        [Required]
        public virtual int Length { get; set; }
    }
}
