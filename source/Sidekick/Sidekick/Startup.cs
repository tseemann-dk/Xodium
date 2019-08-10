using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xodium.Injection;
using Xodium.Injection.MicrosoftHosting;
using Xodium.Mvvm;

namespace Sidekick
{
    public static class Startup
    {
        private static IDependencyResolver dependencyResolver;
        private static readonly Lazy<IExecutionEnvironment> executionEnvironment =
            new Lazy<IExecutionEnvironment>(() => DependencyResolver.Resolve<IExecutionEnvironment>());

        public static IDependencyResolver DependencyResolver
        {
            get => dependencyResolver ?? throw new InvalidOperationException("Startup.Init() has not been called");
            private set => dependencyResolver = value;
        }

        public static IExecutionEnvironment ExecutionEnvironment => executionEnvironment.Value;

        public static void Init(Bootstrapper bootstrapper)
        {
            var builder = new HostBuilder()
                .ConfigureHostConfiguration(ConfigureHost)
                .ConfigureServices((context, services) => ConfigureServices(context, services, bootstrapper));

            var host = builder.Build();
            DependencyResolver = new ServiceProviderDependencyResolver(() => host.Services);
        }

        private static void ConfigureHost(IConfigurationBuilder builder)
        {
            //builder.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services, Bootstrapper bootstrapper)
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                // Development services
            }

            bootstrapper.ConfigureServices(new ServiceCollectionDependencyRegistry(services), () => DependencyResolver);
        }
    }
}
