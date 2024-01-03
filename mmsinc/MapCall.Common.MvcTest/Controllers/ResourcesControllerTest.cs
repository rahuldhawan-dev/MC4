using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using MapCall.Common.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controllers;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using StructureMap;

namespace MapCall.Common.MvcTest.Controllers
{
    [TestClass]
    public class ResourcesControllerTest
    {
        #region Private Members

        private TestResourcesController _target;
        private Mock<HttpResponseBase> _mockResponse;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _mockResponse = new Mock<HttpResponseBase>();
            _target = _container.GetInstance<TestResourcesController>();
            _target.SetResponse(_mockResponse.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockResponse.VerifyAll();
        }

        #endregion

        #region Private Methods

        private string ReadStreamToString(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            using (var writer = new StringWriter())
            {
                writer.Write(reader.ReadToEnd());
                return writer.GetStringBuilder().ToString();
            }
        }

        #endregion

        [TestMethod]
        public void TestSessionStateIsDisabled()
        {
            var attr = _target.GetType().GetCustomAttributes(typeof(SessionStateAttribute), true)
                              .OfType<SessionStateAttribute>().Single();
            Assert.AreEqual(SessionStateBehavior.Disabled, attr.Behavior);
        }

        #region Js

        [TestMethod]
        public void TestJsReturnsHttpNotFoundIfArgumentIsNullOrEmpty()
        {
            HttpNotFoundResult result;
            new[] {null, String.Empty, "\t", " "}
               .Each(
                    s => MyAssert.DoesNotThrow(
                        () => result = (HttpNotFoundResult)_target.Js(s)));
        }

        [TestMethod]
        public void TestJsReturnsJsResourceInActionResult()
        {
            var file = "jquery-3.7.1.min.js";
            var expected = ReadStreamToString(_target.CallGetResourceStream(ResourceType.JS, file));

            var result = (ContentResult)_target.Js(file);

            Assert.AreEqual(ResourceType.JS.ToContentType(), result.ContentType);
            Assert.AreEqual(expected, result.Content);
        }

        [TestMethod]
        public void TestJsTrowsArgumentExceptionWhenFileDoesNotExist()
        {
            var file = "why would a file called this exist?";

            MyAssert
               .Throws<ArgumentException>(() => _target.Js(file));
        }

        #endregion

        #region Css

        [TestMethod]
        public void TestCssReturnsHttpNotFoundIfArgumentIsNullOrEmpty()
        {
            HttpNotFoundResult result;
            new[] {null, String.Empty, "\t", " "}
               .Each(
                    s => MyAssert.DoesNotThrow(
                        () => result = (HttpNotFoundResult)_target.Css(s)));
        }

        [TestMethod]
        public void TestCssTrowsArgumentExceptionWhenFileDoesNotExist()
        {
            var file = "why would a file called this exist?";

            MyAssert
               .Throws<ArgumentException>(() => _target.Css(file));
        }

        #endregion

        #region ResourceTypeExtensions

        [TestMethod]
        public void TestToContentTypeReturnsCorrectStringValue()
        {
            new Dictionary<ResourceType, string> {
                {ResourceType.JS, ResourceTypeExtensions.ContentTypes.JS},
                {ResourceType.CSS, ResourceTypeExtensions.ContentTypes.CSS}
            }.Each(i => Assert.AreEqual(i.Value, i.Key.ToContentType()));
        }

        #endregion
    }

    public class TestResourcesController : ResourcesController
    {
        #region Exposed Methods

        public void SetResponse(HttpResponseBase response)
        {
            var ctx = new Mock<HttpContextBase>();
            ctx.Setup(x => x.Response).Returns(response);
            ControllerContext = new ControllerContext(ctx.Object, new RouteData(), this);
        }

        public Stream CallGetResourceStream(ResourceType type, string file)
        {
            return GetResourceStream(type, file);
        }

        #endregion

        public TestResourcesController(ControllerBaseArguments args) : base(args) { }
    }
}
