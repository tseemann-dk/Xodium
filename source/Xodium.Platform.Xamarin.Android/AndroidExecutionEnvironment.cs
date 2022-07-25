using Android.Content;
using System;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Xamarin.Android.Services;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Android
{
    public class AndroidExecutionEnvironment : ExecutionEnvironmentBase
    {
        private readonly Context context;

        public AndroidExecutionEnvironment(Func<IDependencyResolver> dependencyResolver, Context context) 
            : base(dependencyResolver)
        {
            this.context = context;
        }

        public override void RegisterServices(IDependencyRegistry registry)
        {
            registry.RegisterSingleton<IDeviceService>(new DeviceService(context));
            registry.RegisterSingleton<IPlatformService>(new PlatformService(context));
        }
    }
}
