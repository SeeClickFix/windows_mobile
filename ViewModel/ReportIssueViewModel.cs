using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Info;
using Microsoft.Phone.Tasks;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.Infrastructure;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeClickFix.WP8.ViewModel
{
    public class ReportIssueViewModel : BaseViewModelState
    {
        int? enhanced_watch_area_id = null;
        string photoFileName;

        //IEnumerable requestTypes;
        //public IEnumerable RequestTypes
        //{
        //    get
        //    {
        //        return this.requestTypes;
        //    }
        //    private set
        //    {
        //        this.requestTypes = value;
        //        this.RaisePropertyChanged("RequestTypes");
        //    }
        //}

        //RequestTypeViewModel selectedRequestType;
        //public RequestTypeViewModel SelectedRequestType
        //{
        //    get { return this.selectedRequestType; }
        //    set
        //    {
        //        if (this.selectedRequestType != value)
        //        {
        //            this.selectedRequestType = value;
        //            this.RaisePropertyChanged("SelectedRequestType");
        //            this.GetServiceRequestTypeQuestions();
        //        }
        //    }
        //}

        string userDisplayName;
        [Stateful(ApplicationStateType.Transient)]
        public string UserDisplayName
        {
            get { return this.userDisplayName; }
            set
            {
                if (this.userDisplayName != value)
                {
                    this.userDisplayName = value;
                    this.RaisePropertyChanged("UserDisplayName");
                    this.SubmitIssueCommand.RaiseCanExecuteChanged();
                }
            }
        }

        string userEmail;
        [Stateful(ApplicationStateType.Transient)]
        public string UserEmail
        {
            get { return this.userEmail; }
            set
            {
                if (this.userEmail != value)
                {
                    this.userEmail = value;
                    this.RaisePropertyChanged("UserEmail");
                    this.SubmitIssueCommand.RaiseCanExecuteChanged();
                }
            }
        }

        //string address;
        //public string Address
        //{
        //    get { return this.address; }
        //    private set
        //    {
        //        this.address = value;
        //        this.RaisePropertyChanged("Address");
        //    }
        //}

        //public GeoCoordinate Location
        //{
        //    get;
        //    set;
        //}

        string title;
        [Stateful(ApplicationStateType.Transient)]
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }

        string description;
        [Stateful(ApplicationStateType.Transient)]
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }

        Stream photo;
        public Stream Photo
        {
            get { return this.photo; }
            set
            {
                this.photo = value;
                this.RaisePropertyChanged("Photo");
            }
        }

        public SelectLocationViewModel SelectLocation
        {
            get;
            private set;
        }

        IEnumerable<RequestTypeQuestion> requestTypeQuestions;
        public IEnumerable<RequestTypeQuestion> RequestTypeQuestions
        {
            get { return this.requestTypeQuestions; }
            private set
            {
                if (this.requestTypeQuestions != value)
                {
                    this.requestTypeQuestions = value;
                    this.RaisePropertyChanged("RequestTypeQuestions");
                }
            }
        }

        public RelayCommand SubmitIssueCommand { get; private set; }
        public RelayCommand AttachCommand { get; private set; }
        public RelayCommand SetLocationCommand { get; private set; }     

        public ReportIssueViewModel()
        {
            this.SubmitIssueCommand = new RelayCommand(this.SubmitIsssue);
            this.AttachCommand = new RelayCommand(this.Attach);
            this.SetLocationCommand = new RelayCommand(this.SetlLocation);

            // lat=38.904977122236154&lng=-77.01596796512604
            //this.coordinate = new GeoCoordinate()
            //{
            //    Latitude = 48.89364,// 41.31037258994274,
            //    Longitude = 2.33739 // - 72.92415951148533
            //};

            this.enhanced_watch_area_id = 50;

            this.SelectLocation = new SelectLocationViewModel();
            this.SelectLocation.TrackMeCommand.Execute(null);

            //this.Location = new GeoCoordinate()
            //{
            //    Latitude = 38.904977122236154,// 41.31037258994274,
            //    Longitude = -77.01596796512604 // - 72.92415951148533
            //};

            this.RefreshUserProfile();
            this.SubcribeToUserProfileChanges();
             // this.GetServiceRequestTypes();
        }

        bool CanSubmitIssue()
        {
            bool canSubmitIssue = true;
            //bool hasCategories = false;

            // if categories exist, then user must select a category
            //if (this.RequestTypes != null && (this.RequestTypes as IList).Count > 0)
            //{
            //    hasCategories = true;
                
            //    // a valid category item is the one which is not the dummy item 'select item'
            //    canSubmitIssue = !this.SelectedRequestType.IsDummyItem();
            //    if (!canSubmitIssue)
            //    {
            //        MessageBox.Show("Select a category for the issue");
            //    }
            //    else
            //    {
            //        // check questions
            //    }
            //}

            //if(canSubmitIssue)
            //{
            //    // title is required in the case there are no categories or if the selected category is not 'Other'
            //    // if it's 'Other' then it should have title set
            //    if ((!hasCategories || 
            //        this.SelectedRequestType.IsOther ||
            //        this.SelectedRequestType.IsDefaultOther) && 
            //        string.IsNullOrWhiteSpace(this.Title))
            //    {
            //        MessageBox.Show("Write a title for the issue");
            //        canSubmitIssue = false;
            //    }
            //}

            if (canSubmitIssue && !EmailValidator.IsValid(this.UserEmail))
            {
                MessageBox.Show("Enter a valid email address");
                canSubmitIssue = false;
            }

            if (canSubmitIssue && string.IsNullOrWhiteSpace(this.Title))
            {
                MessageBox.Show("Enter a title for the issue");
                canSubmitIssue = false;
            }

            if (canSubmitIssue && (this.SelectLocation.GeoLocation == null || this.SelectLocation.GeoLocation.IsUnknown || string.IsNullOrWhiteSpace(this.SelectLocation.Address)))
            {
                MessageBox.Show("Select a location for the issue");
                canSubmitIssue = false;
            }

            return canSubmitIssue;
        }

        async void SubmitIsssue()
        {
            if (!this.CanSubmitIssue())
            {
                return;
            }

            this.IsBusy = true;
            AddIssueQuery query = new AddIssueQuery()
            {
                //Summary = string.IsNullOrWhiteSpace(this.Title) ?  : ,
                Summary = this.Title,
                Location = this.SelectLocation.GeoLocation,
                Address = this.SelectLocation.Address,

                // optional
                Description = this.Description,
                ReporterEmail = this.UserEmail,
                ReporterDisplay = this.UserDisplayName,
                Photo = this.Photo,
                PhotoName = this.photoFileName
                //DeviceOs = DeviceExtendedPropertiesUtils.GetDeviceOS(),
                //DeviceId = DeviceExtendedPropertiesUtils.GetDeviceUniqueId(),
                //DeviceName = DeviceStatus.DeviceName
            };
            var response = await this.SCFDataService.AddIssue(query);
            if (response.IssueId == null)
            {
                MessageBox.Show("There was a problem submitting your issue");
            }
            else
            {
                Messenger.Default.Send<MessageBase>(new MessageBase(), Messages.RefreshIssues);
                this.NavigationService.GoBack();
            }
            
            this.IsBusy = false;
        }

        async void Attach()
        {
            var photoChooserTask = new PhotoChooserTask()
            {
                ShowCamera = true
            };

            var result = await photoChooserTask.ShowAsync();
            if (result.TaskResult == TaskResult.OK)
            {
                this.Photo = result.ChosenPhoto;
                this.photoFileName = Path.GetFileName(result.OriginalFileName);
            }
        }

        //async void GetServiceRequestTypes()
        //{
        //    if (this.enhanced_watch_area_id != null)
        //    {
        //        this.IsBusy = true;
        //        var requestTypes = await this.SCFDataService.ListServiceRequestTypes(this.coordinate);
        //        if (requestTypes != null)
        //        {
        //            var requestTypeVMList = requestTypes.OrderBy(rt => rt.Position)
        //                .Select(rt => new RequestTypeViewModel(rt))
        //                .ToList();
                    
        //            if (requestTypes.Count() > 0)
        //            {
        //                // insert the 'Select item' item 
        //                requestTypeVMList.Insert(0, new RequestTypeViewModel());

        //                // if the returned categories do not have Other, include it
        //                if (!requestTypes.Any(rt => rt.IsOther))
        //                {
        //                    // it's important item is 2nd
        //                    // in the expanded-mode in ListPicker, it will be 2nd after 'Select item'
        //                    // and it will be first in the full mode where 'Select item' item is hidden
        //                    requestTypeVMList.Insert(1, new RequestTypeViewModel(null, true));
        //                }
        //                else
        //                {
        //                    // in the case when it has only 'Other', don't display categories
        //                }
        //            }

        //            this.RequestTypes = requestTypeVMList.ToList();
        //        }
        //        else
        //        {
        //            this.RequestTypes = new List<RequestTypeViewModel>();
        //        }
        //        this.IsBusy = false;
        //    }
        //}

        //async void GetServiceRequestTypeQuestions()
        //{
        //    if (this.SelectedRequestType.RequestType == null)
        //    {
        //        return;
        //    }

        //    this.IsBusy = true;
        //    this.RequestTypeQuestions = await this.SCFDataService.ListRequestTypeQuestions(this.SelectedRequestType.RequestType.Id);
        //    this.IsBusy = false;
        //}

        protected override void OnBusyChanged()
        {
            this.CanExecuteCommandsChanged();
        }

        void CanExecuteCommandsChanged()
        {
            this.SubmitIssueCommand.RaiseCanExecuteChanged();
        }

        protected override void OnLoginProfileChanged()
        {
            this.RefreshUserProfile();
        }

        void RefreshUserProfile()
        {
            this.UserEmail = this.UserProfileService.UserProfile.Email;
            this.UserDisplayName = this.UserProfileService.UserProfile.Name;
        }

        void SetlLocation()
        {
            this.NavigationService.NavigateTo(
                new Uri(string.Format("{0}?lat={1}&lng={2}&isSelectingReportLocation={3}", 
                    Constants.SelectLocationPageUri, 
                    this.SelectLocation.GeoLocation.Latitude, 
                    this.SelectLocation.GeoLocation.Longitude,
                    true), UriKind.RelativeOrAbsolute));

        }
    }

    public class RequestTypeViewModel
    {
        public RequestTypeViewModel(RequestType rt = null, bool isDefaultOther = false)
        {
            this.RequestType = rt;
            this.IsDefaultOther = isDefaultOther;
        }

        public RequestType RequestType
        {
            get;
            private set;
        }

        // the 'Select item' is the item which has RequestType null and it's not the 'Other' added automatically
        public bool IsDummyItem()
        {
            return this.RequestType == null && !this.IsDefaultOther;
        }

        public bool IsOther
        {
            get
            {
                return this.RequestType != null && this.RequestType.IsOther;
            }
        }

        // the 'Other' item automatically added 
        public bool IsDefaultOther
        {
            get;
            private set;
        }

        public string DisplayTitle
        {
            get
            {
                if (this.IsDefaultOther)
                {
                    return "Other";
                }
                else if (this.RequestType == null)
                {
                    return "Select a category";
                }
                else
                {
                    return this.RequestType.Title;
                }
            }
        }
    }

    //public class RequestQuestionViewModel : BaseViewModel
    //{
    //    public RequestQuestionViewModel(RequestTypeQuestion question)
    //    {

    //    }

    //    public RequestTypeQuestion Question
    //    {
    //    }

    //    public string Title { get;
    //}
}
