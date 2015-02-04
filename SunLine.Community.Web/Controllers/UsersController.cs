using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;
using SunLine.Community.Services.Dict;
using SunLine.Community.Web.SessionContext;
using SunLine.Community.Web.ViewModels.Messages;
using SunLine.Community.Web.ViewModels.Users;
using SunLine.Community.Web.ViewModelsServices;
using SunLine.Community.Common;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersViewModelService _usersViewModelService;
        private readonly IMessagesViewModelService _messagesViewModelService;
        private readonly IUserService _userService;
        private readonly IUserConnectionService _userConnectionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageService _languageService;
        private readonly ICategoryService _categoryService;
        private readonly IUserCategoryService _userCategoryService;
        private readonly ICategoryFavouriteLevelService _categoryFavouriteLevelService;
        private readonly IFileService _fileService;

        public UsersController(
            IUsersViewModelService usersViewModelService,
            IMessagesViewModelService messagesViewModelService,
            IUserConnectionService userConnectionService, 
            IUnitOfWork unitOfWork, 
            IUserService userService,
            ILanguageService languageService,
            ICategoryService categoryService,
            IUserCategoryService userCategoryService,
            ICategoryFavouriteLevelService categoryFavouriteLevelService,
            IFileService fileService)
        {
            _usersViewModelService = usersViewModelService;
            _messagesViewModelService = messagesViewModelService;
            _userConnectionService = userConnectionService;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _languageService = languageService;
            _categoryService = categoryService;
            _userCategoryService = userCategoryService;
            _categoryFavouriteLevelService = categoryFavouriteLevelService;
            _fileService = fileService;
        }

        [HttpGet]
        public ActionResult Popover(Guid id)
        {
            var popoverViewModel = _usersViewModelService.CreatePopoverViewModel(this.CurrentUserSessionContext().UserId, id);
            return View(popoverViewModel);
        }

        [HttpGet]
        public ActionResult Show(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
                
            string userName = id.Replace("@", "");
            var user = _userService.FindByUserName(userName);

            if (user == null)
            {
                return HttpNotFound();    
            }

            var watcherUserId = this.CurrentUserSessionContext().UserId;
            var profileViewModel = _usersViewModelService.CreateProfileViewModel(watcherUserId, user.Id);
            profileViewModel.TimelineViewModel = _messagesViewModelService.CreateProfileTimelineViewModel(user.Id, watcherUserId);

            return View(profileViewModel);
        }

        [HttpGet]
        public ActionResult TimelineAjax(Guid id)
        {
            var user = _userService.FindById(id);
            if (user == null)
            {
                return HttpNotFound();    
            }

            var watcherUserId = this.CurrentUserSessionContext().UserId;
            TimelineViewModel timelineViewModel = _messagesViewModelService.CreateProfileTimelineViewModel(user.Id, watcherUserId);

            return PartialView("_UserMessageTimelinePartial", timelineViewModel);
        }

        [HttpGet]
        public ActionResult Observers(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            string userName = id.Replace("@", "");
            var user = _userService.FindByUserName(userName);

            if (user == null)
            {
                return HttpNotFound();    
            }

            var watcherUserId = this.CurrentUserSessionContext().UserId;
            var profileViewModel = _usersViewModelService.CreateProfileViewModel(watcherUserId, user.Id);
            profileViewModel.Observers = _usersViewModelService.CreateUserObserversViewModel(watcherUserId, user.Id, 0, 20);

            return View(profileViewModel);
        }

        [HttpGet]
        public ActionResult ObserversAjax(Guid id)
        {
            var user = _userService.FindById(id);
            if (user == null)
            {
                return HttpNotFound();    
            }

            var watcherUserId = this.CurrentUserSessionContext().UserId;
            UsersViewModel observers = _usersViewModelService.CreateUserObserversViewModel(watcherUserId, user.Id, 0, 20);

            return  PartialView("_UserListPartial", observers);
        }

        [HttpGet]
        public ActionResult Observing(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            string userName = id.Replace("@", "");
            var user = _userService.FindByUserName(userName);

            if (user == null)
            {
                return HttpNotFound();    
            }

            var watcherUserId = this.CurrentUserSessionContext().UserId;
            var profileViewModel = _usersViewModelService.CreateProfileViewModel(watcherUserId, user.Id);
            profileViewModel.Observing = _usersViewModelService.CreateObservedByUserViewModel(watcherUserId, user.Id, 0, 20);

            return View(profileViewModel);
        }

        [HttpGet]
        public ActionResult ObservingAjax(Guid id)
        {
            var user = _userService.FindById(id);
            if (user == null)
            {
                return HttpNotFound();    
            }

            var watcherUserId = this.CurrentUserSessionContext().UserId;
            UsersViewModel observing = _usersViewModelService.CreateObservedByUserViewModel(watcherUserId, user.Id, 0, 20);

            return  PartialView("_UserListPartial", observing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleConnection(Guid toUserId)
        {
            bool usersHasConnection = _userConnectionService.ToggleConnection(this.CurrentUserSessionContext().UserId, toUserId);
            _unitOfWork.Commit();

            return Json(new { @success = true, @usersHasConnection = usersHasConnection });
        }
            
        [HttpGet]
        public ActionResult Edit()
        {
            var userId = this.CurrentUserSessionContext().UserId;
            EditProfileViewModel model = _usersViewModelService.CreateEditProfileViewModel(userId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProfileAjax(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = this.CurrentUserSessionContext().UserId;
                User user = _usersViewModelService.TransferDataFromViewModel(model, userId);

                _userService.Update(user);
                _unitOfWork.Commit();

                return Json(new { @success = true });
            }
                
            return Json(new { @success = false, @error = "Some data are incorrect." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCategoryAjax(IList<Guid> languages, IList<CategoryViewModel> levels)
        {
            var userId = this.CurrentUserSessionContext().UserId;
            User user = _userService.FindById(userId);

            RemoveUserLanguages(languages, user);
            AddUserLanguages(languages, user);

            RemoveUserCategories(user);
            AddUserCategories(levels, user);

            _userService.Update(user);
            _unitOfWork.Commit();

            return Json(new { @success = true }); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCover(string coverPatternName, HttpPostedFileBase image, bool deleteImageCover = false)
        {
            var userId = this.CurrentUserSessionContext().UserId;
            User user = _userService.FindById(userId);

            if(deleteImageCover)
            {
                if(user.CoverFile != null)
                {
                    user.CoverFile.Version++;
                    user.CoverFile = null;
                }
            }
            else if(image != null)
            {
                File file = _fileService.Create(userId, image.FileName, image.ContentType, image.ContentLength, image.InputStream);
                user.CoverFile = file;
            }
            else
            { 
                user.CoverPatternName = coverPatternName;
            }

            _userService.Update(user);
            _unitOfWork.Commit();

            TempData[ActionConfirmation.TempDataKey] = ActionConfirmation.CreateSuccess("Your cover has been saved.");
            EditProfileViewModel model = _usersViewModelService.CreateEditProfileViewModel(userId);
            return View("Edit", model);
        }

        private void RemoveUserLanguages(IList<Guid> languages, User user)
        {
            IList<Language> languagesToDelete = user.UserMessageLanguages.Where(x => !languages.Contains(x.Id)).ToList();
            foreach(var language in languagesToDelete)
            {
                user.UserMessageLanguages.Remove(language);
            }
        }

        private void AddUserLanguages(IList<Guid> languages, User user)
        {
            if (languages == null)
            {
                return;
            }
                
            foreach (Guid item in languages)
            {
                Language language = _languageService.FindById(item);
                user.UserMessageLanguages.Add(language);
            }
        }

        private void RemoveUserCategories(User user)
        {
            IList<UserCategory> userCategoriesToDelete = new List<UserCategory>(user.UserCategories);
            foreach (var item in userCategoriesToDelete)
            {
                _userCategoryService.Delete(item);
            }
        }

        private void AddUserCategories(IList<CategoryViewModel> levels, User user)
        {
            if (levels == null)
            {
                return;
            }

            foreach (CategoryViewModel item in levels)
            {
                CategoryFavouriteLevel level = _categoryFavouriteLevelService.FindById(item.LevelId);
                if (level == null)
                {
                    continue;
                }

                foreach (Guid categoryId in item.SelectedCatagories)
                {
                    Category category = _categoryService.FindById(categoryId);
                    if (category == null)
                    {
                        continue;
                    }

                    var userCategory = new UserCategory {
                        User = user,
                        Category = category,
                        CategoryFavouriteLevel = level
                    };

                    _userCategoryService.Create(userCategory);
                }
            }
        }
    }
}