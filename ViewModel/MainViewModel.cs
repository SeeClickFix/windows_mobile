using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using SeeClickFix.WP8.Common;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Ioc;
using System;

namespace SeeClickFix.WP8.ViewModel
{
    public class MainViewModel : BaseViewModelState
    {
        public IssueListViewModel IssueList { get; private set; }
        public ICommand ShowUserProfileCommand { get; private set; }

        public MainViewModel()
        {
            this.ShowUserProfileCommand = new RelayCommand(this.ShowUserProfile);
            this.IssueList = SimpleIoc.Default.GetInstance<IssueListViewModel>();
            this.SubcribeToUserProfileChanges();
        }

        public override void LoadState(IDictionary<string, object> persistentStateDictionary, IDictionary<string, object> transientStateDictionary, bool shouldLoadTransientState)
        {
            this.IssueList.LoadState(persistentStateDictionary, transientStateDictionary, shouldLoadTransientState);
        }

        public override void SaveState(IDictionary<string, object> persistentStateDictionary, IDictionary<string, object> transientStateDictionary)
        {
            this.IssueList.SaveState(persistentStateDictionary, transientStateDictionary);
        }

        void ShowUserProfile()
        {
            var userId = UserProfileService.Inst.UserProfile.UserId;
            if (userId != null)
            {
                SimpleIoc.Default.GetInstance<INavigationService>().NavigateTo(
                    new Uri(string.Format("{0}?userId={1}", Constants.UserProfilePageUri, userId.Value), UriKind.Relative));
            }
        }
    }
}