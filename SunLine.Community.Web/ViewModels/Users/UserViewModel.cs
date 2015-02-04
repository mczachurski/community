using System;
using SunLine.Community.Web.ViewModels.Files;

namespace SunLine.Community.Web.ViewModels.Users
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Motto { get; set; }
        public string Location { get; set; }
        public string WebAddress { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsUserObservingWatcher { get; set; }
        public bool IsObservesByWatcher { get; set; }
        public string GravatarHash { get; set; }
        public string CoverPatternName { get; set;}
        public bool IsThisSameUser { get; set; }
        public FileViewModel File { get; set;}
    }
}

