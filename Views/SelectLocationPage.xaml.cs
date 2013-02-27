using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SeeClickFix.WP8.ViewModel;
using System.Device.Location;

namespace SeeClickFix.WP8.Views
{
    public partial class SelectLocationPage : PhoneApplicationPage
    {
        public SelectLocationPage()
        {
            InitializeComponent();
        }

        private void RadTextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            this.RadTexBox.ActionButtonVisibility = System.Windows.Visibility.Visible;
        }

        private void RadTextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            this.RadTexBox.ActionButtonVisibility = System.Windows.Visibility.Collapsed;
        }

        private void RadTexBox_ActionButtonTap_1(object sender, EventArgs e)
        {
            this.map.Focus();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var queryString = this.NavigationContext.QueryString;
            double lat = double.Parse(queryString["lat"]);
            double lng = double.Parse(queryString["lng"]);
            bool isSelectingReportLocation = bool.Parse(queryString["isSelectingReportLocation"]);

            SelectLocationViewModel vm = this.DataContext as SelectLocationViewModel;
            vm.IsSelectingReportLocation = isSelectingReportLocation;
            vm.GeoLocation = new GeoCoordinate(lat, lng);
        }
    }
}