using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using SeeClickFix.WP8.Infrastructure;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.ViewModel
{
    public class FlagIssueViewModel : BaseViewModelState
    {
        public Issue Issue { get; private set; }

        string message;
        [Stateful(ApplicationStateType.Transient)]
        public string Message
        {
            get { return this.message; }
            set
            {
                if (this.message != value)
                {
                    this.message = value;
                    this.RaisePropertyChanged("Message");
                    this.SendMessageCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand SendMessageCommand { get; private set; }

        public FlagIssueViewModel()
        {
            var viewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
            this.Issue = viewModel.IssueList.SelectedIssue;
            this.SendMessageCommand = new RelayCommand(this.SendMessage,
                () => { return !this.IsBusy; });
        }

        async private void SendMessage()
        {
            this.IsBusy = true;
            this.SendMessageCommand.RaiseCanExecuteChanged();
            var response = await SCFDataService.FlagIssue(this.Issue.Id, this.Message);
            this.IsBusy = false;

            Messenger.Default.Send<int>(this.Issue.Id, Messages.RefreshIssue);
            NavigationService.GoBack();
        }
    }
}
