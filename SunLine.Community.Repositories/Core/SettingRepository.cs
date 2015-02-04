using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class SettingRepository : EntityRepository<Setting>, ISettingRepository
    {
        public SettingRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }
    }
}
