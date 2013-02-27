using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SeeClickFix.WP8.Converters
{
    public class MultibindingVisibilityConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var visibilityArray = values.OfType<Visibility>();
            if (visibilityArray.Count() != values.Count())
            {
                return Visibility.Collapsed;
            }
            else
            {
                return visibilityArray.Contains(Visibility.Collapsed) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
