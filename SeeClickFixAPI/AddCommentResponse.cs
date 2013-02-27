using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class AddCommentResponse
    {
        [DataMember(Name = "comment")]
        public string Comment { get; set; } 

        [DataMember(Name = "comment-type")]
        public string CommentType { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
