using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SeeClickFix.WP8.Infrastructure;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.ViewModel
{
    public abstract class BaseViewModel : ViewModelBase
    {
        bool isBusy;
        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                if (this.isBusy != value)
                {
                    this.isBusy = value;
                    this.RaisePropertyChanged("IsBusy");
                    this.OnBusyChanged();
                }
            }
        }

        public BaseViewModel()
        {
            this.SCFDataService = SimpleIoc.Default.GetInstance<ISCFDataService>();
        }

        public UserProfileService UserProfileService
        {
            get
            {
                return UserProfileService.Inst;
            }
        }

        protected ISCFDataService SCFDataService
        {
            get;
            private set;
        }

        protected INavigationService NavigationService
        {
            get
            {
                return SimpleIoc.Default.GetInstance<INavigationService>();
            }
        }

        protected virtual void OnBusyChanged() { }

        public override void Cleanup()
        {
            this.SubcribeToUserProfileChanges(false);
            base.Cleanup();
        }

     

        protected void SubcribeToUserProfileChanges(bool subscribe = true)
        {
            if (subscribe)
            {
                this.UserProfileService.UserProfileChanged += UserProfileService_UserProfileChanged;
            }
            else
            {
                this.UserProfileService.UserProfileChanged -= UserProfileService_UserProfileChanged;
            }
        }

        protected virtual void OnLoginProfileChanged()
        {
        }

        void UserProfileService_UserProfileChanged(object sender, UserProfile e)
        {
            this.OnLoginProfileChanged();
            this.RaisePropertyChanged("UserProfileService");
        }
    }
}
