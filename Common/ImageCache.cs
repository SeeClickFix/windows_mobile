using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SeeClickFix.WP8.Common
{
    public static class ImageCache
    {
        public static readonly BitmapImage IssueTypeNoImage = GetImage("no-image.png");
        public static readonly BitmapImage ImageNoAvatar = GetImage("no-avatar.png");

        public static BitmapImage GetImage(string name)
        {
            switch (name)
            {
                case "no-image":
                    return IssueTypeNoImage;
                case "avatar":
                    return ImageNoAvatar;
                default:
                    return new BitmapImage(new Uri(string.Format("/Assets/{0}", name), UriKind.RelativeOrAbsolute));
            }
        }
    }
}
