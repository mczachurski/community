using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Core;
using SunLine.Community.Web.ViewModels.Users;

namespace SunLine.Community.Web.ViewModelsServices
{
    public interface IUsersViewModelService
    {
        PopoverViewModel CreatePopoverViewModel(Guid fromUserId, Guid toUserId);
        ProfileViewModel CreateProfileViewModel(Guid fromUserId, Guid toUserId);
        UsersViewModel CreateUserObserversViewModel(Guid watcherUserId, Guid userId, int page, int amountOnPage);
        UsersViewModel CreateObservedByUserViewModel(Guid watcherUserId, Guid userId, int page, int amountOnPage);
        UsersViewModel CreateUsersViewModel(IList<User> users, Guid watcherUserId);
        UserViewModel CreateUserViewModel(Guid watcherUserId, User user);
        EditProfileViewModel CreateEditProfileViewModel(Guid userId);
        User TransferDataFromViewModel(EditProfileViewModel viewModel, Guid userId);
    }
}

