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
    public class Issue
    {
        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "bitly")]
        public string Bitly { get; set; }

        [DataMember(Name = "comment_count_excluding_activity")]
        public int CommentCount { get; set; }

        [DataMember(Name = "created_at_epoch")]
        public long CreatedAtEpoch
        {
            get;
            set;
        }

        [IgnoreDataMember]
        public DateTime CreatedAt 
        {
            get 
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var dateTime = epoch.AddMilliseconds(this.CreatedAtEpoch).ToLocalTime();
                return dateTime;
            }
        }

        //[DataMember(Name = "created_at")]
        //public string CreatedAt { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "following")]
        public bool IsFollowing { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        //[DataMember(Name = "issue_id")]
        //public int IssueId { get; set; }

        [DataMember(Name = "lat")]
        public float Lat { get; set; }

        [DataMember(Name = "lng")]
        public float Long { get; set; }

        [DataMember(Name = "minutes_since_created")]
        public int MinutesSinceCreated { get; set; }
        
        [DataMember(Name = "rating")]
        public int Rating { get; set; }

        [DataMember(Name = "reporter_display")]
        public string Reporter { get; set; }

        [DataMember(Name = "slug")]
        public string Slug { get; set; }

        [DataMember(Name = "square_image")]
        public string Image { get; set; }

        [DataMember(Name = "public_filename")]
        public string PublicFilename { get; set; }

        [DataMember(Name = "small_public_filename")]
        public string SmallPublicFilename { get; set; }

        [DataMember(Name = "square_public_filename")]
        public string SquarePublicFilename { get; set; }

        [DataMember(Name = "status")]
        public IssueStatus Status { get; set; }

        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        [DataMember(Name = "updated_at_raw")]
        public string UpdatedAt { get; set; }

        [DataMember(Name = "user_id")]
        public int? UserId { get; set; }

        [DataMember(Name = "voted_before")]
        public bool WasVoted { get; set; }

        [IgnoreDataMember]
        public GeoCoordinate GeoCoordinate
        {
            get
            {
                return new GeoCoordinate(this.Lat, this.Long);
            }
        }

        public Issue()
        {
        }
    }
}
