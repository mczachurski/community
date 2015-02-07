using System.Diagnostics;
using System.Globalization;

namespace SunLine.Community.Web
{
    public static class CultureConfig
    {
        public static void Register()
        {
            Trace.TraceInformation("Initialize CultureConfig");

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}