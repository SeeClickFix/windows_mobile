using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace SeeClickFix.WP8.Behaviors
{
    public class StartMediaPlayerLauncherOnTapBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty MediaSourceProperty =
          DependencyProperty.Register("MediaSource", typeof(string), typeof(StartMediaPlayerLauncherOnTapBehavior), new PropertyMetadata(null));

        public string MediaSource
        {
            get { return (string)GetValue(MediaSourceProperty); }
            set { SetValue(MediaSourceProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Tap += AssociatedObject_Tap;
        }

        void AssociatedObject_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask()
            {
                Uri = new Uri(this.MediaSource, UriKind.RelativeOrAbsolute)
            };

            task.Show();

            //MediaPlayerLauncher launcher = new MediaPlayerLauncher()
            //{
            //    Media = new Uri(this.MediaSource, UriKind.RelativeOrAbsolute)
            //};
            //launcher.Show();
        }
    }
}
