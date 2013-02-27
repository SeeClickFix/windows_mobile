using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Common
{
    public static class Constants
    {
        public static readonly Uri MainPageUri = new Uri("/MainPage.xaml", UriKind.Relative);
        public static readonly Uri ReportIssuePageUri = new Uri("/Views/ReportIssuePage.xaml", UriKind.Relative);
        public static readonly Uri SelectLocationPageUri = new Uri("/Views/SelectLocationPage.xaml", UriKind.Relative);
        public static readonly Uri FilterIssuesPageUri = new Uri("/Views/FilterSettingsPage.xaml", UriKind.Relative);
        public static readonly Uri ShowIssueDetailsPageUri = new Uri("/Views/IssueDetailsPage.xaml", UriKind.Relative);
        public static readonly Uri NewCommentPageUri = new Uri("/Views/NewCommentPage.xaml", UriKind.Relative);
        public static readonly Uri ShareIssuePageUri = new Uri("/Views/ShareIssuePage.xaml", UriKind.Relative);
        public static readonly Uri UserProfilePageUri = new Uri("/Views/UserProfilePage.xaml", UriKind.Relative);
        public static readonly Uri VoteIssuePageUri = new Uri("/Views/VoteIssuePage.xaml", UriKind.Relative);
        public static readonly Uri FlagIssuePageUri = new Uri("/Views/FlagIssuePage.xaml", UriKind.Relative);
        public static readonly Uri MapPageUri = new Uri("/Views/MapPage.xaml", UriKind.Relative);
        public static readonly Uri ViewImagePageUri = new Uri("/Views/ViewImagePage.xaml", UriKind.Relative);
        public static readonly Uri LocationServicesUserConsentUri = new Uri("/Views/LocationServicesUserConsent.xaml", UriKind.Relative);
        public static readonly Uri AboutPageUri = new Uri("/Views/AboutPage.xaml", UriKind.Relative);
    }
}
