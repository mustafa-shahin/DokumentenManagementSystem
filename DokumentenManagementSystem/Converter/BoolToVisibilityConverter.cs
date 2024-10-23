using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DokumentenManagementSystem.Converter;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool booleanValue)
            return booleanValue ? Visibility.Visible : Visibility.Collapsed;
        else if (value == null)
            return Visibility.Collapsed;
        else
            return Visibility.Visible;
    }



    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}