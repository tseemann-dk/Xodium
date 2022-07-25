using System;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Xamarin.Services;
using Xodium.Services;

namespace Xodium.Platform.Xamarin
{
    public class XamarinExecutionEnvironment : ExecutionEnvironmentBase
    {
        public XamarinExecutionEnvironment(Func<IDependencyResolver> dependencyResolver) 
            : base(dependencyResolver)
        {
        }

        public override void RegisterServices(IDependencyRegistry registry)
        {
            // TODO: 
            // Register Xamarin common services here and register platform-specific services 
            // in derived environment classes, i.e. AndroidExecutionEnvironment and iOSExecutionEnvironment 

            registry.RegisterSingleton<ISynchronizerService>(new SynchronizerService());
        }
    }
}
