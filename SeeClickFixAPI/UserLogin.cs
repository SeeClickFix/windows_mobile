using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public class UserLogin : ResponseBase
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "user_id")]
        public int Id { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "can_ack")]
        public bool CanAcknowledge { get; set; }

        //[DataMember(Name = "response")]
        //public bool CanAcknowledge { get; set; }
    }
}
