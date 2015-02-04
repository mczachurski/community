using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public interface IUserMessageStateRepository : IEntityRepository<UserMessageState>
    {
        UserMessageState FindByEnum(UserMessageStateEnum userMessageStateEnum);
    }
}