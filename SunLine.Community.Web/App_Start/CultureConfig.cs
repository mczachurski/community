using System.Globalization;

namespace SunLine.Community.Web
{
    public static class CultureConfig
    {
        public static void Register()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}