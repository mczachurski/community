using System.Net;
using System.Web.Mvc;

namespace SunLine.Community.Web.Common
{
    public class HttpAccessDeniedResult : HttpStatusCodeResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);

            ViewResult result = new ViewResult
            {
                ViewName = @"AccessDenied",
                ViewData = context.Controller.ViewData,
                TempData = context.Controller.TempData
            };

            result.ExecuteResult(context);
        }
            
        public HttpAccessDeniedResult() : base(HttpStatusCode.Forbidden, "Forbidden")
        {
        }
    }
}