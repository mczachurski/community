using System.Linq;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public class MessageStateRepository : EntityRepository<MessageState>, IMessageStateRepository
    {
        public MessageStateRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public MessageState FindByEnum(MessageStateEnum messageStateEnum)
        {
            return FindAll().FirstOrDefault(x => x.MessageStateEnum == messageStateEnum);
        }
    }
}