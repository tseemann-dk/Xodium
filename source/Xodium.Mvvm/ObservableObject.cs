using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Xodium.Mvvm
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(property, value)) return false;
            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetProperty(bool condition, Action setter, [CallerMemberName] string propertyName = null)
        {
            if (!condition || setter == null) return false;
            setter();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
