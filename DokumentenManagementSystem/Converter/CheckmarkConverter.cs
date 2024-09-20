using System.Globalization;
using System.Windows.Data;

namespace DokumentenManagementSystem.Converter;

public class CheckmarkConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
        {
            return "\uE73A";
        }

        return "\uE739";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}