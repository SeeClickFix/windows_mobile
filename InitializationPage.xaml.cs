using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SeeClickFix.WP8.Services;
using SeeClickFix.WP8.Common;

namespace SeeClickFix.WP8
{
    public partial class InitializationPage : PhoneApplicationPage
    {
        public InitializationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var uri = AppSettingsService.Inst.Settings.WasLocationServicesConsentDisplayed ?
                Constants.MainPageUri : Constants.LocationServicesUserConsentUri;

            this.NavigationService.Navigate(uri);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }
    }
}