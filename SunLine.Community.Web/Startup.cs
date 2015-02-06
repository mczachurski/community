using Microsoft.Owin;
using Owin;
using SunLine.Community.Web;
using System.Diagnostics;

[assembly: OwinStartup(typeof(Startup))]
namespace SunLine.Community.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Trace.TraceInformation("Startup configuration");
            ConfigureAuth(app);
        }
    }
}
