using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class Geocode
    {
        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }

        [DataMember(Name = "full_address")]
        public string FullAddress { get; set; }

        [DataMember(Name = "is_us")]
        public bool IsUs { get; set; }

        [DataMember(Name = "lat")]
        public float Lat { get; set; }

        [DataMember(Name = "lng")]
        public float Lng { get; set; }

        [DataMember(Name = "precision")]
        public string Precision { get; set; }

        [DataMember(Name = "province")]
        public string Province { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "street_address")]
        public string StreetAddress { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "zip")]
        public string ZipCode { get; set; }
    }
}
