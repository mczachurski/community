using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace SunLine.Community.Web.Common
{
    public class WebApiUnityDependencyScope : IDependencyScope
    {
        private bool _isDisposed;
        protected IUnityContainer Container { get; private set; }

        public WebApiUnityDependencyScope(IUnityContainer container)
        {
            Container = container;
        }

        public object GetService(Type serviceType)
        {
            if (typeof(IHttpController).IsAssignableFrom(serviceType))
            {
                return Container.Resolve(serviceType);
            }

            try
            {
                return Container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.ResolveAll(serviceType);
        }

        ~WebApiUnityDependencyScope()
        {
            Dispose(false);
        }
            
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
            
        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                Container.Dispose();
            }

            _isDisposed = true;
        }
    }
}