using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Services.Dict;

namespace SunLine.Community.NUnitTests.Services.Dict
{
    [TestFixture]
    public class CategoryServiceTest : BaseTest
    {
        [Test]
        public void service_must_return_expected_categories()
        {
            var categoryService = ServiceLocator.Current.GetInstance<ICategoryService>();

            IList<Category> categories = categoryService.FindAll();

            Assert.AreEqual(27, categories.Count, "Service must return expected list of categories");
        }

        [Test]
        public void service_must_return_expected_category_by_id()
        {
            var categoryService = ServiceLocator.Current.GetInstance<ICategoryService>();
            var category = categoryService.FindAll().FirstOrDefault();

            Category categoryFromDb = categoryService.FindById(category.Id);

            Assert.AreEqual(category.Id, categoryFromDb.Id, "Service must return expected category by id");
        }
    }
}

