using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Services.Impl
{
    public class SCFDataService : ISCFDataService
    {
        SeeClickFixApi api;

        public SCFDataService()
        {
            this.api =  new SeeClickFixApi("API KEY", null, true);
        }

        public async Task<ICollection<Issue>> ListIssuesByAddressAsync(ListIssuesQuery query, string email)
        {
            return await this.api.ListIssuesByAddressAsync(query, email);
        }

        public async Task<GeoCoordinate> AddressToGeoCoordinateAsync(string address)
        {
            return await this.api.AddressToGeoCoordinateAsync(address);
        }

        public async Task<Address> GeoCoordinateToAddress(GeoCoordinate geoCoordinate)
        {
            return await this.api.GeoCoordinateToAddress(geoCoordinate);
        }

        public async Task<ICollection<IssueHistoryItem>> ListIssueHistory(int issueId)
        {
            return await this.api.ListIssueHistory(issueId);
        }

        public async Task<AddCommentResponse> AddComment(Comment comment)
        {
            return await this.api.AddComment(comment);
        }

        public async Task<AddCommentResponse> CloseIssue(Comment comment)
        {
            return await this.api.CloseIssue(comment);
        }

        public async Task<AddCommentResponse> ReopenIssue(Comment comment)
        {
            return await this.api.ReopenIssue(comment);
        }

        public async Task<AddCommentResponse> AcknowledgeIssue(Comment comment)
        {
            return await this.api.AcknowledgeIssue(comment);
        }

        public async Task<bool> VoteIssue(int issueId, string email)
        {
            return await this.api.VoteIssue(issueId, email);
        }

        public async Task<bool> FollowIssue(int issueId, string email)
        {
            return await this.api.FollowIssue(issueId, email);
        }
        public async Task<Issue> GetIssue(int issueId, string email)
        {
            return await this.api.GetIssue(issueId, email);
        }
        public async Task<User> GetUser(int userId)
        {
            return await this.api.GetUser(userId);
        }
        public async Task<bool> FlagIssue(int issueId, string message = null)
        {
            return await this.api.FlagIssue(issueId, message);
        }
        public async Task<UserLogin> Login(string email, string password)
        {
            return await this.api.Login(email, password);
        }
        public async Task<UserLogin> Register(string name, string email, string password)
        {
            return await this.api.Register(name, email, password);
        }
        public async Task<RequestType[]> ListServiceRequestTypes(GeoCoordinate geoCoordinate)
        {
            return await this.api.ListServiceRequestTypes(geoCoordinate);
        }
        public async Task<ICollection<RequestTypeQuestion>> ListRequestTypeQuestions(int requestTypeId)
        {
            return await this.api.ListRequestTypeQuestions(requestTypeId);
        }
        public async Task<AddIssueResponse> AddIssue(AddIssueQuery query)
        {
            return await this.api.AddIssue(query);
        }
        public async Task<ListWatchAreaResponse> ListWatchAreas(GeoCoordinate geoCoordinate)
        {
            return await this.api.ListWatchAreas(geoCoordinate);
        }

        public async Task<ICollection<Message>> GetUserMessages(GetUserMessagesQuery query)
        {
            return await this.api.GetUserMessages(query);
        }
    }
}
