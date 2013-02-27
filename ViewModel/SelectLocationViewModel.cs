using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.Services;
using SeeClickFix.WP8.Services.Impl;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SeeClickFix.WP8.ViewModel
{
    public class SelectLocationViewModel : BaseViewModel
    {
        SCFDataService seeClickFixSvc;
        bool wasTracked = false;
        bool isLocationValid = false;

        double zoomLevel = 16;
        public double ZoomLevel
        {
            get
            {
                return zoomLevel;
            }

            set
            {
                if (this.zoomLevel != value)
                {
                    this.zoomLevel = value;
                    this.RaisePropertyChanged("ZoomLevel");
                }
            }
        }

        public bool IsSelectingReportLocation
        {
            get;
            set;
        }

        string address;
        public string Address
        {
            get { return this.address; }
            set
            {
                if (this.address != value)
                {
                    this.address = value;
                    this.RaisePropertyChanged("Address");
                    this.UpdadateGeoCoordinateByAddress();
                }
            }
        }

        GeoCoordinate geoLocation;
        public GeoCoordinate GeoLocation
        {
            get { return this.geoLocation; }
            set
            {
                //++coordinateSetCount;
                //if (coordinateSetCount > 0)
                //{
                // if (!this.geoLocation.IsUnknown || //  ignore when the map control tries to set the coordinates automatically on startup or before we finished tracking
                if (this.geoLocation == null ||
                    value == null ||
                    this.geoLocation.Latitude != value.Latitude ||
                    this.geoLocation.Longitude != value.Longitude)
                {
                    //bool wasUnknown = this.geoLocation.IsUnknown;

                    this.geoLocation = value;
                    this.SaveCommand.RaiseCanExecuteChanged();
                    this.RaisePropertyChanged("GeoLocation");

                    if (!this.IsBusy)
                    {
                        if (this.wasTracked)
                        {
                            this.UpdateAddressByGeoCoordinate();
                        }
                        else
                        {
                            this.wasTracked = true;
                        }
                    }
                }
                //}
            }
        }

        public ICommand TrackMeCommand { get; private set; }
        public ICommand SetLocationCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public SelectLocationViewModel()
        {
            this.seeClickFixSvc = new SCFDataService();
            this.TrackMeCommand = new RelayCommand(this.TrackMe);
            this.SetLocationCommand = new RelayCommand<GeoCoordinate>(this.SetLocation);
            this.SaveCommand = new RelayCommand(this.Save, () => !this.GeoLocation.IsUnknown && !this.GeoLocation.Equals(new GeoCoordinate(0, 0)));

            // need default for the map control, otherwise it would crash
            this.geoLocation = new GeoCoordinate();

            // this.TrackMe();

            //if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            //{
            //    // User has opted in or out of Location
            //    return;
            //}
            //else
            //{
            //    MessageBoxResult result =
            //        MessageBox.Show("This app accesses your phone's location. Is that ok?",
            //        "Location",
            //        MessageBoxButton.OKCancel);

            //    if (result == MessageBoxResult.OK)
            //    {
            //        IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
            //    }
            //    else
            //    {
            //        IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
            //    }

            //    IsolatedStorageSettings.ApplicationSettings.Save();
            //}
        }

        void SetLocation(GeoCoordinate coordinate)
        {
            this.GeoLocation = coordinate;
        }

        async void TrackMe()
        {
            if (!AppSettingsService.Inst.Settings.UseLocationServices)
            {
                return;
            }

            this.IsBusy = true;
            var r = await GeoCoordinateWatcherUtil.GetCoordinateAsync();
            this.IsBusy = false;
            if (r.Status == GeoPositionStatus.Ready)
            {
                this.wasTracked = true;
                this.GeoLocation = r.Position;
            }
            else
            {
                // error
            }
            //GeoLocatorService geoLocator = new GeoLocatorService();
            //var currentPosResponse = await geoLocator.GetCurrentPositionAsync();
            //this.IsBusy = false;
            //if (currentPosResponse.Error == null)
            //{
            //    this.wasTracked = true;
            //    this.GeoLocation = currentPosResponse.Coordinate;
            //}
            //else
            //{
            //    if (currentPosResponse.IsLocationServicesDisabled)
            //    {
            //        MessageBox.Show("Location services are disabled");
            //    }
            //    // couldnt obtain the device location
            //    // location turned off or something else happened

            //    // error
            //    // TODO: handle error; handle case when location services is turned off
            //}
        }

        async void UpdateAddressByGeoCoordinate()
        {
            if (this.GeoLocation == null)
            {
                return;
            }

            // Map control is automatically setting a value when opened
            // this one is Congo with Lattitue == 0
            if (this.GeoLocation.IsUnknown || (this.GeoLocation.Latitude == 0 || this.GeoLocation.Longitude == 0))
            {
                this.address = string.Empty;
                this.RaisePropertyChanged("Address");
                return;
            }

            try
            {
                //if (this.lastLocation == null ||
                //    this.lastLocation.Latitude != this.GeoLocation.Latitude ||
                //    this.lastLocation.Longitude != this.GeoLocation.Longitude)
                //{
                //this.lastLocation = this.GeoLocation;
                this.IsBusy = true;
                var address = await this.seeClickFixSvc.GeoCoordinateToAddress(this.GeoLocation);
                this.address = address.FullAddress;
                this.RaisePropertyChanged("Address");
                this.IsBusy = false;
                //}
            }
            catch (Exception ex)
            {
                // TODO: handle error
                this.address = string.Empty;
                this.RaisePropertyChanged("Address");
                this.IsBusy = false;
            }
        }

        async void UpdadateGeoCoordinateByAddress()
        {
            if (string.IsNullOrWhiteSpace(this.Address))
            {
                return;
            }

            try
            {
                this.IsBusy = true;
                var geoCoordinate = await this.seeClickFixSvc.AddressToGeoCoordinateAsync(this.Address);
                this.geoLocation = geoCoordinate;
                this.RaisePropertyChanged("GeoLocation");
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                // TODO: handle error
            }
        }

        void Save()
        {
            if (this.IsSelectingReportLocation)
            {
                SimpleIoc.Default.GetInstance<ReportIssueViewModel>().SelectLocation.GeoLocation = this.GeoLocation;
                // SimpleIoc.Default.GetInstance<MainViewModel>().IssueList.SetLocation();
            }
            else
            {
                SimpleIoc.Default.GetInstance<MainViewModel>().IssueList.SetLocation(this.GeoLocation);
            }

            this.NavigationService.GoBack();
        }

        protected override void OnBusyChanged()
        {
            this.SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
