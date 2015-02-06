using System;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.ServiceLocation;
using Owin;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Web
{
    public partial class Startup
    {
        // add this static variable
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            Trace.TraceInformation("Configuration authorization provider");

            // add this assignment
            DataProtectionProvider = app.GetDataProtectionProvider();

            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext<DatabaseContext>((options, owinContext) => ServiceLocator.Current.GetInstance<IDbSession>().Current as DatabaseContext);
            app.CreatePerOwinContext<ApplicationUserManager>((options, owinContext) => ServiceLocator.Current.GetInstance<ApplicationUserManager>());
            app.CreatePerOwinContext<ApplicationSignInManager>((options, owinContext) => ServiceLocator.Current.GetInstance<ApplicationSignInManager>());

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User, Guid>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: (id) => Guid.Parse(id.GetUserId()))
                }
            });
        }
    }
}