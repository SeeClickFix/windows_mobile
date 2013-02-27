using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SeeClickFix.WP8.Services.Impl
{
    public class NavigationService : INavigationService
    {
        private PhoneApplicationFrame mainFrame;
        public event NavigatingCancelEventHandler Navigating;

        public void NavigateTo(Uri pageUri)
        {
            if (this.EnsureMainFrame())
            {
                this.mainFrame.Navigate(pageUri);
            }
        }
        public void GoBack()
        {
            if (this.EnsureMainFrame() && this.mainFrame.CanGoBack)
            {
                this.mainFrame.GoBack();
            }
        }

        private bool EnsureMainFrame()
        {
            if (this.mainFrame != null)
            {
                return true;
            }

            this.mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (this.mainFrame != null)
            {
                // Could be null if the app runs inside a design tool
                this.mainFrame.Navigating += (s, e) =>
                {
                    if (this.Navigating != null)
                    {
                        this.Navigating(s, e);
                    }
                };
                return true;
            }
            return false;
        }
    }
}
