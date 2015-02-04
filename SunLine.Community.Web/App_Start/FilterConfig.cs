using System.Web.Mvc;
using SunLine.Community.Web.Common;

namespace SunLine.Community.Web
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
