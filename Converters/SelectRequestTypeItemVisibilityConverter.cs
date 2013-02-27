using SeeClickFix.WP8.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SeeClickFix.WP8.Converters
{
    /// <summary>
    /// Used to select visibility in the Fullmode popup of ListPicker
    ///  where we hide the 'Select item's item
    /// </summary>
    public class SelectRequestTypeItemVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            RequestTypeViewModel vm = (RequestTypeViewModel)value;
            return vm.IsDummyItem() ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
