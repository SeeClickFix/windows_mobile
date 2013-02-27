using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Net;
using RestSharp;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    public class ListIssuesQuery
    {
        public ListIssuesQuery()
        {
            this.Status = new IssueStatus[] { IssueStatus.Open };
            this.ResultCount = 20;
            this.Page = 1;
            this.Sort = IssueSort.DateCreated;
            this.SortDirection = Direction.Descending;
            this.Zoom = 10;
        }

        public string Address { get; set; }
        public GeoCoordinate Coordinate { get; set; }
        public int Zoom { get; set; }
        public IssueStatus[] Status { get; set; }
        //public float? StartHoursAgo { get; set; }
        //public float? EndHoursAgo { get; set; }
        public int ResultCount { get; set; }
        public int Page { get; set; }
        public string Keyword { get; set; }
        public IssueSort Sort { get; set; }
        public Direction SortDirection { get; set; }

        public void SetupRequest(RestRequest request)
        {
            string query = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.Address))
            {
                request.AddParameter("at", this.Address);
            }

            if (this.Coordinate != null)
            {
                request.AddParameter("lat", this.Coordinate.Latitude);
                request.AddParameter("lng", this.Coordinate.Longitude);
            }

            request.AddParameter("zoom", this.Zoom);

            if (this.Status != null)
            {
                foreach (IssueStatus status in this.Status)
                {
                    string queryParamName  = string.Format("status[{0}]", status);
                    request.AddParameter(queryParamName, true);
                }
            }

            request.AddParameter("num_results", this.ResultCount);
            request.AddParameter("page", this.Page);

            if (!string.IsNullOrWhiteSpace(this.Keyword))
            {
                request.AddParameter("search", this.Keyword);
            }

            string sort = string.Empty;
            switch (this.Sort)
            {
                case IssueSort.DateCreated:
                   sort = "issues.created_at";
                    break;
                case IssueSort.IssueRating:
                    sort = "issues.rating";
                    break;
                case IssueSort.IssueHotness:
                    sort = "issues.hot ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Sort");
            }
            request.AddParameter("sort", sort);

            string sortdir = string.Empty;
            switch (this.SortDirection)
            {
                case Direction.Ascending:
                    sortdir = "ASC";
                    break;
                case Direction.Descending:
                    sortdir = "DESC";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("SortDirection");
            }
            request.AddParameter("direction", sortdir);
            //return query.Substring(1);
        }
    }

    //public static class QueryStringBuilder
    //{

    //}

    
}
