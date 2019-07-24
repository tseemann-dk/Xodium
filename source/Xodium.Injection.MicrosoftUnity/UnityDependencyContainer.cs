using System;
using Unity;

namespace Xodium.Injection.Unity
{
    public class UnityDependencyContainer : IDependencyContainer
    {
        private readonly IUnityContainer container;

        public UnityDependencyContainer(IUnityContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void RegisterInstance<T>(T instance) 
            where T : class 
            => container.RegisterInstance(instance);

        public void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom
            => container.RegisterType<TFrom, TTo>();

        public void RegisterFactory<T>(Func<IDependencyResolver, T> factory)
            where T : class
            => container.RegisterFactory<T>(container => factory(this));

        public T Resolve<T>() 
            => container.Resolve<T>();

        public object Resolve(Type type) 
            => container.Resolve(type);
    }
}
