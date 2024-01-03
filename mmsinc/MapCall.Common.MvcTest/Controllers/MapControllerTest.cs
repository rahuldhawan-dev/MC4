using System;
using System.Configuration;
using System.Linq;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using StructureMap;
using UserType = MMSINC.Testing.UserType;

namespace MapCall.Common.MvcTest.Controllers
{
    [TestClass]
    public class MapControllerTest : ControllerTestBase<FakeMvcApplication, MapController, MapIcon>
    {
        #region Fields

        private FakeMvcApplicationTester _application;
        private MapView _indexModel;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void MapControllerTestInitialize()
        {
            _application = new FakeMvcApplicationTester(_container);
            _indexModel = new MapView {
                ControllerName = "Controller",
                ActionName = "Index"
            };
        }

        [TestCleanup]
        public void MapControllerTestCleanup()
        {
            _application.Dispose();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(MapIconFactory).Assembly);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            // This is a bit of a hack to use _application instead of Application.
            _application.ControllerFactory.RegisterController("Map", _target);

            // Because MapControllerTest is stuck using FakeMvcApplication, which
            // doesn't have any of the default authorization filters added to it,
            // we need to stick a filter in for this test so the tests don't all fail
            // for thinking we're allowing anonymous access. The proper fix for this
            // would be to add the filter to FakeMvcApplication but that causes all
            // sorts of unrelated tests to start failing. I don't have time to deal 
            // with fixing all of those at the moment. So make sure all of the filter
            // configuration stuff is cleaned up at the end of this test.
            var authFilter = new MvcAuthorizationFilter(_container);
            _application.Filters.GlobalFilters.Add(authFilter);

            // This is null right now. Future proofing this because no one likes tests
            // that only break when all tests run.
            var previousLogoutUrl =
                ConfigurationManager.AppSettings[FormsAuthenticationAuthorizer.AppSettingsKeys.LOGOUT_URL];
            try
            {
                ConfigurationManager.AppSettings[FormsAuthenticationAuthorizer.AppSettingsKeys.LOGOUT_URL] =
                    "some logout url";
                var auth = new MapCallControllerAuthorizationTester(_application, CreateFactoryService(), _container);
                auth.Assert(a => { a.RequiresLoggedInUserOnly("~/Map/Index/"); });
            }
            finally
            {
                ConfigurationManager.AppSettings[FormsAuthenticationAuthorizer.AppSettingsKeys.LOGOUT_URL] =
                    previousLogoutUrl;
            }
        }

        [TestMethod]
        public void TestIndexDoesNotSetMapConfigurationWhenModelStateIsInvalid()
        {
            _target.ModelState.AddModelError("bluh", "afluahe");
            _indexModel.Search.Add("SomeProperty", "SomeValue");
            _target.Index(_indexModel);
            Assert.IsFalse(_target.ViewData.ContainsKey(MapController.MAP_CONFIGURATION));
        }

        [TestMethod]
        public void TestIndexReturns404NotFoundWhenModelStateIsNotValid()
        {
            _target.ModelState.AddModelError("error", "error");
            MvcAssert.IsNotFound(_target.Index(_indexModel));
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // override needed because not ISearchSet
            // noop because Index works differently here and doesn't actually return results.
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // override because Index doesn't actually search.
        }

        [TestMethod]
        public void TestIndexReturnsViewResultWhenModelStateIsValid()
        {
            MvcAssert.IsViewNamed(_target.Index(_indexModel), "Index");
        }

        [TestMethod]
        public void TestIndexSetsDataUrlOnMapConfigurationWhenModelStateIsValid()
        {
            _indexModel.Search.Add("SomeProperty", "SomeValue");
            _target.Index(_indexModel);

            Assert.AreEqual("/Controller/Index.map?SomeProperty=SomeValue", _indexModel.MapConfiguration.dataUrl);
        }

        [TestMethod]
        public void TestIndexSetsViewDataForSerializedIcons()
        {
            var expectedIcon = GetEntityFactory<MapIcon>().Create();
            Session.Flush();
            _target.Index(_indexModel);
            var serializedIcons = _indexModel.MapConfiguration.icons;

            // There should only be one.
            var resultIcon = serializedIcons.Single();
            Assert.AreEqual(String.Format("/Content/images/{0}", expectedIcon.FileName), resultIcon.url);
            Assert.AreEqual(expectedIcon.Width, resultIcon.width);
            Assert.AreEqual(expectedIcon.Height, resultIcon.height);
            Assert.AreEqual(expectedIcon.Id, resultIcon.id);
        }

        [TestMethod]
        public void TestIndexUsesThreatAlertsForDefaultLayers()
        {
            _indexModel.DefaultLayers = null;

            _target.Index(_indexModel);

            Assert.AreEqual(MapController.THREAT_ALERTS, _indexModel.MapConfiguration.defaultLayers[0]);
            Assert.AreEqual(1, _indexModel.MapConfiguration.defaultLayers.Length);
        }

        [TestMethod]
        public void TestIndexUsesDefinedLayersAndIncludesThreatAlertsForDefaultLayers()
        {
            var defaultLayers = new[] { "Water Network", "Sewer Network" };
            _indexModel.DefaultLayers = defaultLayers;

            _target.Index(_indexModel);

            Assert.AreEqual(defaultLayers[0], _indexModel.MapConfiguration.defaultLayers[0]);
            Assert.AreEqual(defaultLayers[1], _indexModel.MapConfiguration.defaultLayers[1]);
            Assert.AreEqual(MapController.THREAT_ALERTS, _indexModel.MapConfiguration.defaultLayers[2]);
            Assert.AreEqual(3, _indexModel.MapConfiguration.defaultLayers.Length);
        }

        #endregion
    }

    public class MapCallControllerAuthorizationTester : ControllerAuthorizationTester<FakeMvcApplication, User,
        MapCallControllerAuthorizationAsserter>
    {
        private readonly IContainer _container;

        #region Constructors

        public MapCallControllerAuthorizationTester(MvcApplicationTester<FakeMvcApplication> appTester,
            ITestDataFactoryService testFactoryService, IContainer container) : base(appTester, testFactoryService)
        {
            _container = container;
        }

        #endregion

        protected override MapCallControllerAuthorizationAsserter CreateAsserter()
        {
            return new MapCallControllerAuthorizationAsserter(_application, _factoryService, _container);
        }
    }

    public class MapCallControllerAuthorizationAsserter : ControllerAuthorizationAsserter<FakeMvcApplication, User>
    {
        protected override User CreateUser(UserType userType)
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = userType == UserType.SiteAdmin});
        }

        public MapCallControllerAuthorizationAsserter(MvcApplicationTester<FakeMvcApplication> appTester,
            ITestDataFactoryService testFactoryService, IContainer container) : base(appTester, testFactoryService,
            container) { }
    }
}
