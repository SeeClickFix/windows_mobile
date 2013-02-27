using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SeeClickFix.WP8.Converters
{
    public class IssueStatusBrushConverter : IValueConverter
    {
        static Brush AcknowledgedBrush = (Brush)App.Current.Resources["IssueAcknowledgedBrush"];
        static Brush ArchivedBrush = (Brush)App.Current.Resources["IssueArchivedBrush"];
        static Brush OpenBrush = (Brush)App.Current.Resources["IssueOpenBrush"];
        static Brush ClosedBrush = (Brush)App.Current.Resources["IssueClosedBrush"];

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush brush = null;
            var status = (IssueStatus)value;
            switch (status)
            {
                case IssueStatus.Acknowledged:
                    brush = AcknowledgedBrush;
                    break;
                case IssueStatus.Archived:
                    brush = ArchivedBrush;
                    break;
                case IssueStatus.Closed:
                    brush = ClosedBrush;
                    break;
                case IssueStatus.Open:
                    brush = OpenBrush;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("status");
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
