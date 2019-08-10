using System;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Uwp.Services;
using Xodium.Services;

namespace Xodium.Platform.Uwp
{
    public class UwpExecutionEnvironment : ExecutionEnvironmentBase
    {
        public UwpExecutionEnvironment(Func<IDependencyResolver> dependencyResolver) 
            : base(dependencyResolver)
        {
        }

        public override void RegisterServices(IDependencyRegistry registry)
        {
            registry.RegisterInstance<IDeviceService>(new DeviceService());
            registry.RegisterInstance<IPlatformService>(new PlatformService());
            registry.RegisterInstance<ISynchronizerService>(new SynchronizerService());
        }
    }
}
