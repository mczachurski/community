using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Dict
{
    public class FileType : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }

        [Required]
        public virtual FileTypeEnum FileTypeEnum { get; set; }
    }
}
