using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class RequestType
    {
        [DataMember(Name = "additional_questions_count")]
        public int AdditionalQuestionsCount { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        public bool IsOther
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Title) && this.Title.ToLowerInvariant() == "other";
            }
        }
    }
}
