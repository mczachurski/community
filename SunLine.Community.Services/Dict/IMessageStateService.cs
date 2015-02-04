using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Services.Dict
{
    public interface IMessageStateService
    {
        MessageState FindByEnum(MessageStateEnum messageStateEnum);
    }
}

