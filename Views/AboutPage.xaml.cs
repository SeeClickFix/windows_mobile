using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Reflection;
using Microsoft.Phone.Tasks;

namespace SeeClickFix.WP8.Views
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();

            var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            this.TxtVersion.Text = string.Format("Version {0}", nameHelper.Version);
        }

        private void BtnComposeEmail_Click_1(object sender, RoutedEventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask()
            {
                To = "contact@seeclickfix.com"
            };
            task.Show();
        }

        private void BtnViewTerms_Click_1(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://seeclickfix.com/terms_of_use");
            WebBrowserTask task = new WebBrowserTask()
            {
                Uri = uri
            };
            task.Show();
        }

        private void BtnViewWebsite_Click_1(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://seeclickfix.com");
            WebBrowserTask task = new WebBrowserTask()
            {
                Uri = uri
            };
            task.Show();
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
    }
}