using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Objectia.Api
{
    public class Usage
    {
        private Usage() {}

        public static async Task<APIUsage> GetAsync() 
        {
            var client = ObjectiaClient.GetRestClient();
            var data = await client.GetAsync("/v1/usage");
            return JsonConvert.DeserializeObject<APIUsage>(data);
        }
    }

    public class APIUsage 
    {
        [JsonProperty("geoip_requests")]
        public int GeoLocationRequests { get; set; }

        [JsonProperty("mail_requests")]
        public int MailRequests { get; set; }
    }
}
