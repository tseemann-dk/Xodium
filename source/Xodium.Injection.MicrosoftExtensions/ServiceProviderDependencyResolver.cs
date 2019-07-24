using Microsoft.Extensions.DependencyInjection;
using System;

namespace Xodium.Injection.MicrosoftHosting
{
    public class ServiceProviderDependencyResolver : IDependencyResolver
    {
        private readonly Func<IServiceProvider> serviceProvider;

        public ServiceProviderDependencyResolver(Func<IServiceProvider> serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public T Resolve<T>() => serviceProvider().GetService<T>();
        public object Resolve(Type type) => serviceProvider().GetService(type);
    }
}
