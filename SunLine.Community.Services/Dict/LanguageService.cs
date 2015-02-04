using System;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Dict;

namespace SunLine.Community.Services.Dict
{
    [BusinessLogic]
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public Language FindByCode(string code)
        {
            return _languageRepository.FindByCode(code);
        }

        public IList<Language> FindAll()
        {
            return _languageRepository.FindAll().ToList();
        }

        public Language FindById(Guid id)
        {
            return _languageRepository.FindById(id);
        }
    }
}
