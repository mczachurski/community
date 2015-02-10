using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using SunLine.Community.Common;
using SunLine.Community.Entities.Core;
using SunLine.Community.Resources.Titles;
using SunLine.Community.Services.Core;
using SunLine.Community.Services.Dict;
using SunLine.Community.Web.SessionContext;
using SunLine.Community.Web.ViewModels.Account;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IAuthenticationManager _authenticationManager;
        private readonly IEmailService _emailService;
        private readonly ILanguageService _languageService;
        private readonly ISettingService _settingService;

        public AccountController(
            ApplicationUserManager userManager, 
            ApplicationSignInManager signInManager, 
            IEmailService emailService, 
            ILanguageService languageService, 
            ISettingService settingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _languageService = languageService;
            _settingService = settingService;
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authenticationManager ?? (_authenticationManager = HttpContext.GetOwinContext().Authentication);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, AccountMessage.InvalidLoginAttempt);
                return View(model);
            }

            if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            {
                ModelState.AddModelError(string.Empty, AccountMessage.EmailNotConfirmed);
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                {
                    this.CreateUserSessionContext(model.UserName);
                    return RedirectToLocal(returnUrl);
                }
                case SignInStatus.LockedOut:
                {
                    return View("Lockout");
                }
                default:
                {
                    ModelState.AddModelError(string.Empty, AccountMessage.InvalidLoginAttempt);
                    return View(model);
                }
            }
        }
            
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            model.Languages = _languageService.FindAll().ToList();
            model.RecaptchaPublicKey = _settingService.RecaptchaPublicKey;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!IsRecaptchaValid())
            {
                ModelState.AddModelError(string.Empty, AccountMessage.CaptchaValidation);
            }

            if (ModelState.IsValid)
            {
                var language = _languageService.FindByCode(model.Language);

                var user = new User { 
                    UserName = model.UserName, 
                    Email = model.Email, 
                    LastName = model.LastName, 
                    FirstName = model.FirstName, 
                    Language = language,
                    GravatarHash = model.Email.Trim().ToLower().CalculateMd5Hash(),
                    CreationDate = DateTime.UtcNow, 
                    Version = 1,
                    CoverPatternName = GetCoverPatternName()
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    if (Request.Url != null)
                    {
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                        string emailTitle = AccountMessage.ConfirmYourEmailTitle;
                        string emailBody = string.Format(AccountMessage.ConfirmYourEmailBody, callbackUrl);
                        _emailService.SendEmail(user.Email, user.FirstName, emailTitle, emailBody);
                    }

                    TempData[ActionConfirmation.TempDataKey] = ActionConfirmation.CreateSuccess(AccountMessage.AccountHasBeenCreated);
                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            model.Languages = _languageService.FindAll().ToList();
            model.RecaptchaPublicKey = _settingService.RecaptchaPublicKey;

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(Guid userId, string code)
        {
            if (userId == Guid.Empty || code == null)
            {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed.
                    return View("ForgotPasswordConfirmation");
                }
                    
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                if (Request.Url != null)
                {
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    string emailTitle = AccountMessage.ResetPasswordEmailTitle;
                    string emailBody = string.Format(AccountMessage.ResetPasswordEmailBody, callbackUrl);
                    _emailService.SendEmail(user.Email, user.FirstName, emailTitle, emailBody);
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
            }

            // If we got this far, something failed, redisplay form.
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist.
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            this.RemoveUserSessionContext();
            return RedirectToAction("Index", "Home");
        }
            
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        private string GetIpAddress()
        {
            string strHostName = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            return ipAddress.ToString();
        }

        private string GetCoverPatternName()
        {
            int maxCoverNameCounter = _settingService.MaxCoverNameCounter;
            if (maxCoverNameCounter == 0)
            {
                return "p00033.png";
            }

            var random = new Random();
            int randomCounter = (random.Next() % maxCoverNameCounter) + 1;

            string coverName = string.Format("p{0:00000}.png", randomCounter);
            return coverName;
        }

        private bool IsRecaptchaValid()
        {
            var recaptcha = Request.Params["g-recaptcha-response"];
            var ipAddress = GetIpAddress();
            string urlRecaptcha = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}",
                    _settingService.RecaptchaPrivateKey, recaptcha, ipAddress);

            var client = new WebClient();
            dynamic recaptchaResult = JsonConvert.DeserializeObject(client.DownloadString(urlRecaptcha));

            return recaptchaResult.success;
        }
    }
}