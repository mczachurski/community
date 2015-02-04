using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public class CategoryFavouriteLevelRepository : EntityRepository<CategoryFavouriteLevel>, ICategoryFavouriteLevelRepository
    {
        public CategoryFavouriteLevelRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }
    }
}
