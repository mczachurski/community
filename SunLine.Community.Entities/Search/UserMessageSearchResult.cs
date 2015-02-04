using System.Collections.Generic;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Entities.Search
{
    public class UserMessageSearchResult : SearchResult
    {
        public IList<UserMessage> UserMessages { get; set; }
    }
}

