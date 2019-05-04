using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

using Objectia.Exceptions;

namespace Objectia
{
    public class RestClient
    {
        #region --- Properties ---        

        protected int _timeout = Constants.DEFAULT_TIMEOUT;

        public string ApiKey { get; private set; }

        public string ApiBaseUrl { get; set; }
        
        public int Timeout
        {
            get { return _timeout; }
            set
            {
                if (value < 1 || value > 180)
                {
                    throw new ArgumentException("Timeout must be within 1 and 180 seconds");
                }
                _timeout = value;
            }
        }
        public string UserAgent { get; private set; }

        #endregion


        ///
        /// Constructor.
        ///
        public RestClient(string apiKey, int? timeout)
        {
            this.ApiBaseUrl = Constants.API_BASE_URL;
            this.UserAgent = "objectia-csharp/" + Constants.VERSION;

            this.ApiKey = apiKey;

            if (timeout.HasValue) {
                this.Timeout = timeout.Value;
            }

            if (string.IsNullOrEmpty(this.ApiKey))
            {
                throw new ArgumentException("No API key provided");
            }
        }


        public async Task<string> Get(string path)
        {
            return await Execute("GET", path);
        }

        public async Task<string> Post(string path, JObject payload)
        {
            return await Execute("POST", path, payload);
        }

        public async Task<string> Put(string path, JObject payload)
        {
            return await Execute("PUT", path, payload);
        }
        public async Task<string> Patch(string path, JObject payload)
        {
            return await Execute("PATCH", path, payload);
        }

        public async Task<string> Delete(string path)
        {
            return await Execute("DELETE", path);
        }

        protected async Task<string> Execute(string method, string path, JObject data = null)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 0, this.Timeout);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", this.UserAgent);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.ApiKey);

                StringContent payload = null;
                if (data != null)
                {
                    payload = new StringContent(data.ToString(Formatting.None), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = null;
                switch (method)
                {
                    case "GET":
                        response = await client.GetAsync(this.ApiBaseUrl + path);
                        break;
                    case "POST":
                        response = await client.PostAsync(this.ApiBaseUrl + path, payload);
                        break;
                    case "PUT":
                        response = await client.PutAsync(this.ApiBaseUrl + path, payload);
                        break;
                    case "DELETE":
                        response = await client.DeleteAsync(this.ApiBaseUrl + path);
                        break;
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.Content != null)
                    {
                        response.Content.Dispose();
                    }

                    JObject obj = JObject.Parse(content);
                    var result = obj["data"].ToString();

                    return result;    
                }
                else
                {
                    int statusCode = (int)response.StatusCode;
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.Content != null)
                    {
                        response.Content.Dispose();
                    }
                    throw new ResponseException(Error.FromJSON(content));
                }
            }
            catch (HttpRequestException ex)
            {
                throw new APIConnectionException(ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                throw new APITimeoutException(ex.Message);
            }
        }
    }
}
