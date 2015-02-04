using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class MessageMentionRepository : EntityRepository<MessageMention>, IMessageMentionRepository
    {
        public MessageMentionRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }
    }
}