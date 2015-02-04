using System;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Common;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Services.Core;
using SunLine.Community.Services.Dict;
using SunLine.Community.Web.Common;
using SunLine.Community.Web.ViewModels;
using SunLine.Community.Web.ViewModels.Users;
using SunLine.Community.Web.ViewModels.Files;

namespace SunLine.Community.Web.ViewModelsServices
{
    [ViewModelService]
    public class UsersViewModelService : IUsersViewModelService
    {
        private readonly IUserService _userService;
        private readonly IUserConnectionService _userConnectionService;
        private readonly IMessageService _messageService;
        private readonly ILanguageService _languageService;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryFavouriteLevelService _categoryFavouriteLevelService;
        private readonly ISettingService _settingsService;
        private readonly IFileService _fileService;

        public UsersViewModelService(
            IUserService userService, 
            IUserConnectionService userConnectionService, 
            IMessageService messageService,
            ILanguageService languageService,
            ICategoryService categoryService,
            ICategoryFavouriteLevelService categoryFavouriteLevelService,
            ISettingService settingsService,
            IFileService fileService)
        {
            _userService = userService;
            _userConnectionService = userConnectionService;
            _messageService = messageService;
            _languageService = languageService;
            _categoryService = categoryService;
            _categoryFavouriteLevelService = categoryFavouriteLevelService;
            _settingsService = settingsService;
            _fileService = fileService;
        }

        public PopoverViewModel CreatePopoverViewModel(Guid fromUserId, Guid toUserId)
        {
            return new PopoverViewModel
            {
                User = _userService.FindById(toUserId),
                YouObserveUser = fromUserId != toUserId && _userConnectionService.IsConnectionBetweenUsers(fromUserId, toUserId),
                UserObservesYou = fromUserId != toUserId && _userConnectionService.IsConnectionBetweenUsers(toUserId, fromUserId),
                IsThisSameUser = fromUserId == toUserId,
                AmountOfAllObservedByUser = _userConnectionService.AmountOfAllObservedByUser(toUserId),
                AmountOfAllUserObservers = _userConnectionService.AmountOfAllUserObservers(toUserId)
            };
        }

        public ProfileViewModel CreateProfileViewModel(Guid fromUserId, Guid toUserId)
        {
            User toUser = _userService.FindById(toUserId);

            FileViewModel fileViewModel = null;
            if(toUser.CoverFile != null)
            {
                fileViewModel = new FileViewModel
                {
                    FileUrl = _fileService.GetUrlToFile(toUser.CoverFile),
                    ThumbnailUrl = _fileService.GetUrlToFileThumbnail(toUser.CoverFile)
                };
            }

            return new ProfileViewModel
            {
                User = toUser,
                YouObserveUser = fromUserId != toUserId && _userConnectionService.IsConnectionBetweenUsers(fromUserId, toUserId),
                UserObservesYou = fromUserId != toUserId && _userConnectionService.IsConnectionBetweenUsers(toUserId, fromUserId),
                IsThisSameUser = fromUserId == toUserId,
                AmountOfAllObservedByUser = _userConnectionService.AmountOfAllObservedByUser(toUserId),
                AmountOfAllUserObservers = _userConnectionService.AmountOfAllUserObservers(toUserId),
                AmountOfAllMessages = _messageService.AmountOfMessages(toUserId, true),
                CoverPatternName = toUser.CoverPatternName,
                File = fileViewModel
            };
        }

        public UsersViewModel CreateUserObserversViewModel(Guid watcherUserId, Guid userId, int page, int amountOnPage)
        {
            IList<User> observers = _userConnectionService.FindUserObservers(userId, page, amountOnPage);
            var usersViewModel = CreateUsersViewModel(observers, watcherUserId);
            return usersViewModel;
        }

        public UsersViewModel CreateObservedByUserViewModel(Guid watcherUserId, Guid userId, int page, int amountOnPage)
        {
            IList<User> observers = _userConnectionService.FindObservedByUser(userId, page, amountOnPage);
            var usersViewModel = CreateUsersViewModel(observers, watcherUserId);
            return usersViewModel;
        }

        public UsersViewModel CreateUsersViewModel(IList<User> users, Guid watcherUserId)
        {
            var usersViewModel = new UsersViewModel();

            foreach (var user in users)
            {
                var userViewModel = CreateUserViewModel(watcherUserId, user);
                usersViewModel.Users.Add(userViewModel);
            }

            return usersViewModel;
        }

        public UserViewModel CreateUserViewModel(Guid watcherUserId, User user)
        {
            FileViewModel fileViewModel = null;
            if(user.CoverFile != null)
            {
                fileViewModel = new FileViewModel
                {
                    FileUrl = _fileService.GetUrlToFileThumbnail(user.CoverFile),
                    ThumbnailUrl = _fileService.GetUrlToFileThumbnail(user.CoverFile) 
                };
            }

            var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Motto = user.Motto,
                    Location = user.Location,
                    WebAddress = user.WebAddress,
                    CreationDate = user.CreationDate,
                    IsUserObservingWatcher = watcherUserId != user.Id && _userConnectionService.IsConnectionBetweenUsers(user.Id, watcherUserId),
                    IsObservesByWatcher = watcherUserId != user.Id && _userConnectionService.IsConnectionBetweenUsers(watcherUserId, user.Id),
                    GravatarHash = user.GravatarHash,
                    CoverPatternName = user.CoverPatternName,
                    IsThisSameUser = watcherUserId == user.Id,
                    File = fileViewModel
                };

            return userViewModel;
        }

        public EditProfileViewModel CreateEditProfileViewModel(Guid userId)
        {
            User user = _userService.FindById(userId);
            if (user == null)
            {
                throw new ArgumentException("User not exist!");
            }

            var editProfileViewModel = new EditProfileViewModel { 
                FirstName = user.FirstName,
                LastName = user.LastName,
                Motto = user.Motto,
                Location = user.Location,
                WebAddress = user.WebAddress,
                Language = user.Language.Id,
                CoverPatternName = user.CoverPatternName
            };

            if(user.CoverFile != null)
            {
                editProfileViewModel.CoverImageUrl = _fileService.GetUrlToFileThumbnail(user.CoverFile);
            }

            IList<CategoryFavouriteLevel> levels = _categoryFavouriteLevelService.FindAll().OrderBy(x => x.FavouriteLevel).ToList();
            foreach (var level in levels)
            {
                var categoryViewModel = new CategoryViewModel {
                    LevelId = level.Id,
                    LevelName = level.Name,
                    FavouriteLevel = level.FavouriteLevel
                };

                IList<Guid> userCategory = user.UserCategories.Where(x => x.CategoryFavouriteLevel.Id == level.Id).Select(x => x.Category.Id).ToList();
                categoryViewModel.SelectedCatagories.AddRange(userCategory);

                editProfileViewModel.CategoryViewModels.Add(categoryViewModel);
            }

            foreach (var language in user.UserMessageLanguages)
            {
                var languageViewModel = new DictViewModel<Guid> {
                    Id = language.Id,
                    Name = language.Name
                };

                editProfileViewModel.UserMessageLanguages.Add(languageViewModel);
            }
                
            for(int i = 1; i <= _settingsService.MaxCoverNameCounter; i++)
            {
                string coverId = string.Format("p{0:00000}.png", i);
                string coverName = string.Format("Template {0:00000}", i);
                editProfileViewModel.CoverPatternNames.Add(new DictViewModel<string>(coverId, coverName));
            }

            editProfileViewModel.Languages.AddRange(_languageService.FindAll());
            editProfileViewModel.Categories.AddRange(_categoryService.FindAll());
            return editProfileViewModel;
        }

        public User TransferDataFromViewModel(EditProfileViewModel viewModel, Guid userId)
        {
            User user = _userService.FindById(userId);
            if (user == null)
            {
                throw new ArgumentException("User not exist!");
            }

            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Motto = viewModel.Motto;
            user.Location = viewModel.Location;
            user.WebAddress = viewModel.WebAddress;
            user.Language = _languageService.FindById(viewModel.Language);

            return user;
        }
    }
}
