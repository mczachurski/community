using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace SunLine.Community.Web.Common
{
    public static class ApiVersionExtension
    {
        public static Version ApiVersion(this ApiController controller)
        {
            string versionToken = GetVersionToken(controller.Request);
            Version version;
            if (!Version.TryParse(versionToken, out version))
            {
                throw new VersionNotFoundException("Version have wrong format.");
            }

            return version;
        }

        private static string GetVersionToken(HttpRequestMessage request)
        {
            if (!request.Headers.Contains("ApiVersion"))
            {
                return null;
            }

            var accessToken = request.Headers.FirstOrDefault(x => x.Key == "ApiVersion");
            return accessToken.Value.FirstOrDefault();
        }
    }
}