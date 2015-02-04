using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class UserCategoryService : IUserCategoryService
    {
        private readonly IUserCategoryRepository _userCategoryRepository;

        public UserCategoryService(IUserCategoryRepository userCategoryRepository)
        {
            _userCategoryRepository = userCategoryRepository;
        }

        public void Delete(UserCategory userCategory)
        {
            _userCategoryRepository.Delete(userCategory);
        }

        public UserCategory Create(UserCategory userCategory)
        {
            return _userCategoryRepository.Create(userCategory);
        }
    }
}
