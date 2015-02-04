using System.Collections.Generic;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Entities.Search
{
    public class UserSearchResult : SearchResult
    {
        public IList<User> Users { get; set; }
    }
}

