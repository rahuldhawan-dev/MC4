using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class SecureAuthHttpClientTest
    {
        private IContainer _container;
        private Mock<ISecureAuthClientFactory> _secureAuthClientFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _secureAuthClientFactory = new Mock<ISecureAuthClientFactory>();
            _container.Inject((_secureAuthClientFactory = new Mock<ISecureAuthClientFactory>()).Object);
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationEndpointBaseUrl"] =
                "AuthenticationEndpointBaseUrl";
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationClientId"] = "AuthenticationClientId";
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationClientSecret"] = "AuthenticationClientSecret";
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationUsername"] = "AuthenticationUsername";
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationPassword"] = "AuthenticationPassword";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationEndpointBaseUrl"] = null;
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationClientId"] = null;
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationClientSecret"] = null;
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationUsername"] = null;
            ConfigurationManager.AppSettings["SecureAuth-AuthenticationPassword"] = null;
        }

        [TestMethod]
        public void TestConstructorSetsSecureAuthClientProperties()
        {
            var target = new SecureAuthHttpClient(_secureAuthClientFactory.Object);

            _secureAuthClientFactory.Verify(x => x
               .Build(It.Is<SecureAuthClientConfiguration>(c =>
                    c.BaseEndpointUrl == "AuthenticationEndpointBaseUrl"
                    && c.ClientId == "AuthenticationClientId"
                    && c.ClientSecret == "AuthenticationClientSecret"
                    && c.Username == "AuthenticationUsername"
                    && c.Password == "AuthenticationPassword"
                )));
        }
    }
}
