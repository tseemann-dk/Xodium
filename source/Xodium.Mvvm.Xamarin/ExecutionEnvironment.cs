using System;
using Xodium.Injection;
using Xodium.Platform.Xamarin.Services;
using Xodium.Services;

namespace Xodium.Mvvm.Xamarin
{
    public class ExecutionEnvironment : ExecutionEnvironmentBase
    {
        public ExecutionEnvironment(Func<IDependencyResolver> dependencyResolver) 
            : base(dependencyResolver)
        {
        }

        public override void RegisterServices(IDependencyRegistry registry)
        {
            // TODO: 
            // Register Xamarin common services here and register platform-specific services 
            // in derived environment classes, i.e. AndroidExecutionEnvironment and iOSExecutionEnvironment 

            registry.RegisterInstance<ISynchronizerService>(new SynchronizerService());
        }
    }
}
