using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeClickFix.WP8.Controls
{
    public class DataTypeDataTemplateSelector : DataTemplateSelector
    {
        public string Prefix { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var name = item.GetType().Name;
            DataTemplate dataTemplate = App.Current.Resources[string.Format("{0}{1}DataTemplate", name, Prefix ?? string.Empty)] as DataTemplate;
            return dataTemplate;
        }
    }
}
