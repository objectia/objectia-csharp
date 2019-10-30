using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Net;
using System.Net.Security;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Objectia;
using Objectia.Api;
using Objectia.Exceptions;

namespace Objectia.Tests
{
    [TestClass]
    public class TestMail
    {
        string apiKey;

        [TestInitialize]
        public void SetUp()
        {
            apiKey = Environment.GetEnvironmentVariable("OBJECTIA_APIKEY");
            ObjectiaClient.Init(apiKey);
        }

        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public async Task SendMail()
        {
            var message = new Message("ok@demo2.org", "Test", "This is a test", "ok@demo2.org");
            message.AddAttachment("/Users/otto/me.png");
            var receipt = await Api.Mail.SendAsync(message); 
            Assert.IsNull(receipt.ID);
            Assert.AreNotEqual(1, receipt.AcceptedRecipients);
            Assert.AreEqual(0, receipt.RejectedRecipients);
        }
    }
}
