using System;
using System.Globalization;
using System.Windows.Data;

namespace ShinraManager.Views.UIConverters
{
    internal class MainButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "True")
            {
                return "Disable launch with Tera";
            }
            else
            { return "Enable launch with Tera"; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}