using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Entities.Core
{
    public class File : BaseEntity
    {
        public File()
        {
            Messages = new List<Message>();
        }

        [Required]
        public virtual User User { get; set; }

        [Required]
        [StringLength(500)]
        public virtual string Path { get; set; }

        [Required]
        [StringLength(100)]
        public virtual string ContentType { get; set; }

        [Required]
        public virtual int ContentLength { get; set; }

        [Required]
        public virtual int Identifier { get; set; }

        [Required]
        public virtual FileType FileType { get; set; }

        public virtual IList<Message> Messages { get; set; }
    }
}
