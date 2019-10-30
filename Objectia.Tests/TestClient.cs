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
        public async Task GetUsage()
        {
            var usage = await Api.Usage.GetAsync(); 
            Assert.AreNotEqual(0, usage.GeoLocationRequests);
        }

        [TestMethod]
        public async Task GetLocation()
        {
            var location = await Api.GeoLocation.GetAsync("8.8.8.8"); 
            Assert.AreEqual("US", location.CountryCode);
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
            var locations = await Api.GeoLocation.GetBulkAsync(new String[]{"8.8.8.8", "apple.com"}); 
            Assert.AreEqual(2, locations.Count);
            foreach (var l in locations) {
                Assert.AreEqual("US", l.CountryCode);
            }
        }
    }
}
