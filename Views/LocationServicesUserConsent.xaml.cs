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

namespace SeeClickFix.WP8.Views
{
    public partial class LocationServicesUserConsent : PhoneApplicationPage
    {
        public LocationServicesUserConsent()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.SaveUserConsent(true);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.SaveUserConsent(false);
        }

        void SaveUserConsent(bool agrees)
        {
            AppSettingsService.Inst.Settings.WasLocationServicesConsentDisplayed = true;
            AppSettingsService.Inst.Settings.UseLocationServices = agrees;
            AppSettingsService.Inst.Save();
            NavigationService.Navigate(Constants.MainPageUri);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.NavigationService.RemoveBackEntry();
            base.OnNavigatedTo(e);
        }
    }
}