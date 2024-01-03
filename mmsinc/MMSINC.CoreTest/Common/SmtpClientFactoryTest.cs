using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Common;

namespace MMSINC.CoreTest.Common
{
    [TestClass]
    public class SmtpClientFactoryTest
    {
        [TestMethod]
        public void TestBuildAlwaysReturnsANewInstance()
        {
            var target = new SmtpClientFactory();
            var result1 = target.Build();
            var result2 = target.Build();
            Assert.AreNotSame(result1, result2);
        }
    }
}
