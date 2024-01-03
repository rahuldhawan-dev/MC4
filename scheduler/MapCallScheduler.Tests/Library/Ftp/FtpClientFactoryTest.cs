using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Ftp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Library.Ftp
{
    [TestClass]
    public class FtpClientFactoryTest
    {
        #region Private Members

        private FtpClientFactory _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _target = _container.GetInstance<FtpClientFactory>();
        }

        #endregion

        [TestMethod]
        public void TestFromConfigReturnsClientFromConfigWithAppropriateSettings()
        {
            var host = "host";
            var user = "user";
            var password = "password";
            var config = new Mock<IFtpConfigSection>();
            config.SetupGet(c => c.Host).Returns(host);
            config.SetupGet(c => c.User).Returns(user);
            config.SetupGet(c => c.Password).Returns(password);

            var client = _target.FromConfig(config.Object);

            Assert.AreEqual(host, client.Host);
            Assert.AreEqual(user, client.Credentials.UserName);
            Assert.AreEqual(password, client.Credentials.Password);
        }
    }
}
