using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Objectia.Core.Messages
{
    public interface IMessage
    {
        JObject RecipientVariables { get; set; }

        /// <summary>
        /// Email address for From header
        /// </summary> 
        IRecipient From { get; set; }

        /// <summary>
        /// Email address of the recipient(s).
        /// </summary>
        ICollection<string> To { get; set; }

        /// <summary>
        /// Same as To but for Cc
        /// </summary>
        ICollection<string> Cc { get; set; }

        /// <summary>
        /// Same as To but for Bcc
        /// </summary>
        ICollection<string> Bcc { get; set; }

        /// <summary>
        /// Message subject
        /// </summary>
        string Subject { get; set; }

        /// <summary>
        /// Body of the message. (text version)
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Body of the message. (HTML version)
        /// </summary>
        string Html { get; set; }

        /// <summary>
        /// File attachments
        /// </summary>
        ICollection<FileInfo> Attachments { get; set; }

        /// <summary>
        /// Alternate format file attachments.
        /// </summary>
        ICollection<IFileAttachment> FileAttachments { get; set; }

        /// <summary>
        /// Images to inline
        /// </summary>
        ICollection<FileInfo> Inline { get; set; }

        /// <summary>
        /// A collection of tag strings. See http://documentation.mailgun.com/user_manual.html#tagging for more examples
        /// </summary>
        ICollection<string> Tags { get; set; }

        /// <summary>
        /// Enables sending in test mode.
        /// </summary>
        bool TestMode { get; set; }

        /// <summary>
        /// Toggles tracking on a per-message basis
        /// </summary>
        bool Tracking { get; set; }

        /// <summary>
        /// Toggles clicks tracking on a per-message basis
        /// </summary>
        bool TrackingClicks { get; set; }

        /// <summary>
        /// Toggles opens tracking on a per-message basis
        /// </summary>
        bool TrackingOpen { get; set; }

        /// <summary>
        /// Name and arbitrary JSON data to add to a message e.g {"customData","{\"myCustomKey\":\"someValue\"}"}
        /// </summary>
        IDictionary<string, JObject> CustomData { get; set; }

        /// <summary>
        /// Name and string values to add as a custom header. e.g. {"X-My-Header","SomeCustomerHeaderValue"}
        /// </summary>
        IDictionary<string, string> CustomHeaders { get; set; }

        /// <summary>
        /// Name and string values to add as a custom parameters.
        /// </summary>
        IDictionary<string, string> CustomParameters { get; set; }

        /// <summary>
        /// Get this message as FormUrlEncodedContent
        /// </summary>
        /// <returns></returns>
        HttpContent AsFormContent();

        /// <summary>
        /// Get this message as a collection of key value pairs in the correct Mailgun format
        /// </summary>
        /// <returns></returns>
        ICollection<KeyValuePair<string, string>> AsKeyValueCollection();
    }
}