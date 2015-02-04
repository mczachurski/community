using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Services.Dict
{
    public interface ICategoryService
    {
        IList<Category> FindAll();
        Category FindById(Guid id);
    }
}

