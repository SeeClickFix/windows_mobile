using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SeeClickFix.WP8.Converters
{
    public class NewCommentTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IssueHistoryItemType itemType = (IssueHistoryItemType)value;
            switch (itemType)
            {
                case IssueHistoryItemType.Reopened:
                    return "reopen issue";
               case IssueHistoryItemType.Closed:
                    return "close issue";
               case IssueHistoryItemType.Acknowledged:
                    return "acknowledge issue";
                case IssueHistoryItemType.Comment:
                    return "add comment";
                case IssueHistoryItemType.Voted:
                    return "vote";
                default:
                    throw new ArgumentOutOfRangeException("itemType");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
