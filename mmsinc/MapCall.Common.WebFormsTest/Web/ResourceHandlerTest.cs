using System;
using System.Web;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Utility;
using MapCall.Common.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.Common.WebFormsTest.Web
{
    /// <summary>
    /// Summary description for ResourceHandlerTest
    /// </summary>
    [TestClass]
    public class ResourceHandlerTest
    {
        #region Constants

        private const string TEST_REQUEST_URL = "http://www.compuglobalhypermega.net/absolute/path/";
        private const string EXPECTED_REQUEST_ABSOLUTEURI = "/absolute/path/";

        #endregion

        #region Fields

        private Mock<IResourceManager> _mockManager = new Mock<IResourceManager>();
        private Mock<IHttpContext> _mockContext = new Mock<IHttpContext>();
        private Mock<IResponse> _mockResponse = new Mock<IResponse>();
        private Mock<IRequest> _mockRequest = new Mock<IRequest>();

        #endregion

        [TestInitialize]
        public void ResourceHandlerTestInitialize()
        {
            ResourceHandler.SetResourceManager(_mockManager.Object);
            _mockContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _mockContext.Setup(x => x.Response).Returns(_mockResponse.Object);

            var assembly = typeof(ResourceManager).Assembly;
            var realPath = "/resources/bender/bender.css";
            _mockManager.Setup(x => x.GetStreamByVirtualPath(realPath))
                        .Returns(assembly.GetManifestResourceStream("MapCall.Common.Resources.bender.bender.css"));
            _mockManager.Setup(x => x.HasResource(realPath)).Returns(true);

            var binaryPath = "/resources/bender/dropShadow.png";
            _mockManager.Setup(x => x.GetStreamByVirtualPath(binaryPath))
                        .Returns(assembly.GetManifestResourceStream("MapCall.Common.Resources.bender.dropShadow.png"));
            _mockManager.Setup(x => x.HasResource(binaryPath)).Returns(true);
        }

        private TestResourceHandlerBuilder Initialize()
        {
            return new TestResourceHandlerBuilder()
               .WithTestHttpContext(_mockContext.Object);
        }

        [TestMethod]
        public void TestContentHelperConstructorSetsPropertyValues()
        {
            var expectedSRT = ResourceHandler.StreamReaderType.BinaryReader;
            var expectedContentType = "yeah/no";

            var ct = new ResourceHandler.ContentHelper(expectedContentType, expectedSRT);

            Assert.AreEqual(expectedContentType, ct.ContentType);
            Assert.AreEqual(expectedSRT, ct.StreamReaderType);
        }

        [TestMethod]
        public void TestSetResourceManagerThrowsForNullArgument()
        {
            MyAssert.Throws(() => ResourceHandler.SetResourceManager(null));
        }

        [TestMethod]
        public void TestSetResourceManagerSetsStaticResourceManagerProperty()
        {
            var expected = _mockManager.Object;
            ResourceHandler.SetResourceManager(expected);
            Assert.AreSame(expected, ResourceHandler.ResourceManager);
        }

        [TestMethod]
        public void TestIsReusableIsFalse()
        {
            Assert.IsFalse(Initialize().Build().IsReusable);
        }

        [TestMethod]
        public void TestGetIHttpContextReturnsWrapper()
        {
            var target = Initialize().Build();
            var result = target.BaseGetIHttpContext();
            Assert.IsNotNull(result);
        }

        #region ProcessRequest tests

        [TestMethod]
        public void TestProcessRequestCallsResourceManagerHasResourceWithAbsoluteUri()
        {
            var mockCache = new Mock<IHttpCachePolicy>();

            _mockRequest.Setup(x => x.Uri).Returns(new Uri(TEST_REQUEST_URL));
            _mockManager.Setup(x => x.HasResource(EXPECTED_REQUEST_ABSOLUTEURI)).Returns(true);
            _mockResponse.Setup(x => x.Cache).Returns(mockCache.Object);

            var target = Initialize()
                        .WithTestProcessEmbeddedResourceRequest(true)
                        .Build();
            target.ProcessRequest(null);
        }

        [TestMethod]
        public void TestProcessRequestCallsProcessEmbeddedResourceRequestIfResourceManagerHasResource()
        {
            var mockCache = new Mock<IHttpCachePolicy>();
            _mockRequest.Setup(x => x.Uri).Returns(new Uri(TEST_REQUEST_URL));
            _mockManager.Setup(x => x.HasResource(EXPECTED_REQUEST_ABSOLUTEURI)).Returns(true);
            _mockResponse.Setup(x => x.Cache).Returns(mockCache.Object);

            var target = Initialize()
                        .WithTestProcessEmbeddedResourceRequest(true)
                        .Build();
            target.ProcessRequest(null);

            Assert.IsTrue(target.ProcessEmbeddedResourceRequestWasCalled);
        }

        [TestMethod]
        public void TestProcessRequestDefersRequestIfResourceManagerHasResourceIsFalse()
        {
            _mockRequest.Setup(x => x.Uri).Returns(new Uri("http://www.thegoogles.com/path/"));
            _mockManager.Setup(x => x.HasResource("/path/")).Returns(false);

            var target = Initialize()
                        .WithTestDeferRequest(true)
                        .Build();
            target.ProcessRequest(null);

            Assert.IsFalse(target.ProcessEmbeddedResourceRequestWasCalled);
            Assert.IsTrue(target.DeferRequestWasCalled);
        }

        [TestMethod]
        public void TestGetResourceAddsToCache()
        {
            var result = ResourceHandler.GetResource(ResourceHandler.StreamReaderType.StreamReader,
                "/resources/bender/bender.css");

            var cached = HttpRuntime.Cache["/resources/bender/bender.css"];

            Assert.IsNotNull(cached);
        }

        [TestMethod]
        public void TestGetResourceReturnsStringWhenStreamReaderTypeIsStreamReader()
        {
            var result = ResourceHandler.GetResource(ResourceHandler.StreamReaderType.StreamReader,
                "/resources/bender/bender.css");

            Assert.IsTrue(result is string);
        }

        [TestMethod]
        public void TestGetResourceReturnsByteArrayWhenStreamReaderIsBinaryReader()
        {
            var result = ResourceHandler.GetResource(ResourceHandler.StreamReaderType.BinaryReader,
                "/resources/bender/dropShadow.png");
            Assert.IsTrue(result is byte[]);
        }

        [TestMethod]
        public void TestProcessEmbeddedResourceRequestWriteseResourceToResponse()
        {
            var target = Initialize().Build();

            target.ProcessEmbeddedResourceRequest(_mockContext.Object, "/resources/bender/bender.css");

            Assert.IsTrue(target.WriteResourceToResponseWasCalled);
        }

        [TestMethod]
        public void TestProcessEmbeddedResourceRequestSetsResponseContentType()
        {
            var expectedContentType = "text/css";
            _mockResponse.SetupSet(x => x.ContentType = expectedContentType);

            var target = Initialize().Build();

            target.ProcessEmbeddedResourceRequest(_mockContext.Object, "/resources/bender/bender.css");

            _mockResponse.VerifyAll();
        }

        [TestMethod]
        public void TestWriteResourceToResponseUsesResponseWriteWhenResourceIsString()
        {
            var target = Initialize().Build();

            var expectedResource
                = (string)ResourceHandler.GetResource(ResourceHandler.StreamReaderType.StreamReader,
                    "/resources/bender/bender.css");
            _mockResponse.Setup(x => x.Write(expectedResource));

            target.ProcessEmbeddedResourceRequest(_mockContext.Object, "/resources/bender/bender.css");

            _mockResponse.VerifyAll();
        }

        [TestMethod]
        public void TestWriteResourceToResponseUsesResponseBinaryWriteWhenResourceIsBinary()
        {
            var target = Initialize().Build();

            var expectedResource
                = (byte[])ResourceHandler.GetResource(ResourceHandler.StreamReaderType.BinaryReader,
                    "/resources/bender/dropShadow.png");
            _mockResponse.Setup(x => x.BinaryWrite(expectedResource));

            target.ProcessEmbeddedResourceRequest(_mockContext.Object, "/resources/bender/dropShadow.png");

            _mockResponse.VerifyAll();
        }

        #endregion
    }

    public class TestResourceHandler : ResourceHandler
    {
        #region Properties

        public IHttpContext TestHttpContext { get; set; }
        public bool UseTestDeferRequest { get; set; }
        public bool UseTestProcessEmbeddedResourceRequest { get; set; }
        public bool ProcessEmbeddedResourceRequestWasCalled { get; set; }
        public bool DeferRequestWasCalled { get; set; }
        public bool WriteResourceToResponseWasCalled { get; set; }

        #endregion

        #region Methods

        internal override IHttpContext GetIHttpContext(HttpContext context)
        {
            return TestHttpContext;
        }

        public IHttpContext BaseGetIHttpContext()
        {
            return base.GetIHttpContext(null);
        }

        internal override void ProcessEmbeddedResourceRequest(IHttpContext icontext, string path)
        {
            ProcessEmbeddedResourceRequestWasCalled = true;
            if (!UseTestProcessEmbeddedResourceRequest)
            {
                base.ProcessEmbeddedResourceRequest(icontext, path);
            }
        }

        internal override void DeferRequestToDefaultHandler(HttpContext context)
        {
            DeferRequestWasCalled = true;
            if (!UseTestDeferRequest)
            {
                base.DeferRequestToDefaultHandler(context);
            }
        }

        internal override void WriteResourceToResponse(ContentHelper helper, IResponse response, string resourcePath)
        {
            WriteResourceToResponseWasCalled = true;
            base.WriteResourceToResponse(helper, response, resourcePath);
        }

        #endregion
    }

    internal class TestResourceHandlerBuilder : TestDataBuilder<TestResourceHandler>
    {
        #region Private Members

        private bool _withTestDefer;
        private bool _withTestProcessEER;
        private IHttpContext _withTestHttpContext;

        #endregion

        #region Exposed Methods

        public TestResourceHandlerBuilder WithTestHttpContext(IHttpContext context)
        {
            _withTestHttpContext = context;
            return this;
        }

        public TestResourceHandlerBuilder WithTestDeferRequest(bool b)
        {
            _withTestDefer = b;
            return this;
        }

        public TestResourceHandlerBuilder WithTestProcessEmbeddedResourceRequest(bool b)
        {
            _withTestProcessEER = b;
            return this;
        }

        public override TestResourceHandler Build()
        {
            var obj = new TestResourceHandler();
            obj.TestHttpContext = _withTestHttpContext;
            obj.UseTestDeferRequest = _withTestDefer;
            obj.UseTestProcessEmbeddedResourceRequest = _withTestProcessEER;
            return obj;
        }

        #endregion
    }
}
