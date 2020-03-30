using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

using RestSharp;
using RestSharp.Serialization.Json;
using RestSharp.Authenticators;

namespace Objectia.Api
{
    public class Usage
    {
        private Usage() { }

        public static async Task<APIUsage> GetAsync()
        {
            var client = new RestClient("https://api.objectia.com");
            client.Timeout = 30000; // 30 seconds  //FIXME

            var request = new RestRequest("/v1/usage", Method.GET);
            request.AddHeader("Authorization", "bearer " + "9f9a09db8f664c5ea897fdf4599dde03"); //FIXME
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            var response = await client.ExecuteGetAsync(request);
            if (response.ErrorException != null)
            {
                throw new Exception(); //FIXME ---> parse response....
            }

            JObject obj = JObject.Parse(response.Content);
            string data = string.Empty;
            if (obj["data"] != null)
            {
                data = obj["data"].ToString();
            }

            var result = JsonConvert.DeserializeObject<APIUsage>(data);

            return result;
        }

        /*        public static async Task<APIUsage> GetAsync()
                {
                    var client = ObjectiaClient.GetRestClient();
                    var resp = await client.GetAsync("/v1/usage");
                    return JsonConvert.DeserializeObject<APIUsage>(resp);
                }
            }*/

        public class APIUsage
        {
            [JsonProperty("requests")]
            public Dictionary<string, int> Requests { get; set; }

            [JsonProperty("cost")]
            public Dictionary<string, float> Cost { get; set; }
        }
    }
}