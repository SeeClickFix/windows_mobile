using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Common
{
    public static class YoutubeURLParser
    {
        static readonly string rxYoutube = @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)";

        static string ParseId(string videoUrl)
        {
            string id = null;
            if (!string.IsNullOrWhiteSpace(videoUrl))
            {
                Match m = Regex.Match(videoUrl.ToString(), rxYoutube);
                if (m.Success)
                {
                    id = m.Groups[1].Value;
                }
            }
            return id;
        }

        public static Uri Parse(string url)
        {
            Uri videoUri = null;
            string videoId = ParseId(url);
            if (!string.IsNullOrWhiteSpace(videoId))
            {
                videoUri = new Uri(string.Format("http://m.youtube.com/#/watch?v={0}", videoId), UriKind.Absolute);
            }
            return videoUri;
        }
    }
}
