using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    public enum IssueHistoryItemType
    {
        Opened,
        Closed,
        Voted,
        Acknowledged,
        Comment,
        WatcherAdded,
        Reopened,
        Assignment
    }
}
