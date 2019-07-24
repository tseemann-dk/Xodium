using System;
using Xodium.Injection;
using Xodium.Flow;
using Xodium.Services;

namespace Xodium.Mvvm
{
    public abstract class ExecutionEnvironmentBase : IExecutionEnvironment
    {
        private readonly Func<IDependencyResolver> dependencyResolver;

        private readonly Lazy<IActionDispatcher> actionDispatcher;
        private readonly Lazy<IClipboardService> clipboardService;
        private readonly Lazy<ICommunicationService> communicationService;
        private readonly Lazy<IDeviceService> deviceService;
        private readonly Lazy<IDialogService> dialogService;
        private readonly Lazy<IFilePickerService> filePickerService;
        private readonly Lazy<IFileSystemService> fileSystemService;
        private readonly Lazy<ILocalizationService> localizationService;
        private readonly Lazy<ILocationService> locationService;
        private readonly Lazy<IMediaPickerService> mediaPickerService;
        private readonly Lazy<IMessengerService> messengerService;
        private readonly Lazy<INavigationService> navigationService;
        private readonly Lazy<IPhotoService> photoService;
        private readonly Lazy<IPlatformService> platformService;
        private readonly Lazy<ISettingsService> settingsService;
        private readonly Lazy<IShareService> shareService;
        private readonly Lazy<ISynchronizerService> synchronizerService;

        public ExecutionEnvironmentBase(Func<IDependencyResolver> dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));

            actionDispatcher = new Lazy<IActionDispatcher>(() => GetService<IActionDispatcher>());
            clipboardService = new Lazy<IClipboardService>(() => GetService<IClipboardService>());
            communicationService = new Lazy<ICommunicationService>(() => GetService<ICommunicationService>());
            deviceService = new Lazy<IDeviceService>(() => GetService<IDeviceService>());
            dialogService = new Lazy<IDialogService>(() => GetService<IDialogService>());
            filePickerService = new Lazy<IFilePickerService>(() => GetService<IFilePickerService>());
            fileSystemService = new Lazy<IFileSystemService>(() => GetService<IFileSystemService>());
            localizationService = new Lazy<ILocalizationService>(() => GetService<ILocalizationService>());
            locationService = new Lazy<ILocationService>(() => GetService<ILocationService>());
            mediaPickerService = new Lazy<IMediaPickerService>(() => GetService<IMediaPickerService>());
            messengerService = new Lazy<IMessengerService>(() => GetService<IMessengerService>());
            navigationService = new Lazy<INavigationService>(() => GetService<INavigationService>());
            photoService = new Lazy<IPhotoService>(() => GetService<IPhotoService>());
            platformService = new Lazy<IPlatformService>(() => GetService<IPlatformService>());
            settingsService = new Lazy<ISettingsService>(() => GetService<ISettingsService>());
            shareService = new Lazy<IShareService>(() => GetService<IShareService>());
            synchronizerService = new Lazy<ISynchronizerService>(() => GetService<ISynchronizerService>());
        }

        public virtual IActionDispatcher ActionDispatcher => actionDispatcher.Value;
        public virtual IClipboardService ClipboardService => clipboardService.Value;
        public virtual IDeviceService DeviceService => deviceService.Value;
        public virtual ICommunicationService CommunicationService => communicationService.Value;
        public virtual IDialogService DialogService => dialogService.Value;
        public virtual IFilePickerService FilePickerService => filePickerService.Value;
        public virtual IFileSystemService FileSystemService => fileSystemService.Value;
        public virtual ILocalizationService LocalizationService => localizationService.Value;
        public virtual ILocationService LocationService => locationService.Value;
        public virtual IMediaPickerService MediaPickerService => mediaPickerService.Value;
        public virtual IMessengerService MessengerService => messengerService.Value;
        public virtual INavigationService NavigationService => navigationService.Value;
        public virtual IPlatformService PlatformService => platformService.Value;
        public virtual IPhotoService PhotoService => photoService.Value;
        public virtual ISettingsService SettingsService => settingsService.Value;
        public virtual IShareService ShareService => shareService.Value;
        public virtual ISynchronizerService SynchronizerService => synchronizerService.Value;

        public T GetService<T>() => dependencyResolver().Resolve<T>();
        public object GetService(Type type) => dependencyResolver().Resolve(type);

        public abstract void RegisterServices(IDependencyRegistry registry);
    }
}
