using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface IUserCategoryService
    {
        void Delete(UserCategory userCategory);
        UserCategory Create(UserCategory userCategory);
    }
}

