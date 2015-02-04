using System.Linq;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public class UserMessageStateRepository : EntityRepository<UserMessageState>, IUserMessageStateRepository
    {
        public UserMessageStateRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public UserMessageState FindByEnum(UserMessageStateEnum userMessageStateEnum)
        {
            return FindAll().FirstOrDefault(x => x.UserMessageStateEnum == userMessageStateEnum);
        }
    }
}