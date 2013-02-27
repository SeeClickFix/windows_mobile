using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Services
{
    public interface ISCFDataService
    {
        Task<ICollection<Issue>> ListIssuesByAddressAsync(ListIssuesQuery query, string email);
        Task<GeoCoordinate> AddressToGeoCoordinateAsync(string address);
        Task<Address> GeoCoordinateToAddress(GeoCoordinate geoCoordinate);
        Task<ICollection<IssueHistoryItem>> ListIssueHistory(int issueId);
        Task<AddCommentResponse> AddComment(Comment comment);
        Task<AddCommentResponse> CloseIssue(Comment comment);
        Task<AddCommentResponse> ReopenIssue(Comment comment);
        Task<AddCommentResponse> AcknowledgeIssue(Comment comment);
        Task<bool> VoteIssue(int issueId, string email);
        Task<bool> FollowIssue(int issueId, string email);
        Task<Issue> GetIssue(int issueId, string email);
        Task<User> GetUser(int userId);
        Task<bool> FlagIssue(int issueId, string message = null);
        Task<UserLogin> Login(string email, string password);
        Task<UserLogin> Register(string name, string email, string password);
        Task<RequestType[]> ListServiceRequestTypes(GeoCoordinate geoCoordinate);
        Task<ICollection<RequestTypeQuestion>> ListRequestTypeQuestions(int requestTypeId);
        Task<AddIssueResponse> AddIssue(AddIssueQuery query);
        Task<ListWatchAreaResponse> ListWatchAreas(GeoCoordinate geoCoordinate);
        Task<ICollection<Message>> GetUserMessages(GetUserMessagesQuery query);
    }
}
