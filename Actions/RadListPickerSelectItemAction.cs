using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;

namespace SeeClickFix.WP8.Actions
{
    public class RadListPickerSelectItemAction : ChangePropertyAction
    {
        public RadListPickerSelectItemAction()
        {
            this.PropertyName = "SelectedItem";
        }

        protected override void Invoke(object parameter)
        {
            var suggestion = (parameter as SuggestionSelectedEventArgs).SelectedSuggestion;

            // get listbox and select item
            RadAutoCompleteBox autoCompleteBox = this.AssociatedObject as RadAutoCompleteBox;
            var lbox = autoCompleteBox.FindName("PopupList") as RadDataBoundListBox;
            lbox.SelectedItem = suggestion;

            // get picker, select item and close it
            var picker = lbox.GetBindingExpression(RadDataBoundListBox.IsCheckModeActiveProperty).DataItem as RadListPicker;
            picker.SelectedItem = lbox.SelectedItem;
            picker.IsExpanded = false;

            // clear selection in autocompletebox
            autoCompleteBox.Text = string.Empty;
        }
    }
}
