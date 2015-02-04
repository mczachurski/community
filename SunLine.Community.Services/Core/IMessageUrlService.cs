using System.Collections.Generic;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface IMessageUrlService
    {
        string ParseMindUrl(string message, out IList<MessageUrl> messageUrls);
        void CreateMessageUrls(Message message, IList<MessageUrl> messageUrls);
    }
}
