using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeClickFix.WP8.Controls
{
    public class IssueQuestionControl : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            RequestTypeQuestion question = (RequestTypeQuestion)item;
            string templateName = string.Format("{0}QuestionDataTemplate", question.QuestionType.ToString());
            
            DataTemplate dataTemplate = App.Current.Resources[templateName] as DataTemplate;

            return dataTemplate;
            //switch (questionType)
            //{
            //    case RequestTypeQuestionType.Text:
            //    case RequestTypeQuestionType.Textarea:
            //    case RequestTypeQuestionType.Datetime:
            //        return;
            //}
            // return null;
        }
    }
}
