using System;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Dict;

namespace SunLine.Community.Services.Dict
{
    [BusinessLogic]
    public class CategoryFavouriteLevelService : ICategoryFavouriteLevelService
    {
        private readonly ICategoryFavouriteLevelRepository _categoryFavouriteLevelRepository;

        public CategoryFavouriteLevelService(ICategoryFavouriteLevelRepository categoryFavouriteLevelRepository)
        {
            _categoryFavouriteLevelRepository = categoryFavouriteLevelRepository;
        }

        public CategoryFavouriteLevel FindById(Guid id)
        {
            return _categoryFavouriteLevelRepository.FindById(id);
        }

        public IList<CategoryFavouriteLevel> FindAll()
        {
            return _categoryFavouriteLevelRepository.FindAll().ToList();
        }
    }
}
