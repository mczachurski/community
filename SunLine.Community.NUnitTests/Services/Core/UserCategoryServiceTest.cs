using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Dict;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class UserCategoryServiceTest : BaseTest
    {
        IUserCategoryService _userCategoryService;
        IUserCategoryRepository _userCategoryRepository;
        ICategoryRepository _categoryRepository;
        ICategoryFavouriteLevelRepository _categoryFavouriteLevelRepository;
        private IUnitOfWork _unitOfWork;

        public override void SetUp()
        {
            base.SetUp();

            _userCategoryService = ServiceLocator.Current.GetInstance<IUserCategoryService>();
            _userCategoryRepository = ServiceLocator.Current.GetInstance<IUserCategoryRepository>();
            _categoryRepository = ServiceLocator.Current.GetInstance<ICategoryRepository>();
            _categoryFavouriteLevelRepository = ServiceLocator.Current.GetInstance<ICategoryFavouriteLevelRepository>();
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
        }

        [Test]
        public void user_can_add_new_categories_to_observe()
        {
            var category = _categoryRepository.FindAll().FirstOrDefault();
            var favouriteLevel = _categoryFavouriteLevelRepository.FindAll().FirstOrDefault();
            var userCategory = new UserCategory { User = DatabaseHelper.UserTest1, Category = category, CategoryFavouriteLevel = favouriteLevel };
            _unitOfWork.Commit();

            _userCategoryService.Create(userCategory);
            _unitOfWork.Commit();

            UserCategory userCategoryFromDb = _userCategoryRepository.FindAll().FirstOrDefault(x => x.User.Id == DatabaseHelper.UserTest1.Id 
                && x.Category.Id == category.Id && x.CategoryFavouriteLevel.Id == favouriteLevel.Id);
            Assert.IsNotNull(userCategoryFromDb, "Category for user wasn't created");
        }

        [Test]
        public void user_can_delete_categories_to_observe()
        {
            var category = _categoryRepository.FindAll().FirstOrDefault();
            var favouriteLevel = _categoryFavouriteLevelRepository.FindAll().FirstOrDefault();
            var userCategory = new UserCategory { User = DatabaseHelper.UserTest2A, Category = category, CategoryFavouriteLevel = favouriteLevel };
            _userCategoryRepository.Create(userCategory);
            _unitOfWork.Commit();

            _userCategoryService.Delete(userCategory);
            _unitOfWork.Commit();

            UserCategory userCategoryFromDb = _userCategoryRepository.FindAll().FirstOrDefault(x => x.User.Id == DatabaseHelper.UserTest2A.Id 
                && x.Category.Id == category.Id && x.CategoryFavouriteLevel.Id == favouriteLevel.Id);
            Assert.IsNull(userCategoryFromDb, "Category for user wasn't deleted");
        }
    }
}

