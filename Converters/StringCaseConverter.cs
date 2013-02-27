using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SeeClickFix.WP8.Converters
{
    public class StringCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = (string)value;
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            string p = (string)parameter;
            if (p == null || string.Compare(p, "firstcase", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                // first case
                return s = string.Format("{0}{1}", s.Substring(0, 1).ToUpperInvariant(), s.Substring(1));
            }
            if (string.Compare(p, "uppercase", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                // uppercase
                return s.ToUpperInvariant();
            }
            else if (string.Compare(p, "lowercase", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                // lowercase
                return s.ToLowerInvariant();
            }
            else
            {
                return s;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
