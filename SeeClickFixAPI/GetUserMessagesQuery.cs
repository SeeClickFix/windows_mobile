using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    public class GetUserMessagesQuery
    {
        public GetUserMessagesQuery()
        {
            this.Page = 1;
            this.ResultCount = 20;
        }

        public int UserId { get; set; }

        public int Page { get; set; }

        public int ResultCount { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
