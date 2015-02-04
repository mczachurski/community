using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunLine.Community.Entities
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        [DefaultValue(1)]
        public virtual int Version { get; set; }

        public virtual DateTime CreationDate { get; set; }

        public virtual DateTime? ModificationDate { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
