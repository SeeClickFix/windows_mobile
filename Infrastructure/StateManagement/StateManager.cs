using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Navigation;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SeeClickFix.WP8.Infrastructure
{
    public static class StateManager
    {
        static bool shouldLoadTransientState = false;

        public static bool ShouldLoadTransientState
        {
            get
            {
                return shouldLoadTransientState;
            }
        }

        public static void Initialize()
        {
            PhoneApplicationService.Current.Activated += Current_Activated;
            PhoneApplicationService.Current.Deactivated += OnDeactivated;
            var frame = App.RootFrame;
            frame.Navigating += OnNavigating;
            frame.Navigated += OnNavigated;
        }

        static void Current_Activated(object sender, ActivatedEventArgs e)
        {
            if (e.IsApplicationInstancePreserved)
            {
                // it’s coming to life from dormancy
                // we do not need to do anything special.
            }
            else
            {
                // it's coming to life from tombstoning
                shouldLoadTransientState = true;
            }
        }

        static void OnDeactivated(object sender, DeactivatedEventArgs e)
        {
        }

        static void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var frame = App.RootFrame;
            var element = frame.Content as FrameworkElement;
            if (element != null)
            {
                if (e.NavigationMode != NavigationMode.Back)
                {
                    IStatePreservation preserver = element.DataContext as IStatePreservation;
                    if (preserver != null)
                    {
                        preserver.SaveState(IsolatedStorageSettings.ApplicationSettings, PhoneApplicationService.Current.State);
                    }
                }
                else if (e.NavigationMode == NavigationMode.Back)
                {
                     IStatePreservation preserver = element.DataContext as IStatePreservation;
                     if (preserver != null)
                     {
                         preserver.ClearState(IsolatedStorageSettings.ApplicationSettings, PhoneApplicationService.Current.State);
                     }
                }
            }
        }

        static void OnNavigated(object sender, NavigationEventArgs e)
        {
            var element = e.Content as FrameworkElement;
            if (element != null)
            {
                IStatePreservation statePreservation = element.DataContext as IStatePreservation;
                if (statePreservation != null)
                {
                    statePreservation.LoadState(IsolatedStorageSettings.ApplicationSettings, PhoneApplicationService.Current.State, shouldLoadTransientState);
                }
            }
        }
    }
}
