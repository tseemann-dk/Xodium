using System;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.iOS.Services;
using Xodium.Services;

namespace Xodium.Platform.iOS
{
    public class iOSExecutionEnvironment : ExecutionEnvironmentBase
    {
        public iOSExecutionEnvironment(Func<IDependencyResolver> dependencyResolver) 
            : base(dependencyResolver)
        {
        }

        public override void RegisterServices(IDependencyRegistry registry)
        {
            registry.RegisterInstance<IDeviceService>(new DeviceService());
            registry.RegisterInstance<IPlatformService>(new PlatformService());
        }
    }
}
