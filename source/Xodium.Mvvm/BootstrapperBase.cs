using System;
using Xodium.Injection;

namespace Xodium.Mvvm
{
    public abstract class BootstrapperBase
    {
        public virtual void ConfigureServices(IDependencyRegistry registry, Func<IDependencyResolver> resolver)
        {
            if (registry is null)
                throw new ArgumentNullException(nameof(registry));

            if (resolver is null)
                throw new ArgumentNullException(nameof(resolver));

            var viewRegistry = GetViewRegistry(type => resolver().Resolve(type));
            RegisterViews(viewRegistry);
            registry.RegisterInstance(viewRegistry);

            var environment = GetExecutionEnvironment(resolver);
            environment.RegisterServices(registry);
            registry.RegisterInstance(environment);

            RegisterServices(registry);
        }

        public void Shutdown()
        {
            OnShutdown();
        }

        protected virtual void OnShutdown()
        {
        }

        protected virtual IViewRegistry GetViewRegistry(Func<Type, object> resolver) => new ViewRegistry(resolver);
        protected abstract IExecutionEnvironment GetExecutionEnvironment(Func<IDependencyResolver> resolver);

        protected virtual void RegisterServices(IDependencyRegistry registry)
        {
        }

        protected virtual void RegisterViews(IViewRegistry registry)
        {
        }
    }
}
