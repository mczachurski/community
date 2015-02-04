using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Core
{
    public class MessageFavourite : BaseEntity
    {
        [Required]
        public virtual Message Message { get; set; }

        [Required]
        public virtual User User { get; set; }
    }
}
