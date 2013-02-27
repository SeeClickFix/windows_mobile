using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    public static class IssueHistoryItemTypeExtensions
    {
        public static IssueHistoryItemType Parse(string itemType)
        {
            switch (itemType)
            {
                case "System Comment":
                case "Comment":
                    return IssueHistoryItemType.Comment;
                case "Issue Closed":
                    return IssueHistoryItemType.Closed;
                case "Issue Opened":
                    return IssueHistoryItemType.Opened;
                case "Issue Reopened":
                    return IssueHistoryItemType.Reopened;
                case "Watcher Added":
                    return IssueHistoryItemType.WatcherAdded;
                case "Issue Voted For":
                    return IssueHistoryItemType.Voted;
                case "Issue Acknowledged":
                    return IssueHistoryItemType.Acknowledged;
                case "Assignment":
                    return IssueHistoryItemType.Assignment;
                default:
                    return IssueHistoryItemType.Comment;
                // default:
                    // throw new ArgumentOutOfRangeException("itemType");
            }
        }

        public static string ToQueryStringValue(this IssueHistoryItemType itemType)
        {
            switch (itemType)
            {
                case IssueHistoryItemType.Comment:
                    return "Comment";
                case IssueHistoryItemType.Closed:
                    return "Issue Closed";
                case IssueHistoryItemType.Opened:
                    return "Issue Opened";
                case IssueHistoryItemType.Reopened:
                    return "Issue Reopened";
                case IssueHistoryItemType.WatcherAdded:
                    return "Watcher Added";
                case IssueHistoryItemType.Voted:
                    return "Issue Voted For";
                case IssueHistoryItemType.Acknowledged:
                    return "Issue Acknowledged";
                case IssueHistoryItemType.Assignment:
                    return "Assignment";
                default:
                    throw new ArgumentOutOfRangeException("itemType");
            }
        }
    }
}
