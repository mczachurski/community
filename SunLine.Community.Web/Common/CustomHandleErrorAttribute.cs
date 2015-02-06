using System;
using System.Diagnostics;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;

namespace SunLine.Community.Web.Common
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            try
            {
                if (filterContext != null && filterContext.Exception != null)
                {
                    IErrorService errorService = ServiceLocator.Current.GetInstance<IErrorService>();
                    IUnitOfWork unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
                    if (errorService != null && unitOfWork != null)
                    {
                        errorService.Create(filterContext.Exception.ToString());
                        unitOfWork.Commit();
                    }
                }
            }
            catch(Exception exception)
            {
                Trace.TraceError("Exception: " + exception);
            }

            // Redirect to page with error message.
            base.OnException(filterContext);
        }
    }
}