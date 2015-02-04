using System;
using System.Web.Mvc;
using SunLine.Community.Web.SessionContext;
using SunLine.Community.Web.ViewModelsServices;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMessagesViewModelService _messagesViewModelService;

        public HomeController(IMessagesViewModelService messagesViewModelService)
        {
            _messagesViewModelService = messagesViewModelService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var timelineUserId = this.CurrentUserSessionContext().UserId;
                var watcherUserId = timelineUserId;
                var timelineViewModel = _messagesViewModelService.CreateUserTimelineViewModel(timelineUserId, watcherUserId);

                return View(timelineViewModel);
            }

            return RedirectToAction("Popular");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Popular()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Mentions()
        {
            var timelineUserId = this.CurrentUserSessionContext().UserId;
            var watcherUserId = timelineUserId;
            var timelineViewModel = _messagesViewModelService.CreateMentionTimelineViewModel(timelineUserId, watcherUserId);

            return View(timelineViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Terms()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Help()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SystemMessages()
        {
            return View();
        }
    }
}