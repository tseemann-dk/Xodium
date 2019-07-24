using System;
using Xodium.Flow;
using Xodium.Injection;
using Xodium.Services;

namespace Xodium.Mvvm
{
    public interface IExecutionEnvironment
    {
        T GetService<T>();
        object GetService(Type type);

        void RegisterServices(IDependencyRegistry registry);

        IActionDispatcher ActionDispatcher { get; }
        IClipboardService ClipboardService { get; }
        ICommunicationService CommunicationService { get; }
        IDeviceService DeviceService { get; }
        IDialogService DialogService { get; }
        IFilePickerService FilePickerService { get; }
        IFileSystemService FileSystemService { get; }
        ILocalizationService LocalizationService { get; }
        ILocationService LocationService { get; }
        IMediaPickerService MediaPickerService { get; }
        IMessengerService MessengerService { get; }
        INavigationService NavigationService { get; }
        IPlatformService PlatformService { get; }
        IPhotoService PhotoService { get; }
        ISettingsService SettingsService { get; }
        IShareService ShareService { get; }
        ISynchronizerService SynchronizerService { get; }
    }
}
