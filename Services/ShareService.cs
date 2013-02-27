using Microsoft.Phone.Tasks;
using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Services
{
    public class ShareService
    {
        public static ShareService Inst = new ShareService();

        ShareService()
        {
        }

        public void Share(Issue issue, ShareServiceType type)
        {
            switch (type)
            {
                case ShareServiceType.Facebook:
                case ShareServiceType.Twitter:
                case ShareServiceType.More:
                    this.ShareByAddThis(issue, type);
                    break;
                case ShareServiceType.Email:
                    this.ShareByEmail(issue);
                    break;
            }
        }

        void ShareByAddThis(Issue issue, ShareServiceType type)
        {
            string fwparam = string.Empty;
            if (type != ShareServiceType.More)
            {
                fwparam = string.Format("forward/{0}/", HttpUtility.UrlEncode(type.ToString().ToLowerInvariant()));
            }

            string url = string.Format("http://api.addthis.com/oexchange/0.8/{0}offer?url=http://seeclickfix.com/issues/{2}&title={3}&description={4}&username=seeclickfix&template=",
                                fwparam,
                                HttpUtility.UrlEncode(type.ToString().ToLowerInvariant()),
                                HttpUtility.UrlEncode(issue.Id.ToString()),
                                HttpUtility.UrlEncode(issue.Summary),
                                HttpUtility.UrlEncode(issue.Description));
            WebBrowserTask task = new WebBrowserTask()
            {
                Uri = new Uri(url, UriKind.RelativeOrAbsolute)
            };
            task.Show();
        }

        void ShareByEmail(Issue issue)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask()
            {
                Subject = string.Format("Please vote to get this issue fixed"),
                Body = string.Format("{0}\n\n{1}\n\nYou can vote this issue here {2}", issue.Summary, issue.Description, issue.Bitly)
            };
            emailComposeTask.Show();
        }
    }
}
