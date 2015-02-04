using System;
using System.Diagnostics;
using System.Linq;
using BitlyDotNET.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Rhino.Mocks;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services;

namespace SunLine.Community.NUnitTests
{
    public static class UnityConfig
    {
        public static void Register()
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return;
            }
                
            var container = BuildUnityContainer();
            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        }

        private static void RegisterTypes(IUnityContainer container)
        {         
            IBitlyService bitlyService = MockRepository.GenerateStub<IBitlyService>();
            container.RegisterInstance<IBitlyService>(bitlyService);

            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterInstance<IDbSession>(new DbSession());

            container.RegisterTypes(
                AllClasses.FromAssembliesInBasePath()
                .Where(t => t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityRepository<>))),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            container.RegisterTypes(
                AllClasses.FromAssembliesInBasePath()
                .Where(t => Attribute.IsDefined(t, typeof(BusinessLogicAttribute))),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            // Logging registered types.
            foreach (var item in container.Registrations)
            {
                Debug.WriteLine(item.RegisteredType + " - " + item.MappedToType + " - " + item.Name);
            }
        }
    }
}