using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.ViewModel;

namespace SeeClickFix.WP8.Views
{
    public partial class FilterSettingsPage : PhoneApplicationPage
    {
        List<IssueStatus> statusFilter;
        bool ignoreSelectionChanged = false;
        IssueListViewModel viewModel;

        public FilterSettingsPage()
        {
            InitializeComponent();
            
            this.viewModel = this.DataContext as IssueListViewModel;

            // load statuses
            this.statusFilter = new List<IssueStatus>(this.viewModel.SearchFilter.Status ?? new IssueStatus[] {} );
            this.StatusList.ItemsSource = Enum.GetValues(typeof(IssueStatus));
            this.StatusList.ItemRealized += StatusList_ItemRealized;

            // load keyword
            this.TextBoxKeyword.Text = this.viewModel.SearchFilter.Keyword ?? string.Empty;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (e.NavigationMode == NavigationMode.Back)
            {
                this.viewModel.SearchFilter = new SearchFilter()
                    {
                        Status = this.statusFilter.ToArray(),
                        Keyword = this.TextBoxKeyword.Text
                    };

                this.viewModel.Cleanup();
            }
        }

        void StatusList_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            // select the items
            var itemContainer = this.StatusList.ContainerFromItem(e.Container.Content) as LongListMultiSelectorItem;
            this.ignoreSelectionChanged = true;
            itemContainer.IsSelected = this.statusFilter.Contains((IssueStatus)itemContainer.Content);
            this.ignoreSelectionChanged = false;
        }

        private void StatusList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ignoreSelectionChanged)
            {
                return;
            }
            
            if (e.AddedItems.Count > 0)
            {
                foreach (IssueStatus status in e.AddedItems)
                {
                    if (!this.statusFilter.Contains(status))
                    {
                        this.statusFilter.Add(status);
                    }
                }
            }

            if (e.RemovedItems.Count > 0)
            {
                foreach (IssueStatus status in e.RemovedItems)
                {
                    if (this.statusFilter.Contains(status))
                    {
                        this.statusFilter.Remove(status);
                    }
                }
            }
        }
    }
}