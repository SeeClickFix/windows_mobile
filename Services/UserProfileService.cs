using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Services
{
    public class UserProfileService
    {
        public static UserProfileService Inst = new UserProfileService();

        public event EventHandler<UserProfile> UserProfileChanged = null;

        public UserProfile UserProfile
        {
            get;
            private set;
        }

        public bool IsLogged
        {
            get
            {
                return this.UserProfile.UserId != null;
            }
        }

        public void Save()
        {
            if (!this.UserProfile.IsChanged)
            {
                return;
            }

            IsolatedStorageSettings.ApplicationSettings["userprofile"] = JsonConvert.SerializeObject(this.UserProfile);
            this.UserProfile.SetUnchanged();

            if (this.UserProfileChanged != null)
            {
                this.UserProfileChanged(this, this.UserProfile);
            }
        }

        public void Load()
        {
            string strUserProfile;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("userprofile", out strUserProfile))
            {
                this.UserProfile = JsonConvert.DeserializeObject<UserProfile>(strUserProfile);
            }
            else
            {
                this.UserProfile = new UserProfile();
            }
        }

        public void Clear()
        {
            this.UserProfile.Email = null;
            this.UserProfile.Name = null;
            this.UserProfile.Password = null;
            this.UserProfile.UserId = null;
            this.UserProfile.CanAcknowledge = false;
            this.Save();
        }

        UserProfileService()
        {
            this.Load();
        }
    }

    public class UserProfile
    {
        public UserProfile()
        {
            this.Version = 1;
            this.IsChanged = false;
        }

        public void SetUnchanged()
        {
            this.IsChanged = false;
        }

        public bool IsChanged { get; private set; }
        
        public int Version { get; private set; }

        string name;
        public string Name 
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.IsChanged = true;
                }
            }
        }

        string email;
        public string Email 
        {
            get
            {
                return this.email; 
            }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.IsChanged = true;
                }
            }
        }

        public string Password { get; set; }
        public int? UserId { get; set; }
        public bool CanAcknowledge { get; set; }

       
    }
}
