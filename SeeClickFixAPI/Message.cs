using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class Message
    {
        //public string action { get; set; }
        [DataMember(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }

        //public string deleted_at { get; set; }

        [DataMember(Name = "excerpt")]
        public string Excerpt { get; set; }

        //public string id { get; set; }

        [DataMember(Name = "issue_id")]
        public int? IssueId { get; set; }

        //public string model_id { get; set; }

        //public string model_klass { get; set; }

        //public string read_at { get; set; }

        ////public object relative_path { get; set; }

        //public string sender_id { get; set; }

        //public string sent_at { get; set; }

        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        //[DataMember(Name = "updated_at ")]
        //public DateTime? updated_at { get; set; }

        [DataMember(Name = "user_id")]
        public int? UserId { get; set; }
    }
}
