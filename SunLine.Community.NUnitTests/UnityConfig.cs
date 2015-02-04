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
using SunLine.Community.Services.Search;

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
            RegisterBitlyService(container);
            container.RegisterType<ILuceneService, LuceneRamService>();
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

        private static void RegisterBitlyService(IUnityContainer container)
        {
            var bitlyService = MockRepository.GenerateStub<IBitlyService>();
            bitlyService.Stub(x => x.Shorten("http://en.wikipedia.org/wiki/List_of_films_considered_the_best")).Return("http://bit.ly/16lmX8q");
            bitlyService.Stub(x => x.Shorten("http://rateyourmusic.com/list/morre/top_500_best_songs_ever/")).Return("http://bit.ly/16llJdo");
            container.RegisterInstance(bitlyService);
        }
    }
}