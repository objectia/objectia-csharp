using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Net;
using System.IO;
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
    public class TestClient
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
        public void GetVersion()
        {
            var version = ObjectiaClient.Version;
            Assert.AreEqual(Constants.VERSION, version);
        }

        [TestMethod]
        public async Task GetUsage()
        {
            try
            {
                var usage = await Api.Usage.GetAsync();
                Assert.AreNotEqual(0, usage.Requests["geoip"]);
                Spew.Dump(usage);
            }
            catch (ResponseException ex)
            {
                Console.WriteLine("ResponseException: " + ex.Message + " - status: " + ex.Status.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task GetLocation()
        {
            var location = await Api.GeoLocation.GetAsync("8.8.8.8");
            Assert.AreEqual("US", location.CountryCode);
        }

        [TestMethod]
        public async Task GetLocationWithSecurityInfo()
        {
            var location = await Api.GeoLocation.GetAsync("8.8.8.8", null, false, true);
            Assert.AreEqual(false, location.Security.IsCrawler);
            Assert.AreEqual(true, location.Security.IsProxy);
            Assert.AreEqual("DCH", location.Security.ProxyType);
            Assert.AreEqual(false, location.Security.IsTOR);
        }

        [TestMethod]
        public async Task GetCurrentLocation()
        {
            var location = await Api.GeoLocation.GetCurrentAsync();
            Assert.AreEqual("LT", location.CountryCode);
        }

        [TestMethod]
        public async Task GetBulkLocation()
        {
            var locations = await Api.GeoLocation.GetBulkAsync(new String[] { "8.8.8.8", "apple.com" });
            Assert.AreEqual(2, locations.Count);
            foreach (var l in locations)
            {
                Assert.AreEqual("US", l.CountryCode);
            }
        }

        [TestMethod]
        public async Task GetLocationFail()
        {
            try
            {
                var location = await Api.GeoLocation.GetAsync("x");
                Assert.AreEqual("US", location.CountryCode);
            }
            catch (ResponseException e)
            {
                Assert.AreEqual(400, e.Status);
                Assert.AreEqual("err-invalid-ip", e.Code);
                Assert.AreEqual("Invalid IP address", e.Message);
            }
        }

        [TestMethod]
        public async Task SendMail()
        {
            var message = new MailMessage("ok@demo2.org", "Test", "This is a test", "ok@demo2.org", "okei@demo2.org");
            message.AddAttachment("/Users/otto/me.png");
            var receipt = await Api.Mail.SendAsync(message);
            Assert.IsNull(receipt.ID);
            Assert.AreNotEqual(1, receipt.AcceptedRecipients);
            Assert.AreEqual(0, receipt.RejectedRecipients);
        }

        [TestMethod]
        public async Task SendSMS()
        {
            var from = "Objectia";
            var to = Environment.GetEnvironmentVariable("MY_PHONE_NUMBER");
            var text = "This a test from C#";

            var receipt = await Api.SMS.SendAsync(from, to, text);
            Assert.AreEqual(from, receipt.From);
            Assert.AreEqual(to, receipt.To);
            Assert.AreEqual(text, receipt.Text);
        }

        [TestMethod]
        public async Task CreatePDF()
        {
            var options = new PDFOptions();
            options.DocumentHTML = "<html>This is a test from C# API client</html>";

            var buf = await Api.PDF.CreateAsync(options);
            Assert.IsNotNull(buf);

            File.WriteAllBytes("/tmp/pdf-sharp.pdf", buf);
        }
    }
}
