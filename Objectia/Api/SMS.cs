using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace Objectia.Api
{
    public class SMS : ApiBase
    {
        private SMS() { }

        public static async Task<SMSReceipt> SendAsync(string from, string to, string text)
        {
            var jsonObject = new JObject();
            jsonObject.Add(new JProperty("from", from));
            jsonObject.Add(new JProperty("to", to));
            jsonObject.Add(new JProperty("text", text));

            var payload = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

            var response = await PostJSON("/v1/sms/send", payload);
            return await ParseResponse<SMSReceipt>(response);
        }
    }

    public class SMSReceipt
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }
    }
}
