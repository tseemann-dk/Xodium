using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI.Xaml;

namespace Xodium.Platform.Uwp.Triggers
{
    public enum DeviceFormFactor { Undefined, Phone, Tablet, Desktop }

    public class DeviceFormFactorTrigger : StateTriggerBase
    {
        private static readonly DeviceFormFactor CurrentFormFactor;

        static DeviceFormFactorTrigger()
        {
            CurrentFormFactor = GetFormFactor();
        }

        public static DeviceFormFactor GetFormFactor()
        {
            var hasHardwareButtons = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");

            switch (AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                case "Windows.Mobile":
                    return hasHardwareButtons ? DeviceFormFactor.Phone : DeviceFormFactor.Tablet;
                case "Windows.Desktop":
                    return DeviceFormFactor.Desktop;
                default:
                    return DeviceFormFactor.Undefined;
            }
        }

        public static readonly DependencyProperty FormFactorProperty =
            DependencyProperty.Register("FormFactor", typeof(DeviceFormFactor), typeof(DeviceFormFactorTrigger), 
                new PropertyMetadata(DeviceFormFactor.Undefined, FormFactorChanged));

        public DeviceFormFactor FormFactor
        {
            get { return (DeviceFormFactor)GetValue(FormFactorProperty); }
            set { SetValue(FormFactorProperty, value);}
        }

        private static void FormFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var trigger = (DeviceFormFactorTrigger)d;
            var value = (DeviceFormFactor)e.NewValue;

            trigger.IsActive = (value == CurrentFormFactor) || (value == DeviceFormFactor.Undefined);
        }

        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive == value) return;
                isActive = value;
                SetActive(value);
            }
        }
    }
}
