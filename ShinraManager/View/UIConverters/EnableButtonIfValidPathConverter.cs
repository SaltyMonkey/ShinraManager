using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ShinraManager.Views.UIConverters
{
    internal class EnableButtonIfValidPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Path.GetFullPath(value.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}