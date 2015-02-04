using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public interface IUserMessageCreationModeRepository : IEntityRepository<UserMessageCreationMode>
    {
        UserMessageCreationMode FindByEnum(UserMessageCreationModeEnum userMessageCreationModeEnum);
    }
}