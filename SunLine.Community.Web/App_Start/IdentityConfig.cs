using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.ServiceLocation;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;

namespace SunLine.Community.Web
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            IEmailService emailService = ServiceLocator.Current.GetInstance<IEmailService>();
            return new Task(() => emailService.SendEmail(message.Destination, string.Empty, message.Subject, message.Body));
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationUserStore : UserStore<User, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUserStore(DatabaseContext context)
            : base(context)
        {
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User, Guid>
    {
        public ApplicationUserManager(IDbSession dbSession)
            : base(new ApplicationUserStore(dbSession.Current as DatabaseContext))
        {
            // Configure validation logic for usernames
            UserValidator = new UserValidator<User, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            EmailService = new EmailService();
            SmsService = new SmsService();

            var dataProtectionProvider = Startup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<User, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public sealed class ApplicationSignInManager : SignInManager<User, Guid>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
