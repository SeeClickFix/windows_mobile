using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SeeClickFix.WP8.Controls
{
    public class ListPickerEx : ListPicker
    {
        private const string ItemsPresenterHostName = "ItemsPresenterHost";
        private const string ItemPlaceholderHostName = "ItemPlaceholderHostName";

        FrameworkElement _itemsPresenterHost;
        FrameworkElement _itemPlaceholderHost;

        public DataTemplate ItemPlaceholderTemplate
        {
            get { return (DataTemplate)GetValue(ItemPlaceholderTemplateProperty); }
            set { SetValue(ItemPlaceholderTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemPlaceholderTemplateProperty =
            DependencyProperty.Register("ItemPlaceholderTemplate", typeof(DataTemplate), typeof(ListPickerEx), new PropertyMetadata(null));

        public object ItemPlaceholder
        {
            get { return GetValue(ItemPlaceholderProperty); }
            set { SetValue(ItemPlaceholderProperty, value); }
        }

        public static readonly DependencyProperty ItemPlaceholderProperty =
            DependencyProperty.Register("ItemPlaceholder", typeof(object), typeof(ListPickerEx), new PropertyMetadata(null));

        public ListPickerEx()
        {
            this.DefaultStyleKey = typeof(ListPickerEx);
            this.SelectionChanged += ListPickerEx_SelectionChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsPresenterHost = base.GetTemplateChild(ItemsPresenterHostName) as FrameworkElement;
            _itemPlaceholderHost = GetTemplateChild(ItemPlaceholderHostName) as FrameworkElement;
            this.UpdateVisibility();
        }

        void ListPickerEx_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.UpdateVisibility();
        }

        void UpdateVisibility()
        {
            if (this._itemPlaceholderHost == null || this.ExpansionMode != Microsoft.Phone.Controls.ExpansionMode.FullScreenOnly)
            {
                return;
            }

            if (this.ListPickerMode == Microsoft.Phone.Controls.ListPickerMode.Normal)
            {
                if (this.SelectedItem == null)
                {
                    _itemsPresenterHost.Visibility = System.Windows.Visibility.Collapsed;
                    _itemPlaceholderHost.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    _itemsPresenterHost.Visibility = System.Windows.Visibility.Visible;
                    _itemPlaceholderHost.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else if (this.ListPickerMode == Microsoft.Phone.Controls.ListPickerMode.Full)
            {
                _itemsPresenterHost.Visibility = System.Windows.Visibility.Visible;
                _itemPlaceholderHost.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
