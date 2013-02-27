using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class Coordinate
    {
        [DataMember(Name = "lng")]
        public double Longitude { get; set; }

        [DataMember(Name = "lat")]
        public double Latitude { get; set; }

        public GeoCoordinate ToGeoCoordinate()
        {
            return new GeoCoordinate(this.Latitude, this.Longitude);
        }

        public static Coordinate FromGeoCoordinate(GeoCoordinate c)
        {
            return new Coordinate()
            {
                Longitude = c.Longitude,
                Latitude = c.Latitude
            };
        }
    }
}
