using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "place_id")]
        public int? PlaceId { get; set; }

        [DataMember(Name = "civic_points")]
        public int CivicPoints { get; set; }

        [DataMember(Name = "witty_title")]
        public string WittyTitle { get; set; }

        [DataMember(Name = "voted_issue_count")]
        public int VotedIssueCount { get; set; }

        [DataMember(Name = "reported_issue_count")]
        public int ReportedIssueCount { get; set; }

        [DataMember(Name = "comments_count")]
        public int CommentsCount { get; set; }

        [DataMember(Name = "closed_issue_count")]
        public int ClosedIssueCount { get; set; }

        [DataMember(Name = "following_issue_count")]
        public int FollowingIssueCount { get; set; }

        [DataMember(Name = "square_image")]
        public string SquareImage { get; set; }

        [DataMember(Name = "public_filename")]
        public string PublicFilename { get; set; }
    }
}
