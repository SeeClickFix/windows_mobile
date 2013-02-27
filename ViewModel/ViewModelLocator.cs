using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using SeeClickFix.WP8.Services;
using SeeClickFix.WP8.Services.Impl;
using System.Windows;

namespace SeeClickFix.WP8.ViewModel
{
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                if (!SimpleIoc.Default.IsRegistered<INavigationService>())
                {
                    SimpleIoc.Default.Register<INavigationService, NavigationService>();
                }

                if (!SimpleIoc.Default.IsRegistered<ISCFDataService>())
                {
                    SimpleIoc.Default.Register<ISCFDataService, SCFDataServiceDesign>();
                }
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<INavigationService, NavigationService>();
                SimpleIoc.Default.Register<ISCFDataService, SCFDataService>();
            }

            if (!SimpleIoc.Default.IsRegistered<MainViewModel>())
            {
                SimpleIoc.Default.Register<MainViewModel>();
                // SimpleIoc.Default.Register<MainViewModel>(() => { return new MainViewModel(); });
            }

            //if (!SimpleIoc.Default.IsRegistered<ReportIssueViewModel>())
            //{
            //    SimpleIoc.Default.Register<ReportIssueViewModel>(() => { return new ReportIssueViewModel(); });
            //}

            if (!SimpleIoc.Default.IsRegistered<SelectLocationViewModel>())
            {
                SimpleIoc.Default.Register<SelectLocationViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<IssueListViewModel>())
            {
                SimpleIoc.Default.Register<IssueListViewModel>();
            }

            //if (!SimpleIoc.Default.IsRegistered<IssueDetailsViewModel>())
            //{
            //    SimpleIoc.Default.Register<IssueDetailsViewModel>();
            //}
        }

        public static ViewModelLocator Instance { get { return Application.Current.Resources["Locator"] as ViewModelLocator; } }

        public SelectLocationViewModel SelectLocation
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SelectLocationViewModel>();
            }
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }


        public IssueListViewModel IssueList
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IssueListViewModel>();
            }
        }

        public IssueDetailsViewModel IssueDetails
        {
            get
            {
                return new IssueDetailsViewModel();
            }
        }

        //public IssueCommentsViewModel IssueComments
        //{
        //    get
        //    {
        //        return new IssueCommentsViewModel();
        //    }
        //}

        public ShareIssueViewModel ShareIssue
        {
            get
            {
                return new ShareIssueViewModel();
            }
        }

        public NewCommentViewModel NewComment
        {
            get
            {
                if (!SimpleIoc.Default.IsRegistered<NewCommentViewModel>())
                {
                    SimpleIoc.Default.Register<NewCommentViewModel>();
                }
                return SimpleIoc.Default.GetInstance<NewCommentViewModel>();
            }
        }

        public UserDetailsViewModel UserDetails
        {
            get
            {
                return new UserDetailsViewModel();
            }
        }

        public VoteIssueViewModel VoteIssue
        {
            get
            {
                return new VoteIssueViewModel();
            }
        }

        public FlagIssueViewModel FlagIssue
        {
            get
            {
                return new FlagIssueViewModel();
            }
        }

        //public LoginViewModel Login
        //{
        //    get
        //    {
        //        return new LoginViewModel();
        //    }
        //}

        //public RegisterViewModel Register
        //{
        //    get
        //    {
        //        return new RegisterViewModel();
        //    }
        //}

        public LoginRegisterViewModel LoginRegister
        {
            get
            {
                return new LoginRegisterViewModel();
            }
        }

        public MapViewModel Map
        {
            get
            {
                return new MapViewModel();
            }
        }

        public ReportIssueViewModel ReportIssue
        {
            get
            {
                if (!SimpleIoc.Default.IsRegistered<ReportIssueViewModel>())
                {
                    SimpleIoc.Default.Register<ReportIssueViewModel>();
                }
                return SimpleIoc.Default.GetInstance<ReportIssueViewModel>();
            }
        }

        public MessagesViewModel Messages
        {
            get
            {
                return new MessagesViewModel();
            }
        }

        public void CleanUpReportIssue()
        {
            this.ReportIssue.Cleanup();
            SimpleIoc.Default.Unregister<ReportIssueViewModel>();
        }

        public SettingsViewModel Settings 
        { 
            get 
            { 
                return new SettingsViewModel(); 
            } 
        }

        public void CleanUpNewComment()
        {
            this.NewComment.Cleanup();
            SimpleIoc.Default.Unregister<NewCommentViewModel>();
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}