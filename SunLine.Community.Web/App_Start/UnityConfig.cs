using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BitlyDotNET.Implementations;
using BitlyDotNET.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SunLine.Community.Entities.Exceptions;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services;
using SunLine.Community.Services.Search;
using SunLine.Community.Web.Common;
using Unity.Mvc5;


namespace SunLine.Community.Web
{
    public static class UnityConfig
    {
        public static IUnityContainer Register()
        {
            Trace.TraceInformation("UnityConfig registration");

            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new WebApiUnityDependencyResolver(container);
            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            SetDefaultLifetimeManager(container);
            RegisterTypes(container);
            return container;
        }

        private static void SetDefaultLifetimeManager(IUnityContainer container)
        {
            container.AddNewExtension<LifetimeContainerExtension>();
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            RegisterBitlyService(container);
            container.RegisterType<ILuceneService, LuceneAzureService>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IDbSession, DbSession>();
            container.RegisterType<IUserRepository, UserRepository>();

            container.RegisterTypes(
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(t => t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityRepository<>))),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            container.RegisterTypes(
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(t => Attribute.IsDefined(t, typeof(BusinessLogicAttribute))),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            container.RegisterTypes(
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(t => Attribute.IsDefined(t, typeof(ViewModelServiceAttribute))),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();

            // Logging registered types.
            foreach (var item in container.Registrations)
            {
                Trace.TraceInformation("Unity types: " + item.RegisteredType + " - " + item.MappedToType + " - " + item.Name);
            }
        }

        private static void RegisterBitlyService(IUnityContainer container)
        {
            string bitlyUserName = ConfigurationManager.AppSettings["BitlyUserName"];
            string bitlyPrivateKey = ConfigurationManager.AppSettings["BitlyPrivateKey"];

            if(bitlyUserName == string.Empty || bitlyPrivateKey == string.Empty)
            {
                throw new BitlyConfigurationNotFoundException();
            }

            container.RegisterType<IBitlyService>(new InjectionFactory(c => new BitlyService(bitlyUserName, bitlyPrivateKey)));
        }
    }
}