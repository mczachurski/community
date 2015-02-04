using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Dict
{
    public class CategoryFavouriteLevel : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }

        [Required]
        [DefaultValue(0)]
        public virtual int FavouriteLevel { get; set; }
    }
}

