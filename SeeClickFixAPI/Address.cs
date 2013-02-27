using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class Address
    {
        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "full_address")]
        public string FullAddress { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "state_combined")]
        public string StateCombined { get; set; }

        [DataMember(Name = "street_address")]
        public string StreetAddress { get; set; }
    }
}
