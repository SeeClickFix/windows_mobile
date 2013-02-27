using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class RequestTypeQuestion
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }

        // Whether or not the answer will be publicly displayed
        [DataMember(Name = "display_answer")]
        public bool DisplayAnswer { get; set; }

        // Suggested position to display the question.
        [DataMember(Name = "position")]
        public int? Position { get; set; }

        // Unique string for the question. Must be passed when answering questions in creating an issue.
        [DataMember(Name = "primary_key")]
        public string PrimaryKey { get; set; }

        // Question to prompt with.
        [DataMember(Name = "question")]
        public string Question { get; set; }

        // - One of [text,textarea,select,number,datetime, multivaluelist, hidden, note]
        [DataMember(Name = "question_type")]
        public RequestTypeQuestionType QuestionType { get; set; }

        [DataMember(Name = "required_response")]
        public bool IsRequired { get; set; }

        // example: select_values=FALSE=No|TRUE=Yes
        [DataMember(Name = "select_values")]
        public string SelectValues { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime? UpdatedAt { get; set; }

        List<SelectQuestionValue> selectValuesItems;
        public IEnumerable<SelectQuestionValue> SelectValuesItems 
        { 
            get 
            {
                if(this.selectValuesItems != null)
                {
                    return this.selectValuesItems;
                }

                this.selectValuesItems = new List<SelectQuestionValue>();
                if (!string.IsNullOrWhiteSpace(this.SelectValues))
                {

                    //Regex rx = new Regex("");
                    //var matches = rx.Matches(this.SelectValues);
                    //foreach (Match m in matches)
                    //{
                    //    yield return new SelectQuestionValue()
                    //    {
                    //        Id = m.Captures[0].Value,
                    //        Name = m.Captures[1].Value
                    //    };
                    //}

                    var groups = this.SelectValues.Split(new char[] { '|' });
                    foreach (string group in groups)
                    {
                        var s = group.Split(new char[] { '=' });
                        this.selectValuesItems.Add(new SelectQuestionValue() { Id = s[0], Name = s[1] });
                    }
                }

                return this.selectValuesItems;
            }
        }
    }
}
