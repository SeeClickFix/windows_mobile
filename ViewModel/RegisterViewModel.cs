using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.Infrastructure;
using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeClickFix.WP8.ViewModel
{
    public class RegisterViewModel : BaseViewModelState
    {
        string name;
        [Stateful(ApplicationStateType.Transient)]
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.RaisePropertyChanged("Name");
                    this.RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

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
                    this.RegisterCommand.RaiseCanExecuteChanged();
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
                    this.RaisePropertyChanged("Password");
                    this.RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        bool isTOSAgreed;
        [Stateful(ApplicationStateType.Transient)]
        public bool IsTOSAgreed
        {
            get { return this.isTOSAgreed; }
            set
            {
                if (this.isTOSAgreed != value)
                {
                    this.isTOSAgreed = value;
                    this.RaisePropertyChanged("IsTOSAgreed");
                    this.RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand RegisterCommand { get; private set; }

        public RegisterViewModel()
        {
            this.RegisterCommand = new RelayCommand(this.Register, this.CanRegisterExecute);
        }

        bool CanRegisterExecute()
        {
            return 
                !this.IsBusy &&
                !string.IsNullOrWhiteSpace(this.Email) &&
                EmailValidator.IsValid(this.Email) &&
                !string.IsNullOrWhiteSpace(this.Password) &&
                !string.IsNullOrWhiteSpace(this.Name) &&
                this.IsTOSAgreed;
        }

        async private void Register()
        {
            if (!this.IsTOSAgreed)
            {
                MessageBox.Show("You need to agree with the terms of use to register");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.Password) && this.Password.Length < SeeClickFixApi.PasswordMinLength)
                {
                    MessageBox.Show(string.Format("Password must be at least {0} characters", SeeClickFixApi.PasswordMinLength));
                }
                else
                {
                    this.IsBusy = true;
                    this.RegisterCommand.RaiseCanExecuteChanged();
                    var userLogin = await this.SCFDataService.Register(this.Name, this.Email, this.Password);
                    this.IsBusy = false;
                    if (userLogin != null && userLogin.Id != 0)
                    {
                        MessageBox.Show("Thank you for joining!\n\nPlease check your email, we sent you a confirmation email for your account.", "SeeClickFix", MessageBoxButton.OK);
                        //Messenger.Default.Send<UserLogin>(userLogin, Messages.RefreshUserLogin);
                        this.NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("Registration failed.\n\nIf you already have an account, please login.\n\nIf you forgot your password, go to our website http://www.seeclickfix.com and try to recover it.", "SeeClickFix", MessageBoxButton.OK);
                    }
                }
            }
        }
    }
}
