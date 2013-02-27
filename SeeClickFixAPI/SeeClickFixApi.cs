using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    public class SeeClickFixApi
    {
        readonly string baseUrl = "http://[locale].seeclickfix.com/[api|devapi]issues.[format]";

        readonly string locale;
        readonly string secretKey;
        readonly bool isDev;

        public static Uri TOSUri
        {
            get
            {
                return new Uri(new Uri(string.Format("http://{0}", Domain), UriKind.Absolute), "/terms_of_use?print=true");
            }
        }

        public static readonly int PasswordMinLength = 4;

        public SeeClickFixApi(string secretKey, string locale = null, bool useDev = false)
        {
            this.secretKey = secretKey;
            this.locale = locale;
            this.isDev = useDev;

            Domain = string.Format("{0}{1}.com", string.IsNullOrWhiteSpace(locale) ? string.Empty : string.Format("{0}.", locale),
                useDev ? "seeclicktest" : "seeclickfix");
            this.baseUrl = string.Format("http://{0}/api", Domain);
        }

        public static string Domain
        {
            get;
            private set;
        }

        async Task<T> Execute<T>(RestRequest request, bool includeApiKey = true, bool secured = false) where T : new()
        {
            secured = !this.isDev;
            var client = new RestClient()
            {
                BaseUrl = secured ? this.baseUrl.Insert(4, "s") : this.baseUrl
            };
            client.AddHandler("application/json", new JsonDotNetDeserializer());
            if (includeApiKey)
            {
                request.AddParameter("api_key", this.secretKey);
            }

            DormantRestClient drc = new DormantRestClient();
            return await drc.ExecuteTaskAsync<T>(client, request);
        }


        async Task<T> Execute<T>(RestRequest request, string username, string password, bool includeApiKey = true, bool secured = false) where T : new()
        {
            secured = !this.isDev;
            var client = new RestClient()
            {
                BaseUrl = secured ? this.baseUrl.Insert(4, "s") : this.baseUrl
            };
            client.AddHandler("application/json", new JsonDotNetDeserializer());
            if (includeApiKey)
            {
                request.AddParameter("api_key", this.secretKey);
            }
            client.Authenticator = new HttpBasicAuthenticator(username, password);

            DormantRestClient drc = new DormantRestClient();
            return await drc.ExecuteTaskAsync<T>(client, request);
        }


        // http://[locale].seeclickfix.com/[api|devapi]issues.[format]
        public async Task<ICollection<Issue>> ListIssuesByAddressAsync(ListIssuesQuery query, string email)
        {
            // without this, image_square is not returned
            if (string.IsNullOrWhiteSpace(email))
            {
                email = string.Empty;
            }

            var request = new RestRequest("issues.json", Method.GET);
            query.SetupRequest(request);
            request.AddParameter("email", email);

            //request.AddUrlSegment("at", address);
            return await Execute<List<Issue>>(request);
        }

        public async Task<GeoCoordinate> AddressToGeoCoordinateAsync(string address)
        {
            var request = new RestRequest("geocodings/address_to_latlng.json?at={at}", Method.GET);
            request.AddUrlSegment("at", address);
            var coordinate = await Execute<Coordinate>(request);
            return new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
        }

        public async Task<Address> GeoCoordinateToAddress(GeoCoordinate geoCoordinate)
        {
            var request = new RestRequest("geocodings/latlng_to_address.json?lat={lat}&lng={lng}", Method.GET);
            request.AddUrlSegment("lat", geoCoordinate.Latitude.ToString());
            request.AddUrlSegment("lng", geoCoordinate.Longitude.ToString());
            return await Execute<Address>(request, false);
        }

        public async Task<ListWatchAreaResponse> ListWatchAreas(GeoCoordinate geoCoordinate)
        {
            var request = new RestRequest("enhanced_watch_areas/nearby_with_place.json", Method.GET);
            request.AddParameter("lat", geoCoordinate.Latitude.ToString());
            request.AddParameter("lng", geoCoordinate.Longitude.ToString());
            return await Execute<ListWatchAreaResponse>(request);
        }

        public async Task<RequestType[]> ListServiceRequestTypes(GeoCoordinate geoCoordinate)
        {
            var request = new RestRequest("enhanced_watch_areas/at.json?lat={lat}&lng={lng}", Method.GET);
            request.AddUrlSegment("lat", geoCoordinate.Latitude.ToString());
            request.AddUrlSegment("lng", geoCoordinate.Longitude.ToString());
            var response = await Execute<ListServiceRequestTypesResponse>(request);

            if (response.Details != null && response.Details.Length > 0)
            {
                // TODO: verify what happens if there are multiple or none
                return response.Details[0].RequestTypes;
            }
            else
            {
                return null;
            }
        }

        public async Task<ICollection<RequestTypeQuestion>> ListRequestTypeQuestions(int requestTypeId)
        {
            var request = new RestRequest("request_types/{id}/request_type_questions.json", Method.GET);
            request.AddUrlSegment("id", requestTypeId.ToString());
            return await Execute<List<RequestTypeQuestion>>(request);
        }

        public async Task<ICollection<IssueHistoryItem>> ListIssueHistory(int issueId)
        {
            var request = new RestRequest("issues/{id}/comments.json", Method.GET);
            request.AddUrlSegment("id", issueId.ToString());
            return await Execute<List<IssueHistoryItem>>(request, false);
        }

        public async Task<AddCommentResponse> AddComment(Comment comment)
        {
            VerifyParameter(comment, "comment");
            VerifyParameter(comment.IssueId, "comment.IssueId", () => comment.IssueId > 0);
            VerifyParameter(comment.Text, "comment.Text");
            VerifyParameter(comment.Email, "comment.Email");

            var request = new RestRequest("issues/{id}/comments.json", Method.POST);
            request.AddUrlSegment("id", comment.IssueId.ToString());

            request.AddParameter("comment[comment_type]", comment.ItemType.ToQueryStringValue());
            request.AddParameter("issue_id", comment.IssueId);
            request.AddParameter("comment[comment]", comment.Text);
            request.AddParameter("comment[email]", comment.Email);

            if (!string.IsNullOrWhiteSpace(comment.Name))
            {
                request.AddParameter("comment[name]", comment.Name);
            }
            if (!string.IsNullOrWhiteSpace(comment.EmailToSubscribe))
            {
                request.AddParameter("comment[send-email]", comment.EmailToSubscribe);
            }
            if (!string.IsNullOrWhiteSpace(comment.YoutubeURL))
            {
                request.AddParameter("comment[youtube_url]", comment.YoutubeURL);
            }

            if (comment.Photo != null)
            {
                byte[] buff = new byte[comment.Photo.Length];
                comment.Photo.Position = 0;
                comment.Photo.Read(buff, 0, buff.Length);
                request.AddFile("comment[comment_image_attributes][uploaded_data]", buff, comment.PhotoName, GetRequestContentTypeFromImageFile(comment.PhotoName));
            }

            // request.AddObject(comment);
            return await Execute<AddCommentResponse>(request);
        }

        public async Task<AddCommentResponse> CloseIssue(Comment comment)
        {
            comment.ItemType = IssueHistoryItemType.Closed;
            return await this.AddComment(comment);
        }

        public async Task<AddCommentResponse> ReopenIssue(Comment comment)
        {
            comment.ItemType = IssueHistoryItemType.Reopened;
            return await this.AddComment(comment);
        }

        public async Task<AddCommentResponse> AcknowledgeIssue(Comment comment)
        {
            comment.ItemType = IssueHistoryItemType.Acknowledged;
            return await this.AddComment(comment);
        }

        public async Task<bool> VoteIssue(int issueId, string email)
        {
            VerifyParameter(email, "email");

            var response = await this.AddComment(new Comment(issueId, "Voted for", email)
            {
                Name = string.Empty,
                ItemType = IssueHistoryItemType.Voted
            });
            return true;
        }

        public async Task<bool> FollowIssue(int issueId, string email)
        {
            VerifyParameter(email, "email");

            var response = await this.AddComment(new Comment(issueId, "Watcher added", email)
            {
                Name = string.Empty,
                ItemType = IssueHistoryItemType.WatcherAdded
            });
            return true;
        }

        public async Task<Issue> GetIssue(int issueId, string email)
        {
            var request = new RestRequest("issues/{id}.json", Method.GET);
            request.AddUrlSegment("id", issueId.ToString());

            if (string.IsNullOrWhiteSpace(email))
            {
                email = string.Empty;
            }
            request.AddParameter("email", email);
            var response = await Execute<List<Issue>>(request, false);
            if (response != null)
            {
                return response.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public async Task<User> GetUser(int userId)
        {
            var request = new RestRequest("users/{id}.json", Method.GET);
            request.AddUrlSegment("id", userId.ToString());
            return await Execute<User>(request, false);
        }

        public async Task<bool> FlagIssue(int issueId, string message = null)
        {
            var request = new RestRequest("rejected_contents.json", Method.POST);
            request.AddParameter("id", issueId);
            request.AddParameter("model", "issue");
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Content is inappropriate";
            };
            request.AddParameter("message", message);
            var r = await Execute<Response>(request, true);
            return r.Succeeded;
        }

        public async Task<UserLogin> Login(string email, string password)
        {
            VerifyParameter(email, "email");
            VerifyParameter(password, "password");

            var request = new RestRequest("session.json", Method.POST);
            request.AddParameter("login", email);
            request.AddParameter("password", password);
            return await Execute<UserLogin>(request, true, !this.isDev);
        }

        public async Task<UserLogin> Register(string name, string email, string password)
        {
            VerifyParameter(name, "name");
            VerifyParameter(email, "email");
            VerifyParameter(password, "password");

            var request = new RestRequest("users.json", Method.POST);
            request.AddParameter("user[name]", name);
            request.AddParameter("user[email]", email);
            request.AddParameter("user[password]", password);
            request.AddParameter("user[password_confirmation]", password);
            request.AddParameter("user[accept_terms]", "true");
            return await Execute<UserLogin>(request, true, !this.isDev);
        }

        public async Task<ICollection<IssueHistoryItem>> GetUserComments(int userId, int pageIndex)
        {
            var request = new RestRequest("users/{id}/comments.json", Method.GET);
            request.AddUrlSegment("id", userId.ToString());
            request.AddParameter("exclude_activity", true);
            request.AddParameter("page", pageIndex);
            return await Execute<List<IssueHistoryItem>>(request, false);
        }

        public async Task<ICollection<Issue>> GetUserFollowedIssues(int userId, int pageIndex, string email)
        {
            var request = new RestRequest("users/{id}/following.json", Method.GET);
            request.AddUrlSegment("id", userId.ToString());
            request.AddParameter("page", pageIndex);
            if (string.IsNullOrWhiteSpace(email))
            {
                email = string.Empty;
            }
            request.AddParameter("email", email);
            return await Execute<List<Issue>>(request, false);
        }

        public async Task<ICollection<Issue>> GetUserVotedIssues(int userId, int pageIndex, string email)
        {
            var request = new RestRequest("users/{id}/voted.json", Method.GET);
            request.AddUrlSegment("id", userId.ToString());
            request.AddParameter("page", pageIndex);
            if (string.IsNullOrWhiteSpace(email))
            {
                email = string.Empty;
            }
            request.AddParameter("email", email);
            return await Execute<List<Issue>>(request, false);
        }

        public async Task<ICollection<Issue>> GetUserClosedIssues(int userId, int pageIndex, string email)
        {
            var request = new RestRequest("users/{id}/closed.json", Method.GET);
            request.AddUrlSegment("id", userId.ToString());
            request.AddParameter("page", pageIndex);
            if (string.IsNullOrWhiteSpace(email))
            {
                email = string.Empty;
            }
            request.AddParameter("email", email);
            return await Execute<List<Issue>>(request, false);
        }

        public async Task<ICollection<Issue>> GetUserReportedIssues(int userId, int pageIndex, string email)
        {
            var request = new RestRequest("users/{id}/reported.json", Method.GET);
            request.AddUrlSegment("id", userId.ToString());
            request.AddParameter("page", pageIndex);
            if (string.IsNullOrWhiteSpace(email))
            {
                email = string.Empty;
            }
            request.AddParameter("email", email);
            return await Execute<List<Issue>>(request, false);
        }

        public async Task<AddIssueResponse> AddIssue(AddIssueQuery query)
        {
            VerifyParameter(query.Summary, "query.Summary");
            VerifyParameter(query.Location, "query.Location");
            VerifyParameter(query.ReporterEmail, "query.Email");

            var request = new RestRequest("issues.json", Method.POST);
            request.AddParameter("issue[summary]", query.Summary);
            request.AddParameter("issue[lat]", query.Location.Latitude);
            request.AddParameter("issue[lng]", query.Location.Longitude);
            request.AddParameter("issue[reporter_email]", query.ReporterEmail);

            // optional
            if (string.IsNullOrWhiteSpace(query.Description))
            {
                query.Description = string.Empty;
            }
            request.AddParameter("issue[description]", query.Description);
            request.AddParameter("issue[address]", query.Address);
            request.AddParameter("issue[reporter_display]", query.ReporterDisplay);

            if (query.Photo != null)
            {
                byte[] buff = new byte[query.Photo.Length];
                query.Photo.Position = 0;
                query.Photo.Read(buff, 0, buff.Length);
                request.AddFile("issue[issue_image_attributes][uploaded_data]", buff, query.PhotoName, GetRequestContentTypeFromImageFile(query.PhotoName));
            }

            if (query.RequestServiceId != null)
            {
                request.AddParameter("issue[request_type_id]", query.RequestServiceId.Value);

                // string answersJson = query.RequetServiceAnswers;
                request.AddParameter("issue_answers_json", query.RequetServiceAnswers);
            }

            return await Execute<AddIssueResponse>(request, true);
        }

        public async Task<ICollection<Message>> GetUserMessages(GetUserMessagesQuery query)
        {
            var request = new RestRequest("users/{id}/messages.json", Method.GET);
            request.AddUrlSegment("id", query.UserId.ToString());
            request.AddParameter("page", query.Page);
            request.AddParameter("num_results", query.ResultCount);
            return await Execute<List<Message>>(request, query.Username, query.Password, true);
        }

        string GetRequestContentTypeFromImageFile(string filename)
        {
            // remove the . from the extension
            return string.Format("image/{0}", Path.GetExtension(filename).Substring(1));
        }

        static void VerifyParameter(object param, string name)
        {
            if (param == null)
            {
                throw new ArgumentNullException("name");
            }
        }

        static void VerifyParameter(int param, string name, Func<bool> func)
        {
            if (!func())
            {
                throw new Exception("name");
            }
        }

        static void VerifyParameter(string param, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
        }
    }

    public class JsonDotNetDeserializer : IDeserializer
    {
        public JsonDotNetDeserializer()
        {
        }

        public string DateFormat
        {
            get;
            set;
        }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public string Namespace
        {
            get;
            set;
        }

        public string RootElement
        {
            get;
            set;
        }
    }

    public class DormantRestClient
    {
        bool wasDeactivated = false;

        public async Task<T> ExecuteTaskAsync<T>(RestClient client, RestRequest request) where T : new()
        {
            var phoneApplicationService = App.Current.ApplicationLifetimeObjects.OfType<PhoneApplicationService>().First();
            phoneApplicationService.Deactivated += phoneApplicationService_Deactivated;
            var t = await client.ExecuteTaskAsync<T>(request);
            if (this.wasDeactivated)
            {
                this.wasDeactivated = false;
                t = await this.ExecuteTaskAsync<T>(client, request);
            }
            return t;
        }

        void phoneApplicationService_Deactivated(object sender, DeactivatedEventArgs e)
        {
            (sender as PhoneApplicationService).Deactivated -= phoneApplicationService_Deactivated;
            this.wasDeactivated = true;
        }
    }
}
