using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class GetIssueResponse
    {
        [DataMember]
        public Issue[] Issues { get; set; }
    }
}
