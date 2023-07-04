using System;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Windows.Services;
using Xodium.Platform.Windows.Wpf.Services;
using Xodium.Services;

namespace Xodium.Platform.Windows.Wpf
{
    public class WpfExecutionEnvironment : ExecutionEnvironmentBase
    {
        public WpfExecutionEnvironment(Func<IDependencyResolver> dependencyResolver) 
            : base(dependencyResolver)
        {
        }

        public override void RegisterServices(IDependencyRegistry registry)
        {
            var fileSystemService = new FileSystemService();

            // WPF Services
            registry.RegisterSingleton<INavigationService>(new NavigationService(() => GetService<IViewRegistry>()));
            registry.RegisterSingleton<IDialogService>(new DialogService(() => GetService<IViewRegistry>()));

            // Basic Windows Services
            registry.RegisterSingleton<IDeviceService>(new DeviceService());
            registry.RegisterSingleton<IFileLauncherService>(new FileLauncherService(fileSystemService));
            registry.RegisterSingleton<IFileSystemService>(fileSystemService);
            registry.RegisterSingleton<IPlatformService>(new PlatformService());
            registry.RegisterSingleton<ISynchronizerService>(new SynchronizerService());
        }
    }
}
