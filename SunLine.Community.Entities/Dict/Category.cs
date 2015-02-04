using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Entities.Dict
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }
        public virtual IList<UserCategory> UserCategories { get; set; }
        public virtual IList<Message> Messages { get; set; }
    }
}
