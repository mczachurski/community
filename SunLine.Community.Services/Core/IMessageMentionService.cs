using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface IMessageMentionService
    {
        void CreateMentions(Message message);
    }
}
