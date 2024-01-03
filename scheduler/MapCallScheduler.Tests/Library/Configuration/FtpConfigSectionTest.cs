using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Library.Configuration
{
    [TestClass]
    public class FtpConfigSectionTest : ConfigSectionTestBase
    {
        [TestMethod]
        public void TestValues()
        {
            Assert.AreEqual("127.0.0.1", _target.FtpConfig.Host);
            Assert.AreEqual("test", _target.FtpConfig.User);
            Assert.AreEqual("password123", _target.FtpConfig.Password);
            Assert.IsFalse(_target.FtpConfig.MakeChanges);
            Assert.AreEqual(".", _target.FtpConfig.WorkingDirectory);
        }
    }
}