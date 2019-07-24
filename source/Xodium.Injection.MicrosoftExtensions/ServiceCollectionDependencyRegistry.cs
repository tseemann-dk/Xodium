using Microsoft.Extensions.DependencyInjection;
using System;

namespace Xodium.Injection.MicrosoftHosting
{
    public class ServiceCollectionDependencyRegistry : IDependencyRegistry
    {
        private readonly IServiceCollection collection;

        public ServiceCollectionDependencyRegistry(IServiceCollection collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public void RegisterFactory<T>(Func<IDependencyResolver, T> factory) where T : class
        {
            collection.AddTransient(provider => factory(new ServiceProviderDependencyResolver(() => provider)));
        }

        public void RegisterInstance<T>(T instance) where T : class
        {
            collection.AddSingleton(instance);
        }

        void IDependencyRegistry.RegisterType<TFrom, TTo>()
        {
            collection.AddTransient<TFrom, TTo>();
        }
    }
}
