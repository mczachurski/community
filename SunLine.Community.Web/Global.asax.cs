using System;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SunLine.Community.Web.Common;

namespace SunLine.Community.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            CultureConfig.Register();
            DatabaseConfig.Register();
            AreaRegistration.RegisterAllAreas();
            UnityConfig.Register();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinderConfig.Register();
        }

        private void Application_Error(object sender, EventArgs e)
        {
            if (Context.IsCustomErrorEnabled)
            {
                Exception exception = Server.GetLastError();
                Server.ClearError();

                Trace.TraceError("Unhandled Exception: " + exception);
                TryHandleException(exception);
            }
        }

        private void TryHandleException(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            if (MaxRequestExceededHelper.IsMaxRequestExceededException(exception))
            {
                Response.Redirect(VirtualPathUtility.ToAbsolute("~/Errors/UploadTooLarge"));
            }
            else
            {
                TryHandleHttpError(exception);
            }
        }

        private void TryHandleHttpError(Exception exception)
        {
            var httpException = exception as HttpException;
            if (httpException == null)
            {
                return;
            }

            Response.Clear();
            switch (httpException.GetHttpCode())
            {
                case (int)HttpStatusCode.Forbidden:
                    Response.Redirect(VirtualPathUtility.ToAbsolute("~/Errors/AccessDenied"));
                    break;
                case (int)HttpStatusCode.NotFound:
                    Response.Redirect(VirtualPathUtility.ToAbsolute("~/Errors/NotFound"));
                    break;
                default:
                    Response.Redirect(VirtualPathUtility.ToAbsolute("~/Errors/Unknown"));
                    break;
            }
        }
    }
}
