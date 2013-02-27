using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SeeClickFix.WP8.Controls
{
    public class IssueTitleControl : Control
    {
        public IssueTitleControl()
        {
            //this.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            //this.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            //this.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            //this.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
            this.DefaultStyleKey = typeof(IssueTitleControl);
        }

        #region RequestTypes
        public IList RequestTypes
        {
            get { return (IList)GetValue(RequestTypesProperty); }
            set { SetValue(RequestTypesProperty, value); }
        }

        public static readonly DependencyProperty RequestTypesProperty =
            DependencyProperty.Register("RequestTypes", typeof(IList), typeof(IssueTitleControl), new PropertyMetadata(null, OnRequestTypesPropertyChanged));

        static void OnRequestTypesPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            IssueTitleControl control = (IssueTitleControl)o;
            control.UpdateControls();
        }
        #endregion

        #region SelectedRequestType
        public object SelectedRequestType
        {
            get { return GetValue(SelectedRequestTypeProperty); }
            set { SetValue(SelectedRequestTypeProperty, value); }
        }

        public static readonly DependencyProperty SelectedRequestTypeProperty =
            DependencyProperty.Register("SelectedRequestType", typeof(object), typeof(IssueTitleControl), new PropertyMetadata(null));
        #endregion

        #region Title
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(IssueTitleControl), new PropertyMetadata(null));
        #endregion

        void UpdateControls()
        {
            string targetName = this.RequestTypes.Count > 0 ? "ListPicker" : "TxtBox";
            (this.GetTemplateChild(targetName) as FrameworkElement).Visibility = System.Windows.Visibility.Visible;
        }
    }
}
