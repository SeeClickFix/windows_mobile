using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeClickFix.WP8.Controls
{
    public class TemplateSelectorControl : DataTemplateSelector
    {
        public DataTemplate PlaceholderTemplate
        {
            get;
            set;
        }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            bool itemAvailable = (item == null);
            if (item is string)
            {
                itemAvailable = !string.IsNullOrWhiteSpace((string)item);
            }
            return itemAvailable ? this.ContentTemplate : this.PlaceholderTemplate;
        }
    }
}
