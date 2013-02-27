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
    public class IssueHistoryItemTypeBrushConverter : IValueConverter
    {
        static Brush AcknowledgedBrush = (Brush)App.Current.Resources["IssueAcknowledgedBrush"];
        static Brush OpenBrush = (Brush)App.Current.Resources["IssueOpenBrush"];
        static Brush ClosedBrush = (Brush)App.Current.Resources["IssueClosedBrush"];
        static Brush IssueAssignedBrush = (Brush)App.Current.Resources["IssueAssignedBrush"];
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush brush = null;
            var itemType = (IssueHistoryItemType)value;
            switch (itemType)
            {
                case  IssueHistoryItemType.Acknowledged:
                    brush = AcknowledgedBrush;
                    break;
                case  IssueHistoryItemType.Closed:
                    brush = ClosedBrush;
                    break;
                case IssueHistoryItemType.Reopened:
                case IssueHistoryItemType.Opened:
                    brush = OpenBrush;
                    break;
                case IssueHistoryItemType.Assignment:
                    brush = IssueAssignedBrush;
                    break;
                case IssueHistoryItemType.Comment:
                case IssueHistoryItemType.WatcherAdded:
                case IssueHistoryItemType.Voted:
                    brush = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("itemType");
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
