using System;
using System.Collections;
using Avalonia.Data.Converters;

namespace DemoApp.ValueConverters;

public class ObjectToTypeNameConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value.GetType().Name;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}