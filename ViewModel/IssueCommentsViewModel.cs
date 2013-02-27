using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SeeClickFix.WP8.ViewModel
{
    public class IssueCommentsViewModel : BaseViewModel
    {
        ObservableCollection<IssueHistoryItem> comments = new ObservableCollection<IssueHistoryItem>();

        public ReadOnlyObservableCollection<IssueHistoryItem> Comments { get; private set; }
        public ICommand NewCommentCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public IssueCommentsViewModel()
        {
            this.Comments = new ReadOnlyObservableCollection<IssueHistoryItem>(this.comments);
            this.NewCommentCommand = new RelayCommand(this.NewComment);
            this.RefreshCommand = new RelayCommand(this.GetComments);

            Messenger.Default.Register<string>(this, Messages.RefreshComments, (m) => { this.GetComments(); });

            this.IsBusy = true;
            this.GetComments();
        }

        public void RefreshComments()
        {
            this.GetComments();
        }

        void NewComment()
        {
            this.NavigationService.NavigateTo(Constants.NewCommentPageUri);
        }

        async void GetComments()
        {
            var vm = SimpleIoc.Default.GetInstance<MainViewModel>();
            var issueId = vm.IssueList.SelectedIssue.Id;

            this.IsBusy = true;
            this.comments.Clear();
            var items = await SCFDataService.ListIssueHistory(issueId);

            // return all items except those when opened, watcher is added and issue was voted
            items = items.Where(
                i => i.ItemType != IssueHistoryItemType.WatcherAdded &&
                        i.ItemType != IssueHistoryItemType.Opened &&
                     i.ItemType != IssueHistoryItemType.Voted).ToList();
            foreach (IssueHistoryItem item in items)
            {
                this.comments.Add(item);
            }
            this.IsBusy = false;
        }
    }
}
