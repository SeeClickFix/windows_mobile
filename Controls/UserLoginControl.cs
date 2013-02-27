using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace SeeClickFix.WP8.Controls
{
    public class UserLoginControl : Control
    {
        public UserLoginControl()
        {
            this.DefaultStyleKey = typeof(UserLoginControl);
            Binding b = new Binding()
            {
                Path = new System.Windows.PropertyPath("UserProfileService.IsLogged"),
                Converter = App.Current.Resources["ObjectToVisibilityConverter"] as IValueConverter,
                ConverterParameter = "inverse"
            };
            this.SetBinding(Control.VisibilityProperty, b);
        }
    }
}
