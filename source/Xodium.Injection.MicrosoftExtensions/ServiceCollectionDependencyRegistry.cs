using Microsoft.Extensions.DependencyInjection;
using System;

namespace Xodium.Injection.MicrosoftExtensions
{
    public class ServiceCollectionDependencyRegistry : IDependencyRegistry
    {
        private readonly IServiceCollection collection;

        public ServiceCollectionDependencyRegistry(IServiceCollection collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public void RegisterSingleton<TInterface, TInstance>()
            where TInterface : class
            where TInstance : class, TInterface
        {
            collection.AddSingleton<TInterface, TInstance>();
        }

        public void RegisterSingleton<TInstance>(TInstance instance)
            where TInstance : class
        {
            collection.AddSingleton(instance);
        }

        public void RegisterSingleton<TInterface, TInstance>(TInstance instance)
            where TInterface : class
            where TInstance : class, TInterface
        {
            collection.AddSingleton<TInterface>(instance);
        }

        public void RegisterSingleton<TInterface>(Func<IDependencyResolver, TInterface> factory)
            where TInterface : class
        {
            collection.AddSingleton(service => factory(ToResolver(service)));
        }

        public void RegisterTransient<TInterface, TInstance>()
            where TInterface : class
            where TInstance : class, TInterface
        {
            collection.AddTransient<TInterface, TInstance>();
        }

        public void RegisterTransient<TInterface>(Func<IDependencyResolver, TInterface> factory)
            where TInterface : class
        {
            collection.AddTransient(service => factory(ToResolver(service)));
        }

        private static IDependencyResolver ToResolver(IServiceProvider provider) => 
            new ServiceProviderDependencyResolver(() => provider);
    }
}
