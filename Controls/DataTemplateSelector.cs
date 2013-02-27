using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SeeClickFix.WP8.Controls
{
    public abstract class DataTemplateSelector : ContentControl
    {
        public DataTemplateSelector()
        {
            this.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }
}
