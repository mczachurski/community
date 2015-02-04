using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Services.Dict;

namespace SunLine.Community.NUnitTests.Services.Dict
{
    [TestFixture]
    public class CategoryFavouriteLevelServiceTest : BaseTest
    {
        [Test]
        public void service_must_return_expected_favourite_levels()
        {
            var favouriteLevelService = ServiceLocator.Current.GetInstance<ICategoryFavouriteLevelService>();

            IList<CategoryFavouriteLevel> levels = favouriteLevelService.FindAll();

            Assert.AreEqual(3, levels.Count, "Service must return expected list of category favourite level");
            Assert.IsTrue(levels.Any(x => x.FavouriteLevel == 100), "Service must return category favourite level with level == 100");
            Assert.IsTrue(levels.Any(x => x.FavouriteLevel == 1000), "Service must return category favourite level with level == 1000");
            Assert.IsTrue(levels.Any(x => x.FavouriteLevel == 10000), "Service must return category favourite level with level == 10000");
        }

        [Test]
        public void service_must_return_expected_favourite_level_by_id()
        {
            var favouriteLevelService = ServiceLocator.Current.GetInstance<ICategoryFavouriteLevelService>();
            var favouriteCategory = favouriteLevelService.FindAll().FirstOrDefault();

            CategoryFavouriteLevel levelFromDb = favouriteLevelService.FindById(favouriteCategory.Id);

            Assert.AreEqual(favouriteCategory.Id, levelFromDb.Id, "Service must return expected favourite category by id");
        }
    }
}

