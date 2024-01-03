using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallScheduler.Tests
{
    [TestClass]
    public class MapCallSchedulerConfigurationTest
    {
        #region Private Members

        private MapCallSchedulerConfiguration _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _target = _container.GetInstance<MapCallSchedulerConfiguration>();
        }

        #endregion

        [TestMethod]
        public void TestGetsStartTimeFromConfiguration()
        {
            Assert.AreEqual(_target.StartTime, ConfigurationManager.AppSettings[MapCallSchedulerConfiguration.ConfigKeys.START_TIME]);
        }

        [TestMethod]
        public void TestGetsAllEmailsGoToFromConfiguration()
        {
            Assert.AreEqual(_target.AllEmailsGoTo, ConfigurationManager.AppSettings[MapCallSchedulerConfiguration.ConfigKeys.ALL_EMAILS_GO_TO]);
        }

        [TestMethod]
        public void TestGetsMaximumConcurrentRequestsFromConfiguration()
        {
            Assert.AreEqual(_target.MaximumConcurrentRequests, int.Parse(ConfigurationManager.AppSettings[MapCallSchedulerConfiguration.ConfigKeys.MAXIMUM_CONCURRENT_REQUESTS]));
        }

        [TestMethod]
        public void TestMaximumRowsGetsMaximumRowsFromConfiguration()
        {
            Assert.AreEqual(_target.MaximumRows, int.Parse(ConfigurationManager.AppSettings[MapCallSchedulerConfiguration.ConfigKeys.MAXIMUM_ROWS]));
        }

        [TestMethod]
        public void TestApiKeyMatchesCurrentConfiguration()
        {
            // This falsely passed because NULL = NULL so testing that the API key is also not null
            Assert.IsNotNull(ConfigurationManager.AppSettings[MapCallSchedulerConfiguration.ConfigKeys.API_KEY]);
            Assert.AreEqual(_target.APIKey, ConfigurationManager.AppSettings[MapCallSchedulerConfiguration.ConfigKeys.API_KEY]);
        }

        [TestMethod]
        public void TestApiUrlMatchesCurrentConfiguration()
        {
            // Same as the above test, never added to app.config so it falsely passed
            Assert.IsNotNull(ConfigurationManager.AppSettings[MapCallSchedulerConfiguration.ConfigKeys.API_URL]);
            Assert.AreEqual(_target.APIURL, ConfigurationManager.AppSettings[MapCallSchedulerConfiguration.ConfigKeys.API_URL]);
        }

        [TestMethod]
        public void TestGetIsProductionReturnsValueFromConfiguration()
        {
            Assert.IsFalse(_target.IsProduction);
        }

        [TestMethod]
        public void TestGetIsStagingReturnsValueFromConfiguration()
        {
            Assert.IsFalse(_target.IsStaging);
        }
    }
}
