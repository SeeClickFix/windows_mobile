using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class ListServiceRequestTypesResponseItem
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "includes_point")]
        public bool IncludesPoint { get; set; }

        [DataMember(Name = "request_types")]
        public RequestType[] RequestTypes { get; set; }
    }
}
