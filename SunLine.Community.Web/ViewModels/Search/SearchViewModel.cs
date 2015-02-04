using SunLine.Community.Web.ViewModels.Messages;
using SunLine.Community.Web.ViewModels.Users;

namespace SunLine.Community.Web.ViewModels.Search
{
    public class SearchViewModel
    {
        public string Query { get; set; }
        public int AmountOfUsers { get ; set; }
        public int AmountOfMessages { get; set; }
        public UsersViewModel Users { get; set; }
        public TimelineViewModel TimelineViewModel { get; set; }
    }
}
