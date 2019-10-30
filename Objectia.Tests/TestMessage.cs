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
    public class TestMessage
    {
        [TestInitialize]
        public void SetUp()
        {
        }

        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public void Message()
        {
            var message = new Message("otto@doseth.com", "Test", "This is a test", "otto@doseth.com");
            Assert.AreEqual("otto@doseth.com", message.From);
            Assert.AreEqual("Test", message.Subject);
            Assert.AreEqual("This is a test", message.Text);
            CollectionAssert.Contains(message.To, "otto@doseth.com");

            Assert.AreEqual(null, message.RequireTLS);

            message.RequireTLS = true;
            Assert.AreEqual(true, message.RequireTLS);

            message.RequireTLS = false;
            Assert.AreEqual(false, message.RequireTLS);

            message.RequireTLS = null;
            Assert.AreEqual(null, message.RequireTLS);

            var fc = message.AsFormContent();
            Console.WriteLine(fc);

        }
    }
}
