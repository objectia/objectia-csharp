using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

using Objectia.Exceptions;

namespace Objectia.Api
{
    public class Usage : ApiBase
    {
        private Usage() { }

        public static async Task<APIUsage> GetAsync()
        {
            var response = await GetJSON("/v1/usage");
            return await ParseResponse<APIUsage>(response);
        }

        public class APIUsage
        {
            [JsonProperty("requests")]
            public Dictionary<string, int> Requests { get; set; }

            [JsonProperty("cost")]
            public Dictionary<string, float> Cost { get; set; }
        }
    }
}