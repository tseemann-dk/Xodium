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
            registry.RegisterInstance<INavigationService>(new NavigationService(() => GetService<IViewRegistry>()));
            registry.RegisterInstance<IDialogService>(new DialogService());

            // Basic Windows Services
            registry.RegisterInstance<IDeviceService>(new DeviceService());
            registry.RegisterInstance<IFileLauncherService>(new FileLauncherService(fileSystemService));
            registry.RegisterInstance<IFileSystemService>(fileSystemService);
            registry.RegisterInstance<IPlatformService>(new PlatformService());
            registry.RegisterInstance<ISynchronizerService>(new SynchronizerService());
        }
    }
}
