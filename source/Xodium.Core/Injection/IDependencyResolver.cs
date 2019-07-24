using System;

namespace Xodium.Injection
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
        object Resolve(Type type);
    }
}
