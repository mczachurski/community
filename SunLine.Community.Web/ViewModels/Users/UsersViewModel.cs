using System.Collections.Generic;

namespace SunLine.Community.Web.ViewModels.Users
{
    public class UsersViewModel
    {
        public UsersViewModel()
        {
            Users = new List<UserViewModel>();
        }

        public IList<UserViewModel> Users { get; set; }
    }
}

