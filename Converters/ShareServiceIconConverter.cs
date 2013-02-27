using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SeeClickFix.WP8.Converters
{
    public class ShareServiceIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ShareServiceType t = (ShareServiceType)value;
            string fileName = (t == ShareServiceType.More ? "tiles.nine" : t.ToString().ToLowerInvariant());
            return new BitmapImage(new Uri(string.Format("/Assets/{0}.png", fileName), UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
