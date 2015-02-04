using System.Web.Mvc;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class DirectMessagesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}