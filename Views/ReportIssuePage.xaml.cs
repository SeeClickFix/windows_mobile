using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Data;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.ViewModel;

namespace SeeClickFix.WP8.Views
{
    public partial class ReportIssuePage : PhoneApplicationPage
    {
        public ReportIssuePage()
        {
            InitializeComponent();

            //var mapOverlay = (this.map.Layers[0] as MapLayer)[0];
            //Binding b = new Binding()
            //{
            //    Path = new PropertyPath("DataContext.SelectLocation.GeoLocation"),
            //    Source = this
            //};
            //MapOverlay
        }

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModelLocator.Instance.CleanUpReportIssue();
            }

            //{
            //    if (e.Uri == Constants.MainPageUri)
            //    {
            //         (e.Content as MainPage).DataContext 
            //    }
            //    else
            //    {

            //    }
            //}
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }
    }
}