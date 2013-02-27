using SeeClickFix.WP8.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class IssueHistoryItem
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "comment_type")]
        public string CommentType { get; set; }

        [IgnoreDataMember]
        public IssueHistoryItemType ItemType
        {
            get
            {
                return IssueHistoryItemTypeExtensions.Parse(this.CommentType);
            }
        }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [DataMember(Name = "issue_id")]
        public int IssueId { get; set; }

        [DataMember(Name = "youtube_url")]
        public string YoutubeUrl { get; set; }

        [DataMember(Name = "minutes_since_created")]
        public long MinutesSinceCreatead { get; set; }

        [DataMember(Name = "comment_image_path")]
        public string CommentImagePath { get; set; }

        [DataMember(Name = "small_comment_image_path")]
        public string SmallCommentImagePath { get; set; }

        [DataMember(Name = "square_comment_image_path")]
        public string SquareCommentImagePath { get; set; }

        [DataMember(Name = "video_path")]
        public string VideoPath { get; set; }

        [DataMember(Name = "user")]
        public User User { get; set; }

        [IgnoreDataMember]
        public Uri VideoThumbnailPath
        //public Uri VideoThumbnailPath
        {
            get
            {
                string videoUrl = null;
                if (!string.IsNullOrWhiteSpace(this.YoutubeUrl))
                {
                    videoUrl = this.YoutubeUrl;
                }
                else
                {
                    videoUrl = this.VideoPath;
                }
                return VideoThumbnailParser.Parse(videoUrl);
            }
        }

        [IgnoreDataMember]
        public Uri YoutubeUri
        {
            get
            {
                return YoutubeURLParser.Parse(this.YoutubeUrl);
            }
        }
    }
}
