using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MMSINC.CoreTest.Utilities
{
    /// <summary>
    /// It's very easy to confuse SecureAuthClient here with SecureAuthHTTPClient.
    /// </summary>
    [TestClass]
    public class SecureAuthHttpClientFactoryTest
    {
        #region Private Members

        private IContainer _container;
        private Mock<ISecureAuthClientFactory> _secureAuthClientFactory;
        private Mock<ISecureAuthClient> _secureAuthClient;

        #endregion

        [TestMethod]
        public void TestBuildCallsClientInitialize()
        {
            var accessToken = "ThisIsMyAccessTokenThereAreManyLikeItButThisOneIsMine";
            _secureAuthClientFactory.Setup(x => x.Build(It.IsAny<SecureAuthClientConfiguration>()))
                                    .Returns(_secureAuthClient.Object);
            _secureAuthClient.Setup(x => x.GetAccessToken())
                             .Returns(new SecureAuthAccessToken {AccessToken = accessToken});
            var settings = new SecureAuthHttpClientSettings {
                Url = "http://foo.bar/",
                Timeout = 42
            };

            var target = new SecureAuthHttpClientFactory(_container);
            var result = target.Build(settings);

            Assert.AreEqual(settings.Url, result.BaseAddress.AbsoluteUri);
            Assert.AreEqual(TimeSpan.FromMinutes(42), result.Timeout);
            var requestHeader = result.DefaultRequestHeaders.FirstOrDefault();
        }

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _secureAuthClientFactory = new Mock<ISecureAuthClientFactory>();
            _container.Inject((_secureAuthClientFactory = new Mock<ISecureAuthClientFactory>()).Object);
            _container.Inject((_secureAuthClient = new Mock<ISecureAuthClient>()).Object);
        }

        [TestCleanup]
        public void TestCleanup() { }

        #endregion
    }
}
