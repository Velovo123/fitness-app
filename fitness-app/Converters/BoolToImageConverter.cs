using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace fitness_app.Converters;

public class BoolToImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isHidden)
        {
            return isHidden ? "eye_disabled" : "eye_enabled"; 
        }
        return "eye_disabled"; 
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}