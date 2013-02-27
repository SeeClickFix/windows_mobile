using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    public class Comment
    {
        [SerializeAs(Name = "issue_id")]
        public int IssueId { get; set; }

        [SerializeAs(Name = "comment[comment]")]
        public string Text { get; set; }

        [SerializeAs(Name = "comment[email]")]
        public string Email { get; set; }

        // optional params
        [SerializeAs(Name = "comment[name]")]
        public string Name { get; set; }

        [SerializeAs(Name = "comment[send_email]")]
        public string EmailToSubscribe { get; set; }

        [SerializeAs(Name = "comment[youtube_url]")]
        public string YoutubeURL { get; set; }

        [SerializeAs(Name = "comment[comment_type]")]
        public IssueHistoryItemType ItemType
        {
            get;
            set;
        }

        [IgnoreDataMember]
        public Stream Photo { get; set; }

        [IgnoreDataMember]
        public string PhotoName { get; set; }

        public Comment(int issueId, string comment, string email) : this()
        {
            this.IssueId = issueId;
            this.Text = comment;
            this.Email = email;
        }

        public Comment()
        {
            this.ItemType = IssueHistoryItemType.Comment;
        }

        //[DataMember(Name = "comment[youtube_url]")]
        //public string YoutubeURL { get; set; }
    }
}
