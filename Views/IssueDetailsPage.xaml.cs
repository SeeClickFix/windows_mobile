using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Input;
using System.Diagnostics;
using AppBarUtils;
using System.Windows.Interactivity;
using SeeClickFix.WP8.ViewModel;
using GalaSoft.MvvmLight;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Views
{
    public partial class IssueDetailsPage : PhoneApplicationPage
    {
        public IssueDetailsPage()
        {
            InitializeComponent();
            
            // this.Pivot.SelectionChanged += Pivot_SelectionChanged_1;
            // (this.DataContext as IssueDetailsViewModel).IssueComments.NewComment.CommentSent += IssueComments_CommentSent;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                (this.DataContext as ViewModelBase).Cleanup();
            }
            base.OnNavigatedFrom(e);
        }

       

        //void IssueComments_CommentSent(Exception error)
        //{
        //    this.HideNewMessagePanelButtons();

        //    var trigger = Interaction.GetTriggers(this.Pivot).OfType<StateChangedTrigger>().First();
        //    trigger.State = 2;
        //    this.GetNewMessageButton();
        //}

        // ApplicationBarIconButton btn;

        //private void Pivot_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    if (this.Pivot.SelectedIndex == 1)
        //    {
        //        this.GetNewMessageButton();
        //    }
        //    else
        //    {
        //        this.HideNewMessagePanelButtons();
        //    }
        //}

        //void IssueDetailsPage_Click(object sender, EventArgs e)
        //{
        //    var trigger = Interaction.GetTriggers(this.Pivot).OfType<StateChangedTrigger>().First();
        //    trigger.State = 1;

        //    this.PanelNewComment.Visibility = System.Windows.Visibility.Visible;
        //    // this.TxtBoxNewComment.Focus();
        //}

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        //void HideNewMessagePanelButtons()
        //{
        //    if (this.btn != null)
        //    {
        //        this.btn.Click -= this.IssueDetailsPage_Click;
        //        this.btn = null;
        //    }

        //    this.PanelNewComment.Visibility = System.Windows.Visibility.Collapsed;
        //}

        //void GetNewMessageButton()
        //{
        //    Dispatcher.BeginInvoke(() =>
        //    {
        //        if (this.ApplicationBar.Buttons.Count > 0)
        //        {
        //            this.btn = this.ApplicationBar.Buttons[0] as ApplicationBarIconButton;
        //            btn.Click += IssueDetailsPage_Click;
        //        }
        //    });
        //}
    }
}