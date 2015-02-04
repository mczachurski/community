using System.ComponentModel.DataAnnotations;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Entities.Core
{
    public class UserCategory : BaseEntity
    {
        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        [Required]
        public virtual CategoryFavouriteLevel CategoryFavouriteLevel { get; set; } 
    }
}
