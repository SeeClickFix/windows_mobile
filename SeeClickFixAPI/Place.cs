using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class Place
    {
        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "name_and_state")]
        public string NameAndState { get; set; }

        [DataMember(Name = "simplified_center")]
        public Coordinate SimplifiedCenter { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "url_name")]
        public string UrlName { get; set; }
    }
}
