using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using SeeClickFix.WP8.ViewModel;
using Telerik.Windows.Controls;

namespace SeeClickFix.WP8.Views
{
    public partial class LoginRegisterPage : PhoneApplicationPage
    {
        public LoginRegisterPage()
        {
            InitializeComponent();
        }

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
               // ViewModelLocator.Instance.CleanUpLogin();
              //  ViewModelLocator.Instance.CleanUpRegister();
            }
        }

        private void RadPasswordBox_PasswordChanged_1(object sender, EventArgs e)
        {
            (sender as RadPasswordBox).GetBindingExpression(RadPasswordBox.PasswordProperty).UpdateSource();
        }
    }
}