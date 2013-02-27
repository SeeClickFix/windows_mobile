using SeeClickFix.WP8.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SeeClickFix.WP8.Behaviors
{
    public class LoadMoreBehavior : Behavior<ScrollViewer>
    {
        //public ICommand LoadMoreDataCommand { get; set; }

        public static readonly DependencyProperty LoadMoreDataCommandProperty =
            DependencyProperty.Register("LoadMoreDataCommand", typeof(ICommand), typeof(LoadMoreBehavior), null);

        public ICommand LoadMoreDataCommand
        {
            get { return (ICommand)GetValue(LoadMoreDataCommandProperty); }
            set { SetValue(LoadMoreDataCommandProperty, value); }
        }

        //public static readonly DependencyProperty CommandParamenterProperty = DependencyProperty.Register("CommandParamenter", typeof(object), typeof(XamMenuItemCommandBehavior), null);
        //public object CommandParamenter
        //{
        //    get { return (object)GetValue(CommandParamenterProperty); }
        //    set { SetValue(CommandParamenterProperty, value); }
        //}

        //public static DependencyProperty AtEndCommandProperty = DependencyProperty.RegisterAttached(
        //   "AtEndCommand", typeof(ICommand),
        //   typeof(LoadMoreBehavior),
        //   new PropertyMetadata(OnAtEndCommandChanged));

        //public static ICommand GetAtEndCommand(DependencyObject obj)
        //{
        //    return (ICommand)obj.GetValue(AtEndCommandProperty);
        //}

        //public static void SetAtEndCommand(DependencyObject obj, ICommand value)
        //{
        //    obj.SetValue(AtEndCommandProperty, value);
        //}

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyPropertyListener listener = new DependencyPropertyListener();
            listener.Changed += delegate
            {
                bool atBottom = this.AssociatedObject.VerticalOffset >= this.AssociatedObject.ScrollableHeight;
                if (atBottom)
                {
                    if (this.LoadMoreDataCommand != null && this.LoadMoreDataCommand.CanExecute(null))
                    {
                        this.LoadMoreDataCommand.Execute(null);
                    }                    
                }
            };
            Binding binding = new Binding("VerticalOffset") { Source = this.AssociatedObject };
            listener.Attach(this.AssociatedObject, binding);
        }
    }
}
