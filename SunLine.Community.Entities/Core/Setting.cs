using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Core
{
    public class Setting : BaseEntity
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Value);
        }
    }
}
