using System.Net;
using System.Web.Mvc;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class ErrorsController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult UploadTooLarge()
        {
            Response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Unknown()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View();
        }
    }
}
