using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Objectia.Exceptions;

namespace Objectia.Api
{
    public class Mail : ApiBase
    {
        private Mail() { }

        /// <summary>
        /// Send a mail message async
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns></returns>
        public static async Task<MailReceipt> SendAsync(MailMessage message)
        {
            //check for parameters
            ThrowIf.IsArgumentNull(() => message);

            var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, ObjectiaClient.GetTimeout());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "objectia-csharp/" + Constants.VERSION);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ObjectiaClient.GetApiKey());
            client.DefaultRequestHeaders.Add("Accept", "application/pdf");

            var payload = message.ToContent();

            var response = await client.PostAsync(Constants.API_BASE_URL + "/v1/mail/send", payload);
            return await ParseResponse<MailReceipt>(response);
        }
    }

    public class MailReceipt
    {
        [JsonProperty("id")]
        public string ID { get; }

        [JsonProperty("accepted_recipients")]
        public int AcceptedRecipients { get; }

        [JsonProperty("rejected_recipients")]
        public int RejectedRecipients { get; }
    }


    public class MailMessage
    {
        ///
        /// Default constructor should not be used, hence private.
        ///
        private MailMessage() { }

        ///
        /// Constructor
        ///
        public MailMessage(string from, string subject, string text, params string[] to)
        {
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
        public bool? PlainTextClickTracking { get; set; }
        public bool? UnsubscribeTracking { get; set; }
        public bool? TestMode { get; set; }

        public void AddCc(params string[] cc)
        {
            this.Cc.AddRange(cc);
        }

        public void AddBcc(params string[] bcc)
        {
            this.Bcc.AddRange(bcc);
        }

        public void AddAttachment(string fileName)
        {
            if (this.Attachments.Count < 10)
            {
                this.Attachments.Add(fileName);
            }
        }

        public void AddTag(string tag)
        {
            if (this.Tags.Count < 3)
            {
                this.Tags.Add(tag);
            }
        }

        public MultipartFormDataContent ToContent()
        {
            var content = new MultipartFormDataContent();

            if (this.Date != null)
            {
                content.Add(new StringContent(this.Date.Value.ToString("o")), "date");
            }

            content.Add(new StringContent(this.From), "from");
            if (!string.IsNullOrEmpty(this.FromName))
            {
                content.Add(new StringContent(this.FromName), "from_name");
            }

            foreach (var v in this.To)
            {
                content.Add(new StringContent(v), "to");
            }
            foreach (var v in this.Cc)
            {
                content.Add(new StringContent(v), "cc");
            }
            foreach (var v in this.Bcc)
            {
                content.Add(new StringContent(v), "bcc");
            }

            content.Add(new StringContent(this.Subject), "subject");
            content.Add(new StringContent(this.Text), "text");
            if (!string.IsNullOrEmpty(this.HTML))
            {
                content.Add(new StringContent(this.HTML), "html");
            }

            foreach (var v in this.Tags)
            {
                content.Add(new StringContent(v), "tags");
            }

            if (!string.IsNullOrEmpty(this.Charset))
            {
                content.Add(new StringContent(this.Charset), "charset");
            }
            if (!string.IsNullOrEmpty(this.Encoding))
            {
                content.Add(new StringContent(this.Encoding), "encoding");
            }

            if (!string.IsNullOrEmpty(this.ReplyTo))
            {
                content.Add(new StringContent(this.ReplyTo), "reply_to");
            }

            // Attachments
            foreach (var fn in Attachments)
            {
                content.Add(new ByteArrayContent(File.ReadAllBytes(fn)), Path.GetFileName(fn), fn);
            }

            // Options
            if (this.RequireTLS != null)
            {
                content.Add(new StringContent(this.RequireTLS.Value.ToString()), "require_tls");
            }
            if (this.VerifyCertificate != null)
            {
                content.Add(new StringContent(this.VerifyCertificate.Value.ToString()), "verify_cert");
            }
            if (this.OpenTracking != null)
            {
                content.Add(new StringContent(this.OpenTracking.Value.ToString()), "open_tracking");
            }
            if (this.ClickTracking != null)
            {
                content.Add(new StringContent(this.ClickTracking.Value.ToString()), "click_tracking");
            }
            if (this.PlainTextClickTracking != null)
            {
                content.Add(new StringContent(this.PlainTextClickTracking.Value.ToString()), "text_click_tracking");
            }
            if (this.UnsubscribeTracking != null)
            {
                content.Add(new StringContent(this.UnsubscribeTracking.Value.ToString()), "unsubscribe_tracking");
            }
            if (this.TestMode != null)
            {
                content.Add(new StringContent(this.TestMode.Value.ToString()), "test_mode");
            }

            return content;
        }
    }
}
