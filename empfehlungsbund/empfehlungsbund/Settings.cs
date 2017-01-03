using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace empfehlungsbund
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        private const string queryKey = "query";
        private static readonly string queryDefault = string.Empty;

        private const string locationKey = "location";
        private static readonly string locationDefault = string.Empty;

        public static string query
        {
            get { return AppSettings.GetValueOrDefault<string>(queryKey, queryDefault); }
            set { AppSettings.AddOrUpdateValue<string>(queryKey, value); }
        }

        public static string location
        {
            get { return AppSettings.GetValueOrDefault<string>(locationKey, locationDefault); }
            set { AppSettings.AddOrUpdateValue<string>(locationKey, value); }
        }

        public static List<int> ignoredJobs { get; set; } = new List<int>();
    }
}
