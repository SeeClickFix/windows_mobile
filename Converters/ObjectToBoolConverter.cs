using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SeeClickFix.WP8.Converters
{
    public class ObjectToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool ret = false;
            if (value == null)
            {
                ret = false;
            }
            else if (value is int)
            {
                ret = (int)value > 0;
            }
            if(parameter != null && (((string)(parameter)).ToLowerInvariant() == "inverse"))
            {
                ret = !ret;
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
