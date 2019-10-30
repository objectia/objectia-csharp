using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

using Objectia;

namespace Objectia.Api
{
    public class Usage
    {
        [JsonProperty("geoip_requests")]
        public int GeoLocationRequests { get; set; }

        private Usage() {}

        public static async Task<Usage> GetAsync() 
        {
            var client = ObjectiaClient.GetRestClient();
            var data = await client.GetAsync("/v1/usage");
            return JsonConvert.DeserializeObject<Usage>(data);
        }
    }
}
