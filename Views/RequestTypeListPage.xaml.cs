using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SeeClickFix.WP8.Views
{
    public partial class RequestTypeListPage : ListPickerPage
    {
        public RequestTypeListPage()
        {
            InitializeComponent();
        }

        private void OnPickerTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // We listen to the tap event because SelectionChanged does not fire if the user picks the already selected item.

            // Only close the page in Single Selection mode.
            if (SelectionMode == SelectionMode.Single)
            {
                // Commit the value and close
                SelectedItem = Picker.SelectedItem;
                ClosePickerPage();
            }
        }

        private void ClosePickerPage()
        {
            // Prevent user from selecting an item as the picker is closing,
            // disabling the control would cause the UI to change so instead
            // it's hidden from hittesting.
            Picker.IsHitTestVisible = false;

            //IsOpen = false;
        }
    }
}