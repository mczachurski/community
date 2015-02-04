using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Services.Dict
{
    public interface ILanguageService
    {
        Language FindByCode(string code);
        IList<Language> FindAll();
        Language FindById(Guid id);
    }
}

