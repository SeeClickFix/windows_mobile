using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SeeClickFix.WP8.Converters
{
    public class UserProfileStatsNavigateUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            User user = (User)value;
            if (user == null)
            {
                return null;
            }

            IssueHistoryItemType statsType = (IssueHistoryItemType)Enum.Parse(typeof(IssueHistoryItemType), (string)parameter);
            switch (statsType)
            {
                case IssueHistoryItemType.Comment:
                    return user.CommentsCount > 0 ? new Uri(string.Format("/Views/UserCommentPage&userId={0}", user.Id), UriKind.RelativeOrAbsolute) : null;
                case IssueHistoryItemType.Closed:
                case IssueHistoryItemType.Opened:
                case IssueHistoryItemType.Voted:
                case IssueHistoryItemType.WatcherAdded:
                case IssueHistoryItemType.Acknowledged:
                    return user.ClosedIssueCount > 0 ? new Uri(string.Format("/Views/UserIssues?issuetype={0}&userId={1}", statsType, user.Id), UriKind.RelativeOrAbsolute) : null;
                default:
                    throw new ArgumentOutOfRangeException("statsType");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public enum UserProfileStatsType
    {
        ReportedIssues,
        VotedIssues,
        FollowingIssues,
        ClosedIssues,
        Comments
    }
}
