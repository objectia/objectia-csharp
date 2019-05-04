using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

using Objectia;

namespace Objectia.Api
{
    public class GeoLocation
    {
        [JsonProperty("country_name")]
        public string Country { get; set; }
        
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        private GeoLocation() {}

        public static async Task<GeoLocation> Get(string ip, string fields=null, bool hostname=false, bool security=false) 
        {
            var client = ObjectiaClient.GetRestClient();
            var data = await client.Get("/geoip/" + ip);
            return JsonConvert.DeserializeObject<GeoLocation>(data);
        }

        public static async Task<GeoLocation> GetCurrent(string fields=null, bool hostname=false, bool security=false) 
        {
            return await GeoLocation.Get("myip", fields, hostname, security);
        }

        public static async Task<List<GeoLocation>> GetBulk(string[] ipList, string fields=null, bool hostname=false, bool security=false) 
        {
            var param = String.Join(",",ipList);
            var client = ObjectiaClient.GetRestClient();
            var data = await client.Get("/geoip/" + param);
            return JsonConvert.DeserializeObject<List<GeoLocation>>(data);
        }

    }
}
