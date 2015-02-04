using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Core
{
    public class MessageHashtag : BaseEntity
    {
        [Required]
        public virtual Message Message { get; set; }

        [Required]
        public virtual Hashtag Hashtag { get; set; }

        [Required]
        public virtual int Index { get; set; }

        [Required]
        public virtual int Length { get; set; }
    }
}
