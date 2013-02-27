using Newtonsoft.Json;
using SeeClickFix.WP8.SeeClickFixAPI;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Services
{
    public class AppSettingsService
    {
        public static AppSettingsService Inst = new AppSettingsService();

        public event EventHandler<ApplicationSettings> Saved = null;

        public ApplicationSettings Settings
        {
            get;
            set;
        }

        AppSettingsService()
        {
            this.Load();
        }

        public void Save()
        {
            IsolatedStorageSettings.ApplicationSettings["settings"] = JsonConvert.SerializeObject(this.Settings);
            IsolatedStorageSettings.ApplicationSettings.Save();

            if (this.Saved != null)
            {
                this.Saved(this, this.Settings);
            }
        }

        public void Load()
        {
            string settings;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("settings", out settings))
            {
                this.Settings = JsonConvert.DeserializeObject<ApplicationSettings>(settings);
            }
            else
            {
                this.Settings = new ApplicationSettings();
            }
        }
    }

    public class ApplicationSettings
    {
        public ApplicationSettings()
        {
            this.Version = 1;
            this.UseLocationServices = true;
            this.WasLocationServicesConsentDisplayed = false;
        }

        public int Version { get; set; }
        public bool UseLocationServices { get; set; }
        public bool WasLocationServicesConsentDisplayed { get; set; }
        public Location Location { get; set; }
    }

    public class Location 
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public Coordinate Position { get; set; }
    }
}
