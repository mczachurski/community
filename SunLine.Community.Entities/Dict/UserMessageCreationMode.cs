using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Dict
{
    public class UserMessageCreationMode : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }

        [Required]
        public virtual UserMessageCreationModeEnum UserMessageCreationModeEnum { get; set; }
    }
}
