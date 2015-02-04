using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Search;

namespace SunLine.Community.Services.Search
{
    public interface ISearchService
    {
        UserSearchResult SearchUsers(string s, int page, int amountOnPage);
        UserMessageSearchResult SearchMessages(string s, int page, int amountOnPage);
        void AddMessageToIndex(Message message);
        void AddUserToIndex(User user);
    }
}
