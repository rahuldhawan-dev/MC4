using System;
using MapCall.Common.Utility;
using MapCall.Common.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.Common.WebFormsTest.Web
{
    /// <summary>
    /// Summary description for MapCallHttpApplicationTest
    /// </summary>
    [TestClass]
    public class MapCallHttpApplicationTest
    {
        private Mock<IHostingEnvironment> _mockHosting = new Mock<IHostingEnvironment>();
        private Mock<IResourceManager> _mockManager = new Mock<IResourceManager>();
        private Mock<IResourceConfiguration> _mockConfig = new Mock<IResourceConfiguration>();
        private readonly Uri _expectedUri = new Uri("http://www.google.com/dingdong");

        private TestMapCallHttpApplication Initialize()
        {
            var target = new TestMapCallHttpApplication(_mockConfig.Object);
            target.HostingEnvironment = _mockHosting.Object;
            target.TestResourceManager = _mockManager.Object;
            target.TestCurrentRequestUri = _expectedUri;
            return target;
        }

        #region Static property tests

        [TestMethod]
        public void TestStaticMachineCheckPropertiesAreFalseByDefault()
        {
            Assert.Inconclusive("Need to add tests for this when this whole thing is properly figured out.");
            //Assert.IsFalse(MapCallHttpApplication.IsDevMachine);
            //Assert.IsFalse(MapCallHttpApplication.IsLocalhostMapCall);
            //Assert.IsFalse(MapCallHttpApplication.IsVisualStudioDevServer);
        }

        #endregion

        [TestMethod]
        public void TestConstractorSetsStaticInstanceProperty()
        {
            var target = Initialize();
            Assert.AreSame(target, MapCallHttpApplication.Instance);
        }

        [TestMethod]
        public void TestOnApplicationStartCallsResourceHandlerSetResourceManager()
        {
            var target = Initialize();
            target.DoOnApplicationStart();
            var expected = target.ResourceManager;
            Assert.AreSame(expected, ResourceHandler.ResourceManager);
        }

        [TestMethod]
        public void TestInitRegistersResourceVirtualPathProviderWithHostingEnvironment()
        {
            var target = Initialize();
            var expectedVPP = target.CreateResourceVirtualPathProvider();

            _mockHosting.Setup(x => x.RegisterVirtualPathProvider(expectedVPP));

            target.DoOnApplicationStart();

            _mockHosting.VerifyAll();
        }

        [TestMethod]
        public void TestGetHostingEnvironmentReturnsNonNullObject()
        {
            var target = Initialize();

            // Nulling out the test property has it return
            // base.GetHostingEnvironment()
            target.HostingEnvironment = null;

            Assert.IsNotNull(target.HostingEnvironment);
        }

        [TestMethod]
        public void TestCreateResourceManagerReturnsResourceManager()
        {
            var target = Initialize();
            var rm = target.BaseCreateResourceManager();
            Assert.IsNotNull(rm);
        }

        [TestMethod]
        public void TestResourceManagerPropertyInitializesCreatedResourceManager()
        {
            var target = Initialize();
            var expectedSiteConfiguration = target.SiteConfiguration;

            Assert.IsNotNull(expectedSiteConfiguration);

            _mockManager.Setup(x => x.InitializeConfiguration(expectedSiteConfiguration));

            // Need to call the property to get the initialization done. 
            var rm = target.ResourceManager;

            _mockManager.VerifyAll();
        }

        [TestMethod]
        public void TestSiteConfigurationPropertySetsConfigurationResourceName()
        {
            _mockConfig.SetupSet(x => x.ConfigurationResourceName = MapCallHttpApplication.CONFIGURATION_RESOURCE_NAME);

            var target = Initialize();
            var result = target.SiteConfiguration;
            _mockConfig.VerifyAll();
        }
    }

    public class TestMapCallHttpApplication : MapCallHttpApplication
    {
        #region Fields

        private ResourceVirtualPathProvider _provider;
        private IHostingEnvironment _hostingEnvironment;

        #endregion

        #region Properties

        public IHostingEnvironment HostingEnvironment
        {
            get
            {
                if (_hostingEnvironment != null)
                {
                    return _hostingEnvironment;
                }

                return base.GetHostingEnvironment();
            }
            set { _hostingEnvironment = value; }
        }

        public IResourceConfiguration TestResourceConfiguration { get; }
        public IResourceManager TestResourceManager { get; set; }
        public Uri TestCurrentRequestUri { get; set; }

        #endregion

        #region Constructors

        public TestMapCallHttpApplication(IResourceConfiguration resourceConfiguration)
        {
            TestResourceConfiguration = resourceConfiguration;
        }

        #endregion

        #region Methods

        public void DoOnApplicationStart()
        {
            OnApplication_Start();
        }

        public override IResourceConfiguration GetResourceConfiguration()
        {
            return TestResourceConfiguration;
        }

        public IResourceManager BaseCreateResourceManager()
        {
            return base.CreateResourceManager();
        }

        public override IResourceManager CreateResourceManager()
        {
            return TestResourceManager;
        }

        public override IHostingEnvironment GetHostingEnvironment()
        {
            return HostingEnvironment;
        }

        public override ResourceVirtualPathProvider CreateResourceVirtualPathProvider()
        {
            if (_provider == null)
            {
                _provider = base.CreateResourceVirtualPathProvider();
            }

            return _provider;
        }

        #endregion
    }
}
