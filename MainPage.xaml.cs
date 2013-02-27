using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using SeeClickFix.WP8.ViewModel;
using System.Windows.Media.Animation;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;

namespace SeeClickFix.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //async protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    var p = await this.GetPosition();
        //}

        //async Task<Geoposition> GetPosition()
        //{
        //    Geolocator geolocator = new Geolocator();
        //    geolocator.DesiredAccuracyInMeters = 50;

        //    Geoposition geoposition = null;
        //    try
        //    {
        //        geoposition = await geolocator.GetGeopositionAsync(
        //            maximumAge: TimeSpan.FromMinutes(5),
        //            timeout: TimeSpan.FromSeconds(10)
        //            );
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return geoposition;
        //}

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.NavigationService.RemoveBackEntry();
            base.OnNavigatedTo(e);
        }

        private void Panorama_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            double targetOpacity = 0.33;
            if (this.Panorama.SelectedIndex == 0)
            {
                targetOpacity = 0.12;
            }
            else if (this.Panorama.SelectedIndex == 1)
            {
                targetOpacity = 0.22;
            }
            
            var s = this.Resources["StoryboardAnimateBkgndOpacity"] as Storyboard;
            var a = s.Children[0] as DoubleAnimation;
            a.To = targetOpacity;
            s.Begin();

            //switch (this.Panorama.SelectedIndex)
            //{
            //    case 0:
            //        this.ImgBrush.Opacity = 0.12;
            //        break;
            //    default:
            //        this.ImgBrush.Opacity = 0.33;
            //        break;
            //}
            
        }
    }
}