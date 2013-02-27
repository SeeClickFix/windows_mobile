using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SeeClickFix.WP8.Converters
{
    public class IssueImageConverter : IValueConverter
    {
        static BitmapImageConverter BitmapImageConverter = App.Current.Resources["BitmapImageConverter"] as BitmapImageConverter;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Issue issue = (Issue)value;

            // check if reporter attached an image
            if (!string.IsNullOrWhiteSpace(issue.SquarePublicFilename))
            {
                return BitmapImageConverter.Convert(issue.SquarePublicFilename, targetType, parameter, culture);
            }
            else if (!string.IsNullOrWhiteSpace(issue.Image))
            {
                // check if image is a stock one
                // if it's not this means it's an image from comment
                if (SCFImageUtils.IsStockImage(issue.Image))
                {
                    // stock image
                    return BitmapImageConverter.Convert(issue.Image, targetType, parameter, culture);
                }
                else
                {
                    // image from comments, don't display it
                    return BitmapImageConverter.Convert(null, targetType, parameter, culture);
                }
            }
            else 
            {
                // no image
                return BitmapImageConverter.Convert(issue.Image, targetType, parameter, culture);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
