using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SeeClickFix.WP8.Converters
{
    public class ObjectToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility;
            if (value is bool)
            {
                visibility = (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is string)
            {
                visibility = (string.IsNullOrWhiteSpace((string)value) ? Visibility.Collapsed : Visibility.Visible);
            }
            else if (value is int)
            {
                visibility = ((int)value > 0 ? Visibility.Visible : Visibility.Collapsed);
            }
            else if (value is double)
            {
                visibility = ((double)value > 0 ? Visibility.Visible : Visibility.Collapsed);
            }
            else if (value is long)
            {
                visibility = ((long)value > 0 ? Visibility.Visible : Visibility.Collapsed);
            }
            else if (value is float)
            {
                visibility = ((float)value > 0 ? Visibility.Visible : Visibility.Collapsed);
            }
            else if (value is ICollection)
            {
                visibility = ((value as ICollection).Count > 0 ? Visibility.Visible : Visibility.Collapsed);
            }
            else
            {
                visibility = (value != null) ? Visibility.Visible : Visibility.Collapsed;
            }

            if (parameter != null)
            {
                bool negate = false;    

                string strParam = (string)parameter;
                if (strParam.ToLowerInvariant() == "inverse")
                {
                    negate = true;
                }
                else
                {
                    negate = strParam.StartsWith("!");
                    if (negate)
                    {
                        strParam = strParam.Substring(1);
                    }
                    visibility = (value.ToString() == strParam ? Visibility.Visible : Visibility.Collapsed);
                }

                if (negate)
                {
                    visibility = (visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
                }
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
