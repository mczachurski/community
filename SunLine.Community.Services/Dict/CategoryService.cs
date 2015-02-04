using System;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Dict;

namespace SunLine.Community.Services.Dict
{
    [BusinessLogic]
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
            
        public IList<Category> FindAll()
        {
            return _categoryRepository.FindAll().ToList();
        }

        public Category FindById(Guid id)
        {
            return _categoryRepository.FindById(id);
        }
    }
}
