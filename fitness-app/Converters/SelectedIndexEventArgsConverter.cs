using System.Globalization;
using fitness_app.Controls;

namespace fitness_app.Converters;

public class SelectedIndexEventArgsConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SelectedIndexChangedEventArgs args)
        {
            return args.NewIndex; 
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}