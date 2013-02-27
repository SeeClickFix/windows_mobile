using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Services.Impl
{
    public class SCFDataServiceDesign : ISCFDataService
    {
       public Task<ICollection<Issue>> ListIssuesByAddressAsync(ListIssuesQuery query, string email)
        {
            var issues = new List<Issue>(new Issue[] { new Issue() { Id = 1, Address = "Mineola Blvd" } });
            var task = new Task<ICollection<Issue>>(() => { return issues; });
            return task;
        }

        public Task<GeoCoordinate> AddressToGeoCoordinateAsync(string address)
        {
            return null;
        }

        public Task<Address> GeoCoordinateToAddress(GeoCoordinate geoCoordinate)
        {
            return null;
        }

        public Task<ICollection<IssueHistoryItem>> ListIssueHistory(int issueId)
        {
            var item = new IssueHistoryItem()
                    {
                        Name = "Ben",
                        Comment = "Win won for the City",
                        CommentType = "Comment",
                        IssueId = 395620,
                        MinutesSinceCreatead = 384
                    };
            var items = new List<IssueHistoryItem>(new IssueHistoryItem[] { item });

            return new Task<ICollection<IssueHistoryItem>>(() => { return items; });
        }

        public Task<AddCommentResponse> AddComment(Comment comment)
        {
            return null;
        }

        public Task<AddCommentResponse> CloseIssue(Comment comment)
        {
            return null;
        }

        public Task<AddCommentResponse> ReopenIssue(Comment comment)
        {
            return null;
        }

        public Task<AddCommentResponse> AcknowledgeIssue(Comment comment)
        {
            return null;
        }

        public Task<bool> VoteIssue(int issueId, string email)
        {
            return null;
        }

        public Task<bool> FollowIssue(int issueId, string email)
        {
            return null;
        }

        public Task<Issue> GetIssue(int issueId, string email)
        {
            return null;
        }

        public Task<User> GetUser(int userId)
        {
            var user = new User()
            {
                Name = "Andrei Nitescu",
                CivicPoints = 10000,
                WittyTitle = "City Mayor",
                ReportedIssueCount = 100,
                ClosedIssueCount = 50,
                VotedIssueCount = 20
            };
            return new Task<User>(() => { return user; });
        }

        public Task<bool> FlagIssue(int issueId, string message = null)
        {
            return new Task<bool>(() => { return true; });
        }

        public Task<UserLogin> Login(string email, string password)
        {
            return null;
        }
        public Task<UserLogin> Register(string name, string email, string password)
        {
            return null;
        }
        public Task<RequestType[]> ListServiceRequestTypes(GeoCoordinate geoCoordinate)
        {
            return null;
        }
        public Task<ICollection<RequestTypeQuestion>> ListRequestTypeQuestions(int requestTypeId)
        {
            return null;
        }
        public Task<AddIssueResponse> AddIssue(AddIssueQuery query)
        {
            return null;
        }
        public Task<ListWatchAreaResponse> ListWatchAreas(GeoCoordinate geoCoordinate)
        {
            return null;
        }

        public Task<ICollection<Message>> GetUserMessages(GetUserMessagesQuery query)
        {
            return null;
        }
    }
}
