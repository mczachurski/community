using SunLine.Community.Entities.Core;

namespace SunLine.Community.Web.ViewModels.Users
{
    public class PopoverViewModel
    {
        public User User { get; set; }
        public bool UserObservesYou { get; set; }
        public bool IsThisSameUser { get; set; }
        public bool YouObserveUser { get; set; }
        public int AmountOfAllObservedByUser { get; set; }
        public int AmountOfAllUserObservers { get; set; }
    }
}
