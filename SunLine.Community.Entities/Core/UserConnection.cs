using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Core
{
    public class UserConnection : BaseEntity
    {
        [Required]
        public virtual User FromUser { get; set; }

        [Required]
        public virtual User ToUser { get; set; }
    }
}
