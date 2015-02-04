using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Web.SessionContext
{
    public static class UserSessionContextExtension
    {
        public static string UserSessionContextKey = "UserSessionContextKey";


        public static void CreateUserSessionContext(this Controller controller, string userName)
        {
            CreateContext(controller.Session, userName);
        }

        public static void RemoveUserSessionContext(this Controller controller)
        {
            controller.Session.Remove(UserSessionContextKey);
        }

        public static UserSessionContext CurrentUserSessionContext(this Controller controller)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserSessionContext context = controller.Session[UserSessionContextKey] as UserSessionContext ??
                                             CreateContext(controller.Session, HttpContext.Current.User.Identity.Name);

                return context;
            }

            return null;
        }

        public static UserSessionContext CurrentUserSessionContext(HttpSessionStateBase session)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserSessionContext context = session[UserSessionContextKey] as UserSessionContext ??
                                             CreateContext(session, HttpContext.Current.User.Identity.Name);

                return context;
            }

            return null;
        }

        private static UserSessionContext CreateContext(HttpSessionStateBase session, string userName)
        {
            var userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();

            var currentUser = userRepository.FindByUserName(userName);

            var context = new UserSessionContext(currentUser.Id, currentUser.UserName, currentUser.Email, currentUser.FirstName, currentUser.LastName, currentUser.GravatarHash, currentUser.Language.Id);
            
            session[UserSessionContextKey] = context;

            return context;
        }
    }
}