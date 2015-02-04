using SunLine.Community.Entities.Core;
using SunLine.Community.Web.ViewModels.Files;
using SunLine.Community.Web.ViewModels.Messages;

namespace SunLine.Community.Web.ViewModels.Users
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public bool UserObservesYou { get; set; }
        public bool IsThisSameUser { get; set; }
        public bool YouObserveUser { get; set; }
        public int AmountOfAllObservedByUser { get; set; }
        public int AmountOfAllUserObservers { get; set; }
        public int AmountOfAllMessages { get; set; }
        public TimelineViewModel TimelineViewModel { get; set; }
        public UsersViewModel Observers { get; set; }
        public UsersViewModel Observing { get; set; }
        public string CoverPatternName { get; set; }
        public FileViewModel File { get; set;}
    }
}
