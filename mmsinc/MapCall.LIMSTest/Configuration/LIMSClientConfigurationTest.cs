using MapCall.LIMS.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCall.LIMSTest.Configuration
{
    [TestClass]
    public class LIMSClientConfigurationTest
    {
        #region Private Members

        private LIMSClientConfiguration _limsConfiguration;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _limsConfiguration = _container.GetInstance<LIMSClientConfiguration>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestBuildsApiUrlFromConfiguration()
        {
            Assert.AreEqual("http://some-domain.com/api/test-service", _limsConfiguration.ApiUri.ToString());
        }

        #endregion
    }
}
