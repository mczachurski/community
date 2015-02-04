using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Dict
{
    public class UserMessageState : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }

        [Required]
        public virtual UserMessageStateEnum UserMessageStateEnum { get; set; }
    }
}
