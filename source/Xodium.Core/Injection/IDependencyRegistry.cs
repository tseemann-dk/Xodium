using System;

namespace Xodium.Injection
{
    public enum Lifespan
    {
        Singleton,
        Transient
    }

    public interface IDependencyRegistry
    {
        void RegisterSingleton<TInterface, TInstance>()
            where TInterface : class
            where TInstance : class, TInterface;

        void RegisterSingleton<TInterface>(TInterface instance)
            where TInterface : class;

        void RegisterSingleton<TInterface, TInstance>(TInstance instance)
            where TInterface : class
            where TInstance : class, TInterface;

        void RegisterSingleton<TInterface>(Func<IDependencyResolver, TInterface> factory)
            where TInterface : class;

        void RegisterTransient<TInterface, TInstance>()
            where TInterface : class
            where TInstance : class, TInterface;

        void RegisterTransient<TInterface>(Func<IDependencyResolver, TInterface> factory)
            where TInterface : class;
    }
}
