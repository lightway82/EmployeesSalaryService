using System;
using System.Collections;
using Avalonia.Data.Converters;

namespace DemoApp.ValueConverters;

public class CollectionEmptyConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is ICollection collection)
        {
            return collection.Count == 0;
        }

        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}