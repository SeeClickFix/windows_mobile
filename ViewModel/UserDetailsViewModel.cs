using GalaSoft.MvvmLight.Command;
using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeClickFix.WP8.ViewModel
{
    public class UserDetailsViewModel : BaseViewModel
    {
        User user;
        public User User
        {
            get { return this.user; }
            private set
            {
                this.user = value;
                this.RaisePropertyChanged("User");
            }
        }

        public RelayCommand LogoutCommand { get; private set; }

        public UserDetailsViewModel()
        {
            this.IsBusy = true;
            this.LogoutCommand = new RelayCommand(this.Logout);
        }

        public void Logout()
        {
            this.UserProfileService.Clear();
            this.NavigationService.GoBack();
        }

        public async void LoadUser(int userId)
        {
            this.IsBusy = true;

            var user = await this.SCFDataService.GetUser(userId);
            if (user != null)
            {
                this.User = user;
            }

            this.IsBusy = false;
        }


    }
}
