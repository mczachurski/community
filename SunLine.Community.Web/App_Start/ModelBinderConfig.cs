using System.Diagnostics;
using System.Web.Mvc;
using SunLine.Community.Web.ModelBinder;

namespace SunLine.Community.Web
{
    public static class ModelBinderConfig
    {
        public static void Register()
        {
            Trace.TraceInformation("ModelBinderConfig registration");
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
        }
    }
}