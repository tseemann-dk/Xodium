using System;

namespace Xodium.Injection
{
    public interface IDependencyRegistry
    {
        void RegisterType<TFrom, TTo>() 
            where TFrom : class 
            where TTo : class, TFrom;

        void RegisterInstance<T>(T instance) 
            where T : class;

        void RegisterFactory<T>(Func<IDependencyResolver, T> factory)
            where T : class;
    }
}
