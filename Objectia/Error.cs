using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Objectia
{
    public class Error
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        public Error()
        {
        }

        public static Error FromJSON(string json)
        {
            return JsonConvert.DeserializeObject<Error>(json);
        }
    }
}
