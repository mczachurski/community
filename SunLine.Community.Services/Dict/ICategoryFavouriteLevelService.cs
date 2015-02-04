using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Services.Dict
{
    public interface ICategoryFavouriteLevelService
    {
        CategoryFavouriteLevel FindById(Guid id);
        IList<CategoryFavouriteLevel> FindAll();
    }
}

