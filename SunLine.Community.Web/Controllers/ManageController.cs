using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SunLine.Community.Common;
using SunLine.Community.Resources.Titles;
using SunLine.Community.Web.ViewModels.Manage;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? (_signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>());
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ChangePasswordAsync(Guid.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                TempData[ActionConfirmation.TempDataKey] = ActionConfirmation.CreateSuccess(ManageMessage.PasswordHasBeenChanged);
                return View("Index", new IndexViewModel());
            }

            AddErrors(result);
            return View("Index", model);
        }
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAccountState(bool isAccountEnabled)
        {
            if (isAccountEnabled)
            {
                TempData[ActionConfirmation.TempDataKey] = ActionConfirmation.CreateSuccess(ManageMessage.AccountHasBeenEnabled);
            }
            else
            {
                TempData[ActionConfirmation.TempDataKey] = ActionConfirmation.CreateSuccess(ManageMessage.AccountHasBeenDisabled);
            }

            return View("Index", new IndexViewModel());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}