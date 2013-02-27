using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.ViewModel
{
    public class MessagesViewModel : BaseViewModel
    {
        GetUserMessagesQuery query = new GetUserMessagesQuery();
        public ObservableCollection<Message> messages = new ObservableCollection<Message>();

        public ReadOnlyObservableCollection<Message> Messages { get; private set; }
        public RelayCommand LoadMoreCommand { get; private set; }
        public RelayCommand<Message> ShowIssueDetailsCommand { get; private set; }

        public MessagesViewModel()
        {
            this.ShowIssueDetailsCommand = new RelayCommand<Message>(this.ShowIssueDetails);
            this.LoadMoreCommand = new RelayCommand(this.LoadMoreMessages);
            this.Messages = new ReadOnlyObservableCollection<Message>(this.messages);
            this.SubcribeToUserProfileChanges();
        }

        void ResetMessages()
        {
            this.query.Page = 1;
            this.messages.Clear();
            this.IsBusy = false;
            this.LoadMessages();
        }

        async void LoadMessages()
        {
            if (this.IsBusy)
            {
                return;
            }

            if (this.UserProfileService.IsLogged)
            {
                var userProfile = this.UserProfileService.UserProfile;
                this.query.UserId = userProfile.UserId.Value;
                this.query.Username = userProfile.Email;
                this.query.Password = userProfile.Password;
                this.IsBusy = true;
                try
                {
                    var messages = await this.SCFDataService.GetUserMessages(this.query);
                    if (messages != null)
                    {
                        foreach (Message m in messages)
                        {
                            this.messages.Add(m);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    // error
                }
                this.IsBusy = false;
            }
            this.RaisePropertyChanged("Messages");
        }

        void LoadMoreMessages()
        {
            if (this.IsBusy)
            {
                return;
            }

            if (this.Messages.Count > 0)
            {
                this.query.Page++;
            }

            this.LoadMessages();
        }

        protected override void OnLoginProfileChanged()
        {
            this.ResetMessages();
        }

        async void ShowIssueDetails(Message m)
        {
            if (m.IssueId != null && m.IssueId != 0)
            {
                this.IsBusy = true;
                try
                {
                    var issue = await this.SCFDataService.GetIssue(m.IssueId.Value, this.UserProfileService.UserProfile.Email);
                    if (issue != null)
                    {
                        var viewModel = SimpleIoc.Default.GetInstance<IssueListViewModel>();
                        viewModel.ShowIssueDetailsCommand.Execute(issue);
                        this.IsBusy = false;
                        this.NavigationService.NavigateTo(Constants.ShowIssueDetailsPageUri);
                    }
                }
                catch (Exception ex)
                {
                    // TODO
                    this.IsBusy = false;
                }
            }

            
            // var viewModel = SimpleIoc.Default.GetInstance<IssueDetailsViewModel>();
        }
    }
}
