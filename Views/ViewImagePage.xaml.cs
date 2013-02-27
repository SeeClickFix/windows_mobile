using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SeeClickFix.WP8.Common;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls.SlideView;

namespace SeeClickFix.WP8.Views
{
    public partial class ViewImagePage : PhoneApplicationPage
    {
        public ViewImagePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var queryString = this.NavigationContext.QueryString;
            Uri uri = new Uri(queryString["ImageUri"], UriKind.RelativeOrAbsolute);
            this.PanAndZoomImage.Source = new BitmapImage(uri);
        }
    }
}