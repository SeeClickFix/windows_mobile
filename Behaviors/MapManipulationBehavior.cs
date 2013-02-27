using GalaSoft.MvvmLight;
using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows;

namespace SeeClickFix.WP8.Behaviors
{
    public class MapManipulationBehavior : Behavior<Map>
    {
        public event EventHandler<GeoCoordinate> ManipulationEnd;
        DispatcherTimer dt = new DispatcherTimer();

        public ICommand SetLocationCommand
        {
            get { return (ICommand)GetValue(SetLocationCommandProperty); }
            set { SetValue(SetLocationCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SetLocationCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetLocationCommandProperty =
            DependencyProperty.Register("SetLocationCommand", typeof(ICommand), typeof(MapManipulationBehavior), new PropertyMetadata(0));
                

        public MapManipulationBehavior()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                this.AssociatedObject.CenterChanged += AssociatedObject_CenterChanged;
            }
        }

        void StartTimer()
        {
            this.dt = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            this.dt.Tick += dt_Tick;
            this.dt.Start();
        }

        void StopTimer()
        {
            if (this.dt != null)
            {
                this.dt.Stop();
                this.dt = null;
            }
        }

        void dt_Tick(object sender, EventArgs e)
        {
            this.StopTimer();
            if (this.ManipulationEnd != null)
            {
                this.ManipulationEnd(this.AssociatedObject, this.AssociatedObject.Center);
            }
        }

        void AssociatedObject_CenterChanged(object sender, MapCenterChangedEventArgs e)
        {
            if (dt != null)
            {
                this.StopTimer();
            }
            
            this.StartTimer();
        }
    }
}
