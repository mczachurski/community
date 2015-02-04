using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class UserCategoryRepository : EntityRepository<UserCategory>, IUserCategoryRepository
    {
        public UserCategoryRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }
    }
}
