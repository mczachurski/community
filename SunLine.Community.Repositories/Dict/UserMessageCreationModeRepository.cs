using System.Linq;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public class UserMessageCreationModeRepository : EntityRepository<UserMessageCreationMode>, IUserMessageCreationModeRepository
    {
        public UserMessageCreationModeRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public UserMessageCreationMode FindByEnum(UserMessageCreationModeEnum userMessageCreationModeEnum)
        {
            return FindAll().FirstOrDefault(x => x.UserMessageCreationModeEnum == userMessageCreationModeEnum);
        }
    }
}