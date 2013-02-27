using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
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
    public class VoteIssueViewModel : BaseViewModel
    {
        public Issue Issue { get; private set; }

        string email;
        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.RaisePropertyChanged("Email");
                    this.SendVoteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand SendVoteCommand { get; private set; }

        public void SendVote()
        {
            this.SendVoteAsync(false);
        }

        public VoteIssueViewModel()
        {
            var viewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
            this.Issue = viewModel.IssueList.SelectedIssue;
            this.SendVoteCommand = new RelayCommand(()=> this.SendVoteAsync(true), this.CanVoteExecute);

            var userProfile = UserProfileService.Inst.UserProfile;
            this.Email = userProfile.Email;

            //Messenger.Default.Register<UserLogin>(this, Messages.RefreshUserLogin, (itemType) =>
            //{
            //    this.OnLoginProfileChanged();
            //});
            this.SubcribeToUserProfileChanges();
        }

        bool CanVoteExecute()
        {
            return
                !this.IsBusy &&
                !string.IsNullOrWhiteSpace(this.Email) &&
                EmailValidator.IsValid(this.Email);
        }

        async private void SendVoteAsync(bool navigateBack = true)
        {
            if (EmailValidator.IsValid(this.Email))
            {
                this.IsBusy = true;
                this.SendVoteCommand.RaiseCanExecuteChanged();

                var response = await SCFDataService.VoteIssue(this.Issue.Id, this.Email);

                // save email
                UserProfileService.Inst.UserProfile.Email = this.Email;
                UserProfileService.Inst.Save();

                this.IsBusy = false;

                Messenger.Default.Send<int>(this.Issue.Id, Messages.RefreshIssue);
                
                if (navigateBack)
                {
                    NavigationService.GoBack();
                }
            }
            else
            {
                MessageBox.Show("Enter a valid email", "SEECLICKFIX", MessageBoxButton.OK);
            }
        }

        protected override void OnLoginProfileChanged()
        {
            // when login profile changed
            var userProfile = UserProfileService.Inst.UserProfile;
            this.Email = userProfile.Email;
        }
    }
}
