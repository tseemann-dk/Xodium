using System;
using Unity;
using Unity.Lifetime;

namespace Xodium.Injection.UnityContainer
{
    public class UnityDependencyContainer : IDependencyContainer
    {
        private readonly IUnityContainer container;

        public UnityDependencyContainer(IUnityContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void RegisterSingleton<TInterface, TInstance>()
            where TInterface : class
            where TInstance : class, TInterface => 
            container.RegisterType<TInterface, TInstance>(TypeLifetime.Singleton);

        public void RegisterSingleton<TInterface>(Func<IDependencyResolver, TInterface> factory)
            where TInterface : class =>
            container.RegisterFactory<TInterface>(container => factory(this), FactoryLifetime.Singleton);

        public void RegisterSingleton<TInterface>(TInterface instance)
            where TInterface : class => 
            container.RegisterInstance(instance);

        public void RegisterSingleton<TInterface, TInstance>(TInstance instance)
            where TInterface : class
            where TInstance : class, TInterface => 
            container.RegisterInstance(typeof(TInterface), instance);

        public void RegisterTransient<TInterface, TInstance>()
            where TInterface : class
            where TInstance : class, TInterface =>
            container.RegisterType<TInterface, TInstance>(TypeLifetime.Transient);

        public void RegisterTransient<TInterface>(Func<IDependencyResolver, TInterface> factory)
            where TInterface : class =>
            container.RegisterFactory<TInterface>(container => factory(this), FactoryLifetime.Transient);

        public T Resolve<T>() => container.Resolve<T>();

        public object Resolve(Type type) => container.Resolve(type);
    }
}
