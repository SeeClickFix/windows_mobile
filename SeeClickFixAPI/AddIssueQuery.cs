using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    public class AddIssueQuery
    {
        public AddIssueQuery()
        {
            this.RequetServiceAnswers = new Dictionary<string, string>();
        }

        //
        // Required
        //

        [DataMember(Name="issue[summary]")]
        public string Summary { get; set; }

        [IgnoreDataMember]
        public GeoCoordinate Location { get; set; }

        [DataMember(Name = "issue[lat]")]
        double Lat { get { return this.Location.Latitude; } }

        [DataMember(Name = "issue[lng]")]
        double Lng { get { return this.Location.Longitude; } }

        //
        // Optional
        //
        
        [DataMember(Name = "issue[description]")]
        public string Description { get; set; }

        [DataMember(Name = "issue[address]")]
        public string Address { get; set; }

        [DataMember(Name = "[reporter_email]")]
        public string ReporterEmail { get; set; }

        [DataMember(Name = "[reporter_display]")]
        public string ReporterDisplay { get; set; }

        public Stream Photo { get; set; }

        public string PhotoName { get; set; }

        [DataMember(Name="issue[request_type_id]")]
        public int? RequestServiceId { get; set; }

        [DataMember(Name="device_os")]
        public string DeviceOs { get; set; }

        [DataMember(Name = "device_id")]
        public string DeviceId { get; set; }

        [DataMember(Name = "device_name")]
        public string DeviceName { get; set; }

        [IgnoreDataMember]
        public Dictionary<string, string> RequetServiceAnswers { get; set; }
    }
}
