using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Services.Dict;

namespace SunLine.Community.NUnitTests.Services.Dict
{
    [TestFixture]
    public class LanguageServiceTest : BaseTest
    {
        [Test]
        public void service_must_return_expected_languages()
        {
            var languageService = ServiceLocator.Current.GetInstance<ILanguageService>();

            IList<Language> languages = languageService.FindAll();

            Assert.AreEqual(136, languages.Count, "Service must return expected list of languages");
        }

        [Test]
        public void service_must_return_expected_language_by_id()
        {
            var languageService = ServiceLocator.Current.GetInstance<ILanguageService>();
            var language = languageService.FindAll().FirstOrDefault();

            Language languageFromDb = languageService.FindById(language.Id);

            Assert.AreEqual(language.Id, languageFromDb.Id, "Service must return expected language by id");
        }

        [Test]
        public void service_must_return_expected_language_by_code()
        {
            var languageService = ServiceLocator.Current.GetInstance<ILanguageService>();

            Language categoryFromDb = languageService.FindByCode("PL");

            Assert.AreEqual("PL", categoryFromDb.Code, "Service must return expected language by code");
        }
    }
}

