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
using System.Globalization;
using SeeClickFix.WP8.Services;

namespace SeeClickFix.WP8.Views
{
    public partial class UserProfilePage : PhoneApplicationPage
    {
        public UserProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var queryString = this.NavigationContext.QueryString;
            int userId = int.Parse(queryString["userId"]);

            this.ApplicationBar.IsVisible = (UserProfileService.Inst.IsLogged && UserProfileService.Inst.UserProfile.UserId == userId);
            (this.DataContext as UserDetailsViewModel).LoadUser(userId);
        }
    }
}