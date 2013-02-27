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
using GalaSoft.MvvmLight;

namespace SeeClickFix.WP8.Views
{
    public partial class NewCommentPage : PhoneApplicationPage
    {
        public NewCommentPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModelLocator.Instance.CleanUpNewComment();
            }

            base.OnNavigatedFrom(e);
        }

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}