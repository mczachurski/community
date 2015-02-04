using System.Web.Mvc;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace SunLine.Community.Web.Common
{
    /// <summary>
    /// Extension that change default LifetimeManager for all objects created by Unity.
    /// </summary>
    public class LifetimeContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var strategy = new LifetimeContainerBuilderStrategy(Container);
            Context.Strategies.Add(strategy, UnityBuildStage.Creation);
        }
            
        class LifetimeContainerBuilderStrategy : BuilderStrategy
        {
            private readonly IUnityContainer _container;

            public LifetimeContainerBuilderStrategy(IUnityContainer container)
            {
                _container = container;
            }

            public override void PreBuildUp(IBuilderContext context)
            {
                // We have to create one object per request (DbSession, Repositories, Services). But the controllers we have to create each time new.
                // If we don't then we have a exception: "A single instance of controller cannot be used to handle multiple requests. If a custom 
                // controller factory is in use, make sure that it creates a new instance of the controller for each request)."
                if (!(context.BuildKey.Type.IsSubclassOf(typeof(Controller))))
                {
                    context.PersistentPolicies.Set<ILifetimePolicy>(new PerRequestLifetimeManager(), context.BuildKey);
                }
            }
        }
    }
}