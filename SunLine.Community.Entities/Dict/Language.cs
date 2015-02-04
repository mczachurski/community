using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Entities.Dict
{
    public class Language : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(5)]
        public virtual string Code { get; set; }

        public virtual IList<User> UserMessageLanguages { get; set; }
    }
}
