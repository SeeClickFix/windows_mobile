using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.ViewModel
{
    public class MapViewModel : BaseViewModel
    {
        public RelayCommand GetDirectionsCommand { get; private set; }

        public MapViewModel()
        {
            this.GetDirectionsCommand = new RelayCommand(this.GetDirections);
        }

        void GetDirections()
        {
            var vm = SimpleIoc.Default.GetInstance<MainViewModel>();
            var issue = vm.IssueList.SelectedIssue;

            BingMapsDirectionsTask mapsDirectionsTask = new BingMapsDirectionsTask();
            // If you set the geocoordinate parameter to null, the label parameter is used as a search term.
            LabeledMapLocation spaceNeedleLML = new LabeledMapLocation(issue.Address, issue.GeoCoordinate);
            // If mapsDirectionsTask.Start is not set, the user's current location is used as the start point.
            mapsDirectionsTask.End = spaceNeedleLML;
            mapsDirectionsTask.Show();


            //BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();

            //// You can specify a label and a geocoordinate for the end point.
            //// GeoCoordinate spaceNeedleLocation = new GeoCoordinate(47.6204,-122.3493);
            //// LabeledMapLocation spaceNeedleLML = new LabeledMapLocation("Space Needle", spaceNeedleLocation);

            //// If you set the geocoordinate parameter to null, the label parameter is used as a search term.
            //LabeledMapLocation spaceNeedleLML = new LabeledMapLocation("Space Needle", null);

            //bingMapsDirectionsTask.End = spaceNeedleLML;

            //// If bingMapsDirectionsTask.Start is not set, the user's current location is used as the start point.

            //bingMapsDirectionsTask.Show();
        }
    }
}
