using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunLine.Community.Entities.Core
{
    public class Hashtag : BaseEntity
    {
        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public virtual string Name { get; set; }

        public virtual IList<MessageHashtag> MessageHashtags { get; set; }
    }
}
