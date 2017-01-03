using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json;

namespace empfehlungsbund
{
    public class JSONSearchJobs
    {
        public int total_pages { get; set; }
        public int length { get; set; }
        public int current_page { get; set; }
        public List<Job> jobs { get; set; }
        public string spellcheck { get; set; }
    }

    public class Job
    {
        public int id { get; set; }
        public string title { get; set; }
        public string location { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string user_name { get; set; }
        public string company_logo { get; set; }
        public string company_logo_big { get; set; }
        public string preview { get; set; }
        public double score { get; set; }
        public double fk_score { get; set; }
        public int fid { get; set; }
        public string url { get; set; }
        public string original_url { get; set; }
        public string domain_name { get; set; }
        public List<string> tags { get; set; }
        public string pubDate { get; set; }
    }

    public class JSONReverseGeocomplete
    {
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public static class Api
    {
        public static async Task SearchJobs(string query, string location, Action<JSONSearchJobs> callback)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync("http://api.empfehlungsbund.de/api/v2/jobs/search.json?q=" + query + "&o=" + location);

            if (callback != null)
            {
                callback(JsonConvert.DeserializeObject<JSONSearchJobs>(json));
            }
        }

        public static async Task ReverseGeocode(double latitude, double longitude, Action<JSONReverseGeocomplete> callback)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync("http://api.empfehlungsbund.de/api/v2/utilities/reverse_geocomplete.json?api_key=API_KEY_REMOVED&lat=" + latitude.ToString(CultureInfo.InvariantCulture) + "&lon=" + longitude.ToString(CultureInfo.InvariantCulture));

            if (callback != null)
            {
                callback(JsonConvert.DeserializeObject<JSONReverseGeocomplete>(json));
            }
        }
    }
}
