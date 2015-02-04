using System;
using System.Collections.Generic;

namespace SunLine.Community.Web.ViewModels.Users
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            SelectedCatagories = new List<Guid>();
        }

        public Guid LevelId { get; set; }
        public string LevelName { get; set; }
        public int FavouriteLevel { get ; set; }
        public IList<Guid> SelectedCatagories { get ;set; }
    }
}

