using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ServiceSizeTest
    {
        [TestMethod]
        public void TestDescriptionReturnsSize()
        {
            var target = new ServiceSize();
            target.ServiceSizeDescription = "what";
            Assert.AreEqual("what", target.Description);
        }

        [TestMethod]
        public void TestToStringReturnsSize()
        {
            var target = new ServiceSize();
            target.ServiceSizeDescription = "what";
            Assert.AreEqual("what", target.ToString());
        }
    }
}
