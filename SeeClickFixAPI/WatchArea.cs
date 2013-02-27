using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class WatchArea
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        //[DataMember(Name = "demo_code")]
        //public string DemoCode { get; set; }

        //[DataMember(Name = "demo_code")]
        //public string DemoCode { get; set; }

        [DataMember(Name = "h_background_color")]
        public string BackgroundColor { get; set; }

        [DataMember(Name = "h_button_color")]
        public string ButtonColor { get; set; }

        [DataMember(Name = "h_highlight_color")]
        public string HighlightColor { get; set; }

        [DataMember(Name = "h_start_gradient_button_color")]
        public string GradientButtonColor { get; set; }

        [DataMember(Name = "h_text_color")]
        public string TextColor { get; set; }

        //[DataMember(Name = "icon_color")]
        //public string TextColor { get; set; }

        [DataMember(Name = "logo_path")]
        public string LogoPath { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
