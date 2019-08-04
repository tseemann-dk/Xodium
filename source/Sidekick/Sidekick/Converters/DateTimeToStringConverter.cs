using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sidekick.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is DateTime dt 
                ? (parameter is string format ? dt.Date.ToString(format) : dt.Date.ToString()) 
                : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
