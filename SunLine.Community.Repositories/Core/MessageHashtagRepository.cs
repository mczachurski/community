using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class MessageHashtagRepository : EntityRepository<MessageHashtag>, IMessageHashtagRepository
    {
        public MessageHashtagRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }
    }
}