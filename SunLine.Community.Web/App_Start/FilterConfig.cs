using System.Diagnostics;
using System.Web.Mvc;
using SunLine.Community.Web.Common;

namespace SunLine.Community.Web
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            Trace.TraceInformation("Initialize FilterConfig");

            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
