using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Common
{
    public static class SCFImageUtils
    {
        public static bool IsStockImage(string url)
        {
            string lowercasedUrl = url.ToLowerInvariant();
            return
                lowercasedUrl.Contains(SeeClickFixApi.Domain.ToLowerInvariant()) &&
                lowercasedUrl.Contains("/images/categories".ToLowerInvariant());
        }

        public static bool IsStockImage(Uri uri)
        {
            var url = uri.ToString();
            return IsStockImage(url);
        }
    }
}
