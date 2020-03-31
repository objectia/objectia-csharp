using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

using Objectia.Exceptions;

namespace Objectia.Api
{
    public class PDF : ApiBase
    {
        private PDF() { }

        public static async Task<byte[]> CreateAsync(PDFOptions options)
        {
            //check for parameters
            ThrowIf.IsArgumentNull(() => options);

            var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, ObjectiaClient.GetTimeout());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "objectia-csharp/" + Constants.VERSION);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ObjectiaClient.GetApiKey());
            client.DefaultRequestHeaders.Add("Accept", "application/pdf");

            var payload = options.ToContent();

            var response = await client.PostAsync(Constants.API_BASE_URL + "/v1/pdf/create", payload);
            return await GetRawBody(response);



            /*            var client = new RestClient(Constants.API_BASE_URL);
                        client.Timeout = ObjectiaClient.GetTimeout() * 1000;
                        client.UseJson();

                        var request = new RestRequest("/v1/pdf/create", Method.POST);
                        request.AddHeader("Authorization", "bearer " + ObjectiaClient.GetApiKey());
                        request.AddHeader("User-Agent", "objectia-csharp/" + Constants.VERSION);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Accept", "application/pdf");

                        var payload = options.ToJSON();
                        request.AddJsonBody(payload.ToString());

                        var response = await client.ExecuteAsync(request);
                        return GetRawBody(response);*/
        }
    }

    public class PDFOptions
    {
        ///
        /// Constructor
        ///
        public PDFOptions() { }

        public string DocumentURL { get; set; }
        public string DocumentHTML { get; set; }

        public HttpContent ToContent()
        {
            var jsonObject = new JObject();

            if (!string.IsNullOrEmpty(this.DocumentURL))
            {
                jsonObject.Add(new JProperty("document_url", this.DocumentURL));
            }
            else if (!string.IsNullOrEmpty(this.DocumentHTML))
            {
                jsonObject.Add(new JProperty("document_html", this.DocumentHTML));
            }

            //FIXME: add all options

            return new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
        }
    }
}