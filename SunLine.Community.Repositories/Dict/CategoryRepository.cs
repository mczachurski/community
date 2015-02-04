using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public class CategoryRepository : EntityRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }
    }
}