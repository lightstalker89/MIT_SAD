using System;

namespace VirtualMachineClient.Converter
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;

    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return String.Join(", ", (List<string>)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
