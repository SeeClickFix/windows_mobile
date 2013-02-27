using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.Infrastructure;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeClickFix.WP8.ViewModel
{
    public class LoginViewModel : BaseViewModelState
    {
        string email;
        [Stateful(ApplicationStateType.Transient)]
        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.RaisePropertyChanged("Email");
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        string password;
        [Stateful(ApplicationStateType.Transient)]
        public string Password
        {
            get { return this.password; }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    this.RaisePropertyChanged(Password);
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            this.LoginCommand = new RelayCommand(this.Login, this.CanLoginExecute);
        }

        bool CanLoginExecute()
        {
            return
                !this.IsBusy &&
                !string.IsNullOrWhiteSpace(this.Email) &&
                !string.IsNullOrWhiteSpace(this.Password) &&
                EmailValidator.IsValid(this.Email);
        }

        async private void Login()
        {
            this.IsBusy = true;
            this.LoginCommand.RaiseCanExecuteChanged();
            var userLogin = await this.SCFDataService.Login(this.Email, this.Password);
            this.IsBusy = false;
            this.LoginCommand.RaiseCanExecuteChanged();

            if (userLogin != null && userLogin.Id != 0)
            {
                UserProfileService.Inst.UserProfile.Email = userLogin.Email;
                UserProfileService.Inst.UserProfile.Name = userLogin.Name;
                UserProfileService.Inst.UserProfile.Password = this.Password;
                UserProfileService.Inst.UserProfile.CanAcknowledge = userLogin.CanAcknowledge;
                UserProfileService.Inst.UserProfile.UserId = userLogin.Id;
                UserProfileService.Inst.Save();

                //Messenger.Default.Send(userLogin, Messages.RefreshUserLogin);
                this.NavigationService.GoBack();
            }
            else
            {
                // TODO: handle error
                MessageBox.Show("Login failed");
            }
        }
    }
}
