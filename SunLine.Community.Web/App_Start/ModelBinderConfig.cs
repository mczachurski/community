using SunLine.Community.Web.ModelBinder;
using System.Web.Mvc;

namespace SunLine.Community.Web
{
    public static class ModelBinderConfig
    {
        public static void Register()
        {
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
        }
    }
}