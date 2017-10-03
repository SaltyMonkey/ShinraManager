using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ShinraManager.Converters
{
    internal class EnableToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return "Green";
            return "Red";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}