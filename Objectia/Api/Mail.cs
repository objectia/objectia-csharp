using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

using Objectia;
using Objectia.Exceptions;

namespace Objectia.Api
{
    public class Mail
    {
        private Mail() {}

        /// <summary>
        /// Send a message async
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns></returns>
        public static async Task<MailReceipt> SendAsync(Message message) 
        {
            //check for parameters
            ThrowIf.IsArgumentNull(() => message);

            var client = ObjectiaClient.GetRestClient();
            var resp = await client.PostAsync("/v1/mail/send", message.AsFormContent()); 
            return JsonConvert.DeserializeObject<MailReceipt>(resp);
        }

        public static async Task<MailReceipt> Test() 
        {
            var client = ObjectiaClient.GetRestClient();

            var data = new JObject(
                new JProperty("name", "Joey User")
            );
            var content = new StringContent(data.ToString(Formatting.None), Encoding.UTF8, "application/json"); 

            var resp = await client.PostAsync("/v1/test", content); 
            return JsonConvert.DeserializeObject<MailReceipt>(resp);
        }

    }

    public class MailReceipt
    {
        [JsonProperty("id")]
        public string ID { get; }
        
        [JsonProperty("accepted_recipients")]
        public int AcceptedRecipients { get;  }
        
        [JsonProperty("rejected_recipients")]
        public int RejectedRecipients { get;  }
    }


    public class Message {
        ///
        /// Default constructor should not be used, hence private.
        ///
        private Message() {}

        ///
        /// Constructor
        ///
        public Message(string from, string subject, string text, params string[] to) {
            this.From = from;
            this.Subject = subject;
            this.Text = text;
            this.To = new List<string>(to);
            this.Cc = new List<string>();
            this.Bcc = new List<string>();
            this.Tags = new List<string>();
            this.Attachments = new List<string>();
        }

        public DateTime? Date { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string HTML { get; set; }
        public List<string> Attachments { get; set; }
        public List<string> Tags { get; set; }
        public string Charset { get; set; }
        public string Encoding { get; set; }
        
        /// Options
        public bool? RequireTLS { get; set; }
        public bool? VerifyCertificate { get; set; }
        public bool? OpenTracking { get; set; }
        public bool? ClickTracking { get; set; }
        public bool? HTMLOnlyClickTracking { get; set; }
        public bool? UnsubscribeTracking { get; set; }
        public bool? TestMode { get; set; }

        public void AddCc(params string[] cc) {
            this.Cc.AddRange(cc);
        }

        public void AddBcc(params string[] bcc) {
            this.Bcc.AddRange(bcc);
        }

        public void AddAttachment(string fileName) {
            if (this.Attachments.Count < 10) {
                this.Attachments.Add(fileName);
            }
        }

        public void AddTag(string tag) {
            if (this.Tags.Count < 3) {
                this.Tags.Add(tag);
            }
        }

        public HttpContent AsFormContent() {
            var content = new MultipartFormDataContent();

            if (this.Date != null) {
                content.Add(new StringContent(this.Date.Value.ToString("o")), "date"); 
            }

            content.Add(new StringContent(this.From), "from");
            if (!string.IsNullOrEmpty(this.FromName)) {
                content.Add(new StringContent(this.FromName), "from_name");
            }
            
            content.Add(new StringContent(string.Join(",", this.To)), "to");
            if (this.Cc.Count > 0) {
                content.Add(new StringContent(string.Join(",", this.Cc)), "cc");
            }
            if (this.Bcc.Count > 0) {
                content.Add(new StringContent(string.Join(",", this.Bcc)), "bcc");
            }

            content.Add(new StringContent(this.Subject), "subject");
            content.Add(new StringContent(this.Text), "text");
            if (!string.IsNullOrEmpty(this.HTML)) {
                content.Add(new StringContent(this.HTML), "html");
            }

            if (this.Tags.Count > 0) {
                content.Add(new StringContent(string.Join(",", this.Tags)), "tags");
            }
            
            if (!string.IsNullOrEmpty(this.Charset)) {
                content.Add(new StringContent(this.Charset), "charset");
            }
            if (!string.IsNullOrEmpty(this.Encoding)) {
                content.Add(new StringContent(this.Encoding), "encoding");
            }

            if (!string.IsNullOrEmpty(this.ReplyTo)) {
                content.Add(new StringContent(this.ReplyTo), "reply_to");
            }

            // Attachments
            foreach (var fn in Attachments)
            {
                content.Add(new ByteArrayContent(File.ReadAllBytes(fn)), Path.GetFileName(fn), fn);
            }

            // Options
            if (this.RequireTLS != null) {
                content.Add(new StringContent(this.RequireTLS.Value.ToString()), "require_tls");
            }
            if (this.VerifyCertificate != null) {
                content.Add(new StringContent(this.VerifyCertificate.Value.ToString()), "verify_cert");
            }
            if (this.OpenTracking != null) {
                content.Add(new StringContent(this.OpenTracking.Value.ToString()), "open_tracking");
            }
            if (this.ClickTracking != null) {
                content.Add(new StringContent(this.ClickTracking.Value.ToString()), "click_tracking");
            }
            if (this.HTMLOnlyClickTracking != null) {
                content.Add(new StringContent(this.HTMLOnlyClickTracking.Value.ToString()), "html_click_tracking");
            }
            if (this.UnsubscribeTracking != null) {
                content.Add(new StringContent(this.UnsubscribeTracking.Value.ToString()), "unsubscribe_tracking");
            }
            if (this.TestMode != null) {
                content.Add(new StringContent(this.TestMode.Value.ToString()), "test_mode");
            }

            return content;
        }
    }
}
