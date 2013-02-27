using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    public static class VideoThumbnailParser
    {
        static readonly string rxYoutube = @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)";
        static readonly string rxVimeo = @"vimeo\.com/(?:.*#|.*/videos/)?([0-9]+)";

        public static Uri Parse(string videoUrl)
        {
            if(string.IsNullOrWhiteSpace(videoUrl))
            {
                return null;
            }

            string thumbnailUri = null;
            Match m = Regex.Match(videoUrl.ToString(), rxYoutube);
            if (m.Success)
            {
                string id = m.Groups[1].Value;
                thumbnailUri = string.Format("http://img.youtube.com/vi/{0}/default.jpg", id);
            }
            else
            {
                //m = Regex.Match(uri.ToString(), rxVimeo);
                //if (m.Success)
                //{
                //    string id = m.Groups[1].Value;
                //    thumbnailUri = string.Format("http://img.youtube.com/vi/{0}", id);
                //}
            }

            return string.IsNullOrWhiteSpace(thumbnailUri) ? null : new Uri(thumbnailUri, UriKind.Absolute);
        }
    }
}
