using System.Web.Mvc;

namespace SunLine.Community.Web.SessionContext
{
    public static class SystemSessionContextExtension
    {
        public static SystemSessionContext CurrentSystemSessionContext(this Controller controller)
        {
            if (controller.Request == null || controller.Request.Url == null)
            {
                return null;
            }

            return new SystemSessionContext
            {
                Host = controller.Request.Url.Host,
                Port = controller.Request.Url.Port,
                ApplicationPath = controller.Request.ApplicationPath
            };
        }
    }
}