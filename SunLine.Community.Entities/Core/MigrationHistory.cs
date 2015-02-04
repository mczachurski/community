using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunLine.Community.Entities.Core
{
    public class MigrationHistory
    {
        [MaxLength(150)]
        [Required]
        [Key]
        [Column(Order = 1)] 
        public virtual string MigrationId { get; set; }

        [MaxLength(300)]
        [Required]
        [Key]
        [Column(Order = 2)] 
        public virtual string ContextKey { get; set; }

        [Required]
        public virtual byte[] Model { get; set; }

        [MaxLength(32)]
        [Required]
        public virtual string ProductVersion { get; set; }
    }
}
