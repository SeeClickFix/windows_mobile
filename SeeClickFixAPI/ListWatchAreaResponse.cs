using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class ListWatchAreaResponse
    {
        [DataMember(Name = "enhanced_watch_area_id")]
        public int? Id { get; set; }

        [DataMember(Name = "enhanced_watch_areas")]
        public WatchArea[] Areas { get; set; }

        [DataMember(Name = "geocode")]
        public Geocode Geocode { get; set; }

        [DataMember(Name = "place")]
        public Place Place { get; set; }
    }
}
