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

            var viewRegistry = CreateViewRegistry(type => resolver().Resolve(type));
            RegisterViews(viewRegistry);
            registry.RegisterInstance(viewRegistry);
            ViewRegistry = viewRegistry;

            var environment = CreateExecutionEnvironment(resolver);
            environment.RegisterServices(registry);
            registry.RegisterInstance(environment);

            RegisterServices(registry);
        }

        public IViewRegistry ViewRegistry { get; private set; }

        protected virtual IViewRegistry CreateViewRegistry(Func<Type, object> resolver) => new ViewRegistry(resolver);
        protected abstract IExecutionEnvironment CreateExecutionEnvironment(Func<IDependencyResolver> resolver);

        protected virtual void RegisterServices(IDependencyRegistry registry)
        {
        }

        protected virtual void RegisterViews(IViewRegistry registry)
        {
        }
    }
}
