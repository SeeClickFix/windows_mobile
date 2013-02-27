using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace SeeClickFix.WP8.Behaviors
{
    public class MapCenterBehavior : Behavior<Map>
    {
        public GeoCoordinate Center
        {
            get { return (GeoCoordinate)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public static readonly System.Windows.DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(GeoCoordinate), typeof(MapCenterBehavior), new PropertyMetadata(new PropertyChangedCallback(OnCenterChanged)));

        public static void OnCenterChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as MapCenterBehavior).AssociatedObject.Center = (GeoCoordinate)e.NewValue;
        }

        public MapCenterBehavior()
        {
            
        }
    }
}
