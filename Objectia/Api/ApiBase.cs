using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Objectia.Exceptions;

namespace Objectia.Api
{
    abstract public class ApiBase
    {
        public ApiBase() { }

        protected static async Task<HttpResponseMessage> GetJSON(string path)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 0, ObjectiaClient.GetTimeout());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "objectia-csharp/" + Constants.VERSION);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ObjectiaClient.GetApiKey());
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                return await client.GetAsync(Constants.API_BASE_URL + path);
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

        protected static async Task<HttpResponseMessage> PostJSON(string path, HttpContent payload)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 0, ObjectiaClient.GetTimeout());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "objectia-csharp/" + Constants.VERSION);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ObjectiaClient.GetApiKey());
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                return await client.PostAsync(Constants.API_BASE_URL + path, payload);
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

        protected static async Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (response.Content != null)
                {
                    response.Content.Dispose();
                }

                JObject obj = JObject.Parse(content);
                string data = string.Empty;
                if (obj["data"] != null)
                {
                    data = obj["data"].ToString();
                }
                var result = JsonConvert.DeserializeObject<T>(data);
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

                Error error = null;
                try
                {
                    // Try to parse JSON
                    error = Error.FromJSON(content);
                }
                catch
                {
                    throw new APIConnectionException(content);
                }

                throw new ResponseException(error);
            }
        }


        protected static async Task<byte[]> GetRawBody(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                if (response.Content != null)
                {
                    response.Content.Dispose();
                }
                return content;
            }
            else
            {
                int statusCode = (int)response.StatusCode;
                var content = await response.Content.ReadAsStringAsync();
                if (response.Content != null)
                {
                    response.Content.Dispose();
                }

                Error error = null;
                try
                {
                    // Try to parse JSON
                    error = Error.FromJSON(content);
                }
                catch
                {
                    throw new APIConnectionException(content);
                }

                throw new ResponseException(error);
            }
        }

    }
}