using Microsoft.Owin;
using Owin;
using SunLine.Community.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace SunLine.Community.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
