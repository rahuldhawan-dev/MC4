using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Library.Configuration
{
    [TestClass]
    public class IncomingEmailConfigSectionTest : ConfigSectionTestBase
    {
        [TestMethod]
        public void TestValues()
        {
            Assert.AreEqual("127.0.0.1", _target.IncomingEmailConfig.Server);
            Assert.AreEqual("testing@mapcall.com", _target.IncomingEmailConfig.Username);
            Assert.AreEqual("password123", _target.IncomingEmailConfig.Password);
            Assert.AreEqual(143, _target.IncomingEmailConfig.Port);
            Assert.AreEqual(123, _target.IncomingEmailConfig.GapInterval);
            Assert.IsTrue(_target.IncomingEmailConfig.MakeChanges);
        }
    }
}