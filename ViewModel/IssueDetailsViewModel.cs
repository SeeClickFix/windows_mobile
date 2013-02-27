using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Shell;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SeeClickFix.WP8.ViewModel
{
    public class IssueDetailsViewModel : BaseViewModel
    {
        public Issue Issue
        {
            get;
            private set;
        }

        public RelayCommand VoteToFixIssueCommand { get; private set; }
        public RelayCommand FollowIssueCommand { get; private set; }
        public ICommand ShareIssueCommand { get; private set; }
        public RelayCommand CloseOrReopenIssueCommand { get; private set; }
        public RelayCommand FlagIssueCommand { get; private set; }
        public RelayCommand ShowIssueOnMapCommand { get; private set; }

        public IssueCommentsViewModel IssueComments
        {
            get;
            private set;
        }

        public IssueDetailsViewModel()
        {
            this.VoteToFixIssueCommand = new RelayCommand(this.VoteToFixIssue, () => { return !this.IsBusy && this.Issue.Status != IssueStatus.Closed && !this.Issue.WasVoted; });
            this.FollowIssueCommand = new RelayCommand(this.FollowIssue, () => { return !this.IsBusy && !this.Issue.IsFollowing; });
            this.ShareIssueCommand = new RelayCommand(this.ShareIssue, () => !this.IsBusy);
            this.CloseOrReopenIssueCommand = new RelayCommand(this.CloseOrReopenIssue, this.CanCloseOrReopenIssue);
            this.FlagIssueCommand = new RelayCommand(this.FlagIssue, () => !this.IsBusy);
            this.ShowIssueOnMapCommand = new RelayCommand(this.ShowOnMap, () => !this.IsBusy);

            this.IssueComments = new IssueCommentsViewModel();

            var viewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
            this.Issue = viewModel.IssueList.SelectedIssue;

            Messenger.Default.Register<int>(this, Messages.RefreshIssue, (issueId) =>
            {
                this.IsBusy = true;
                this.RefreshIssue(issueId);
                this.IsBusy = false;
            });
        }

        public bool CanCloseOrReopenIssue()
        {
            return !this.IsBusy && this.Issue.Status != IssueStatus.Acknowledged;
        }

        public override void Cleanup()
        {
            base.Cleanup();
            this.IssueComments.Cleanup();
        }

        async void RefreshIssue(int issueId)
        {
            // TODO Handle error
            var issue = await this.SCFDataService.GetIssue(issueId, UserProfileService.Inst.UserProfile.Email);
            if (issue != null)
            {
                var viewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
                viewModel.IssueList.SelectedIssue = issue;

                this.IssueComments.RefreshComments();

                this.Issue = issue;
                this.RaisePropertyChanged("Issue");

                this.RaiseCommandsCanExecuteChanged();
            }
        }

        private void VoteToFixIssue()
        {
            if (this.UserProfileService.IsLogged)
            {
                this.IsBusy = true;
                VoteIssueViewModel voteIssueViewModel = new VoteIssueViewModel();
                voteIssueViewModel.SendVote();
                this.IsBusy = false;
                voteIssueViewModel.Cleanup();
            }
            else
            {
                this.NavigationService.NavigateTo(Constants.VoteIssuePageUri);
            }
        }

        async private void FollowIssue()
        {
            if (!string.IsNullOrWhiteSpace(UserProfileService.Inst.UserProfile.Email))
            {
                this.IsBusy = true;
                await this.SCFDataService.FollowIssue(this.Issue.Id, UserProfileService.Inst.UserProfile.Email);
                this.RefreshIssue(this.Issue.Id);
                this.IsBusy = false;
            }
        }

        private void CloseOrReopenIssue()
        {
            IssueHistoryItemType possibleItemType;
            if (this.Issue.Status == IssueStatus.Open ||
                this.Issue.Status == IssueStatus.Archived)
            {
                possibleItemType = IssueHistoryItemType.Closed;
            }
            else if (this.Issue.Status == IssueStatus.Closed)
            {
                possibleItemType = IssueHistoryItemType.Reopened;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Issue.Status");
            }

            // PhoneApplicationService.Current.State["NewCommentType"] = possibleItemType;
            //Uri uri = new Uri(Constants.NewCommentPageUri, string.Format("commentType={0}", possibleItemType));
            this.NavigationService.NavigateTo(Constants.NewCommentPageUri);

            ViewModelLocator.Instance.NewComment.CommentType = possibleItemType;
        }

        private void ShareIssue()
        {
            this.NavigationService.NavigateTo(Constants.ShareIssuePageUri);
        }

        private void FlagIssue()
        {
            this.NavigationService.NavigateTo(Constants.FlagIssuePageUri);
        }

        void ShowOnMap()
        {
            NavigationService.NavigateTo(Constants.MapPageUri);
        }

        protected override void OnBusyChanged()
        {
            this.RaiseCommandsCanExecuteChanged();
        }

        void RaiseCommandsCanExecuteChanged()
        {
            this.VoteToFixIssueCommand.RaiseCanExecuteChanged();
            this.CloseOrReopenIssueCommand.RaiseCanExecuteChanged();
            this.FlagIssueCommand.RaiseCanExecuteChanged();
            this.FollowIssueCommand.RaiseCanExecuteChanged();
        }
    }
}
