using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace Objectia.Api
{
    public class PDF
    {
        private PDF() { }

        public static async Task<byte[]> CreateAsync(PDFOptions options)
        {
            var client = ObjectiaClient.GetRestClient();
            var resp = await client.PostAsync("/v1/pdf/create", options.ToHttpContent());
            return JsonConvert.DeserializeObject<byte[]>(resp);
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

        public HttpContent ToHttpContent()
        {
            var jsonObject = new JObject();

            if (!string.IsNullOrEmpty(this.DocumentURL))
            {
                jsonObject.Add(new JProperty("document_url", this.DocumentURL));
            }
            else
            {
                jsonObject.Add(new JProperty("document_html", this.DocumentHTML));
            }

            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

            return content;
        }
    }
}