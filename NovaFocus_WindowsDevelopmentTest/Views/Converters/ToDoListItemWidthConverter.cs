using System;
using System.Globalization;
using System.Windows.Data;

namespace NovaFocus_WindowsDevelopmentTest.Views.Converters
{
    public class ToDoListItemWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualWidth && parameter is string parameterValue)
            {
                if (double.TryParse(parameterValue, out double subtractValue))
                {
                    return actualWidth - subtractValue;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
