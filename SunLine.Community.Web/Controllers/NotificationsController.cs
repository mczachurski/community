using System.Web.Mvc;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}