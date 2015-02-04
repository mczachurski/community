using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public interface IMessageStateRepository : IEntityRepository<MessageState>
    {
        MessageState FindByEnum(MessageStateEnum messageStateEnum);
    }
}
