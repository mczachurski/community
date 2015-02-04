using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class MessageUrlRepository : EntityRepository<MessageUrl>, IMessageUrlRepository
    {
        public MessageUrlRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }
    }
}