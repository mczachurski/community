using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class FileRepository : EntityRepository<File>, IFileRepository
    {
        public FileRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }
    }
}