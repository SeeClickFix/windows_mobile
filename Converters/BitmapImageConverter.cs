using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SeeClickFix.WP8.Converters
{
    public class BitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (value is string && string.IsNullOrWhiteSpace((string)value)))
            {
                if (!string.IsNullOrWhiteSpace((string)parameter))
                {
                    return ImageCache.GetImage((string)parameter);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (value is string)
                {
                    string url = HttpUtility.UrlDecode((string)value);
                    // keep lowercased uri separated because links case sensitivy matters!
                    string lowercasedUrl = url.ToLowerInvariant();

                    if ((lowercasedUrl.Contains(SeeClickFixApi.Domain.ToLowerInvariant())) &&
                        lowercasedUrl.Contains("/images/categories/".ToLowerInvariant()))
                    {
                        // create uri from original string because links case sensitivy matters!
                        int i = lowercasedUrl.IndexOf("categories");
                        url = url.Substring(0, i) + "categories_trans" + url.Substring(i + "categories".Length);
                    }
                    
                    Uri uri;
                    if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
                    {
                        if (!uri.IsAbsoluteUri)
                        {
                            uri = new Uri(new Uri(string.Format("http://{0}", SeeClickFixApi.Domain), UriKind.Absolute), uri);
                        }
                        return new BitmapImage(uri);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                    if (value is Uri)
                    {
                        return new BitmapImage((Uri)value);
                    }
                    else if (value is Stream)
                    {
                        var img = new BitmapImage()
                        {
                            CreateOptions = BitmapCreateOptions.None
                        };
                        img.SetSource(value as Stream);
                        return img;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
