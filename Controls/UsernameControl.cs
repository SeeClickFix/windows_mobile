using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
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

namespace SeeClickFix.WP8.Controls
{
    public class UsernameControl : DataTemplateSelector
    {
        static DataTemplate AnonymousTemplate = (DataTemplate)App.Current.Resources["UsernameAnonymousTemplate"];
        static DataTemplate GuestTemplate = (DataTemplate)App.Current.Resources["UsernameGuestTemplate"];
        static DataTemplate UserTemplate = (DataTemplate)App.Current.Resources["UsernameTemplate"];

        public ICommand ShowUserProfileCommand { get; private set; }

        public string Username { get; private set; }

        public UsernameControl()
        {
            this.ShowUserProfileCommand = new RelayCommand(this.ShowUserProfile);
        }
        
        void ShowUserProfile()
        {
            int? userId = null;
            IssueHistoryItem item = this.Content as IssueHistoryItem;
            if (item != null && item.User != null)
            {
                userId = item.User.Id;
            }
            else if (this.Content is Issue)
            {
                userId = (this.Content as Issue).UserId;
            }

            if (userId != null)
            {
                SimpleIoc.Default.GetInstance<INavigationService>().NavigateTo(new Uri(string.Format("{0}?userId={1}", Constants.UserProfilePageUri, userId.Value), UriKind.Relative));
            }
        }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            DataTemplate template;
            if (item is IssueHistoryItem)
            {
                IssueHistoryItem comment = (IssueHistoryItem)item;
                if (!string.IsNullOrWhiteSpace(comment.Name))
                {
                    this.Username = comment.Name;
                    if (comment.User != null && comment.User.Id != 0)
                    {
                        template = UserTemplate;
                    }
                    else
                    {
                        template = GuestTemplate;
                    }
                }
                else
                {
                    template = AnonymousTemplate;
                }
            }
            else if (item is Issue)
            {
                Issue issue = (Issue)item;
                if (issue.UserId != null && issue.UserId != 0)
                {
                    this.Username = issue.Reporter;
                    template = UserTemplate;
                }
                else if (!string.IsNullOrWhiteSpace(issue.Reporter))
                {
                    this.Username = issue.Reporter;
                    template = GuestTemplate;
                }
                else
                {
                    template = AnonymousTemplate;
                }
            }
            else
            {
                template = null;
            }
            return template;
        }
    }
}
