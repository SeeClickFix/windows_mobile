using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Shell;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.Infrastructure;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using Windows.Devices.Geolocation;

namespace SeeClickFix.WP8.ViewModel
{
    public class IssueListViewModel : BaseViewModelState
    {
        ListIssuesQuery query = new ListIssuesQuery();
        ObservableCollection<Issue> issues = new ObservableCollection<Issue>();

        public ReadOnlyObservableCollection<Issue> Issues { get; private set; }

        Location location;
        public Location Location
        {
            get { return this.location; }
            set
            {
                this.location = value;
                this.RaisePropertyChanged("Location");
            }
        }

        bool hasGettingIssuesError = false;
        public bool HasGettingIssuesError
        {
            get { return this.hasGettingIssuesError; }
            private set
            {
                this.hasGettingIssuesError = value;
                this.RaisePropertyChanged("HasGettingIssuesError");
            }
        }

        bool hasLocationServicesError = false;
        public bool HasLocationServicesError
        {
            get { return this.hasLocationServicesError; }
            private set
            {
                this.hasLocationServicesError = value;
                this.RaisePropertyChanged("HasLocationServicesError");
            }
        }

        bool isLocationServicesNotAgreed = false;
        public bool IsLocationServicesNotAgreed
        {
            get { return this.isLocationServicesNotAgreed; }
            private set
            {
                this.isLocationServicesNotAgreed = value;
                this.RaisePropertyChanged("IsLocationServicesNotAgreed");
            }
        }

        Issue selectedIssue;
        [Stateful(ApplicationStateType.Transient)]
        public Issue SelectedIssue
        {
            get
            {
                return this.selectedIssue;
            }
            set
            {
                this.selectedIssue = value;
                //PhoneApplicationService.Current.State["SelectedIssue"] = this.SelectedIssue;
            }
        }

        SearchFilter searchFilter;
        [Stateful(ApplicationStateType.Transient)]
        public SearchFilter SearchFilter
        {
            get { return this.searchFilter; }
            set
            {
                if (this.searchFilter == null || !this.searchFilter.Equals(value))
                {
                    this.searchFilter = value;
                    this.ResetIssues();
                    //PhoneApplicationService.Current.State["SearchFilter"] = this.SearchFilter;
                }
            }
        }

        //List<IssueStatus> statusFilter;
        //public IEnumerable<IssueStatus> StatusFilter 
        //{
        //    get { return this.statusFilter; }
        //    set
        //    {
        //        var newValues = value.Except(this.statusFilter).ToList();
        //        var removeValues = this.statusFilter.Except(value).ToList();

        //        foreach (IssueStatus status in newValues)
        //        {
        //            this.statusFilter.Add(status);
        //        }

        //        foreach (IssueStatus status in removeValues)
        //        {
        //            this.statusFilter.Remove(status);
        //        }

        //        if (newValues.Count() > 0 || removeValues.Count() > 0)
        //        {
        //            this.ResetIssues();
        //        }
        //    }
        //}
        //public string KeywordFilter { get; set; }

        public RelayCommand LoadMoreIssuesCommand { get; private set; }
        public RelayCommand ReportIssueCommand { get; private set; }
        public RelayCommand FilterIssuesCommand { get; private set; }
        public RelayCommand<Issue> ShowIssueDetailsCommand { get; private set; }
        public RelayCommand RefreshIssuesCommand { get; private set; }
        public RelayCommand ShowIssuesOnMapCommand { get; private set; }
        public RelayCommand SetLocationCommand { get; private set; }

        SelectLocationViewModel selectLocationViewModel = new SelectLocationViewModel();

        async public void SetLocation(GeoCoordinate coordinate)
        {
            if (coordinate != null && (query.Coordinate == null || !query.Coordinate.Equals(coordinate)))
            {
                query.Coordinate = coordinate;

                this.IsBusy = true;
                this.Location = await this.GetLocation(coordinate);
                AppSettingsService.Inst.Settings.Location = this.Location;
                AppSettingsService.Inst.Save();
                this.IsBusy = false;

                this.HasLocationServicesError = false;
                this.IsLocationServicesNotAgreed = false;

                this.ResetIssues();
            }
        }

        public IssueListViewModel()
        {
            //this.RegisterStatefulProperty(ApplicationStateType.Transient, () => this.SearchFilter);
            //this.RegisterStatefulProperty(ApplicationStateType.Transient, () => this.SelectedIssue);

            //object selectedIssue;
            //if (PhoneApplicationService.Current.State.TryGetValue("SelectedIssue", out selectedIssue))
            //{
            //this.SelectedIssue = (Issue)selectedIssue;
            //}

            //object issues;
            //if (PhoneApplicationService.Current.State.TryGetValue("Issues", out issues))
            //{
            //    this.issues = new ObservableCollection<Issue>((IEnumerable<Issue>)issues);
            //}

            this.Issues = new ReadOnlyObservableCollection<Issue>(this.issues);
            this.LoadMoreIssuesCommand = new RelayCommand(this.LoadMoreIssues);
            this.ReportIssueCommand = new RelayCommand(this.ReportIssue);
            this.FilterIssuesCommand = new RelayCommand(this.FilterIssues, () => !this.IsBusy && this.Issues.Count() > 0); //, () => { return !this.IsBusy; });
            this.ShowIssueDetailsCommand = new RelayCommand<Issue>(this.ShowIssueDetails);
            this.RefreshIssuesCommand = new RelayCommand(this.RefreshIssues, () => !this.IsBusy);
            this.ShowIssuesOnMapCommand = new RelayCommand(this.ShowIssuesOnMap, () => !this.IsBusy);
            this.SetLocationCommand = new RelayCommand(this.SetLocation);

            this.searchFilter = new SearchFilter()
            {
                Status = new IssueStatus[] { IssueStatus.Open }
            };

            /*new System.Device.Location.GeoCoordinate()
            {
                Latitude = 41.31037258994274,
                Longitude = -72.92415951148533
            };*/

            this.LoadState();
            if (query.Coordinate == null)
            {
                this.LoadLocation();
            }

            Messenger.Default.Register<MessageBase>(this, Messages.RefreshIssues, (m) =>
            {
                this.ResetIssues();
            });

            Messenger.Default.Register<int>(this, Messages.RefreshIssue, (itemType) =>
            {
                this.ResetIssues();
            });

            this.SubcribeToUserProfileChanges();
        }

        async Task<Location> GetLocation(GeoCoordinate position)
        {
            string placeName = null;
            string logoUrl = null;
            var watchArea = await this.SCFDataService.ListWatchAreas(query.Coordinate);
            if (watchArea != null)
            {
                if (watchArea.Id != null && watchArea.Areas.Count() > 0)
                {
                    var area = watchArea.Areas[0];
                    placeName = area.Title;
                    logoUrl = area.LogoPath;
                }
                else if (watchArea.Place != null && watchArea.Place.Id != 0)
                {
                    placeName = watchArea.Place.NameAndState;
                }
                else if (watchArea.Geocode != null)
                {
                    placeName = watchArea.Geocode.City;
                }
            }

            Location location = new Location()
            {
                LogoUrl = logoUrl,
                Name = placeName,
                Position = Coordinate.FromGeoCoordinate(position)
            };
            return location;
        }

        async void LoadLocation()
        {
            this.HasLocationServicesError = false;
            this.IsLocationServicesNotAgreed = false;

            // load coordinates from settings, if any
            if (AppSettingsService.Inst.Settings.Location != null)
            {
                query.Coordinate = AppSettingsService.Inst.Settings.Location.Position.ToGeoCoordinate();
                this.Location = AppSettingsService.Inst.Settings.Location;
            }
            else if (AppSettingsService.Inst.Settings.UseLocationServices)
            {
                // use location services
                this.IsBusy = true;
                var r = await GeoCoordinateWatcherUtil.GetCoordinateAsync();
                if (r.Status == GeoPositionStatus.Ready)
                {
                    query.Coordinate = r.Position;

                    //var location = new Location() 
                    //{
                    //    Position = new Coordinate()
                    //    {
                    //        Latitude = query.Coordinate.Latitude,
                    //        Longitude = query.Coordinate.Longitude
                    //    }
                    //};

                    //AppSettingsService.Inst.Settings.Location = location;
                    //AppSettingsService.Inst.Save();

                    //var wa = await GetLocation(r.Position);

                    this.Location = await this.GetLocation(r.Position);
                    AppSettingsService.Inst.Settings.Location = this.Location;
                    AppSettingsService.Inst.Save();

                    this.IsBusy = false;
                }
                else
                {
                    this.IsBusy = false;
                    // location services disabled or other error
                    // user should setup his location
                    this.HasLocationServicesError = true;
                }
            }
            else
            {
                // user didn't give his consent
                this.IsLocationServicesNotAgreed = true;
            }
            this.ResetIssues();
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {

        }

        void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
        }

        //public override void LoadState(IDictionary<string, object> persistentStateDictionary, IDictionary<string, object> transientStateDictionary, bool shouldLoadTransientState)
        //{
        //    if (this.IsLoaded)
        //    {
        //        return;
        //    }

        //    base.LoadState(persistentStateDictionary, transientStateDictionary, shouldLoadTransientState);
        //}

        private void ShowIssuesOnMap()
        {

        }

        public override void Cleanup()
        {
            base.Cleanup();
            Messenger.Default.Unregister(this);
        }

        void RefreshIssues()
        {
            if (this.query.Coordinate == null)
            {
                this.LoadLocation();
            }
            else
            {
                this.ResetIssues();
            }
        }

        void LoadMoreIssues()
        {
            if (this.IsBusy)
            {
                return;
            }

            if (this.Issues.Count > 0)
            {
                this.query.Page++;
            }

            this.LoadIssues();
        }

        async void LoadIssues()
        {
            if (this.IsBusy )
            {
                return;
            }

            if(this.query.Coordinate == null)
            {
                return;
            }

            this.IsBusy = true;
            this.query.Status = this.SearchFilter.Status;
            this.query.Keyword = this.SearchFilter.Keyword;
            var svc = SimpleIoc.Default.GetInstance<ISCFDataService>();
            var issues = await svc.ListIssuesByAddressAsync(this.query, UserProfileService.Inst.UserProfile.Email);
            if (issues != null)
            {
                foreach (Issue issue in issues)
                {
                    this.issues.Add(issue);
                }
            }
            else
            {
                MessageBox.Show("We couldn't get any nearby issues.\nMake sure your internet connection is available.", "SeeClickFix", MessageBoxButton.OK);
            }
            this.IsBusy = false;
        }

        void ResetIssues()
        {
            this.query.Page = 1;
            this.issues.Clear();
            this.LoadIssues();
        }

        void ReportIssue()
        {
            this.NavigationService.NavigateTo(Constants.ReportIssuePageUri);
        }

        void FilterIssues()
        {
            this.NavigationService.NavigateTo(Constants.FilterIssuesPageUri);
        }

        protected override void OnBusyChanged()
        {
            this.FilterIssuesCommand.RaiseCanExecuteChanged();
            this.RefreshIssuesCommand.RaiseCanExecuteChanged();
            this.ShowIssuesOnMapCommand.RaiseCanExecuteChanged();
        }

        void ShowIssueDetails(Issue issue)
        {
            this.SelectedIssue = issue;
            this.NavigationService.NavigateTo(Constants.ShowIssueDetailsPageUri);
            // var viewModel = SimpleIoc.Default.GetInstance<IssueDetailsViewModel>();
        }

        protected override void OnLoginProfileChanged()
        {
            this.ResetIssues();
        }

        void SetLocation()
        {
            // this.NavigationService.NavigateTo(Constants.SelectLocationPageUri);
            GeoCoordinate c = new GeoCoordinate();
            if (this.query.Coordinate != null)
            {
                c = query.Coordinate;
            }

            this.NavigationService.NavigateTo(
                new Uri(string.Format("{0}?lat={1}&lng={2}&isSelectingReportLocation={3}",
                    Constants.SelectLocationPageUri,
                    c.Latitude,
                    c.Longitude,
                    false), UriKind.RelativeOrAbsolute));
        }
    }

    public static class IssueListComparison
    {
        public static bool Compare(IssueStatus[] thisArray, IssueStatus[] array)
        {
            if (thisArray == null || array == null)
            {
                return thisArray == array;
            }

            if (thisArray.Count() != array.Count())
            {
                return false;
            }

            var newValues = array.Except(thisArray).ToList();
            var removeValues = thisArray.Except(array).ToList();

            return newValues.Count() == 0 && removeValues.Count() == 0;
        }
    }

    public class SearchFilter
    {
        public SearchFilter()
        {
        }

        [DataMember]
        string keyword;
        public string Keyword
        {
            get { return this.keyword; }
            set
            {
                if (value != null)
                {
                    this.keyword = value.Trim();
                }
                else
                {
                    this.keyword = value;
                }
            }
        }

        [DataMember]
        public IssueStatus[] Status
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            SearchFilter filter = obj as SearchFilter;
            if (filter == null)
            {
                return false;
            }

            string k1 = NullIfEmpty(this.keyword);
            string k2 = NullIfEmpty(filter.Keyword);

            if (k1 == null || k2 == null)
            {
                if (k1 != k2)
                {
                    return false;
                }

            }
            else if (string.Compare(k1.Trim(), k2.Trim(), StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                return false;
            }

            if (this.Status == null)
            {
                return this.Status == filter.Status;
            }
            else
            {
                return IssueListComparison.Compare(this.Status, filter.Status);
            }
        }

        public override int GetHashCode()
        {
            return -1;
        }

        static string NullIfEmpty(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                s = null;
            }
            return s;
        }
    }
}
