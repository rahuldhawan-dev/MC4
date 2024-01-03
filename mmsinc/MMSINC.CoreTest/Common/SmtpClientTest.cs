using System.Net.Mail;
using MMSINC.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Common
{
    /// <summary>
    /// Summary description for SmtpClientTest
    /// </summary>
    [TestClass]
    public class SmtpClientWrapperTest
    {
        [TestMethod]
        public void TestConstructorCreatesInnerSmtpClient()
        {
            var target = new TestSmtpClientWrapper();

            Assert.IsInstanceOfType(target.Client, typeof(SmtpClient));
        }
    }

    public class TestSmtpClientWrapper : SmtpClientWrapper
    {
        #region Properties

        public SmtpClient Client
        {
            get { return _client; }
        }

        #endregion
    }
}
