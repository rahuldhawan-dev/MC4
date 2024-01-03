using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.Core.MvcTest
{
    [TestClass]
    public class JsonHttpStatusCodeResultTest
    {
        #region Fields

        private ControllerContext _controllerContext;
        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpResponseBase> _response;

        #endregion

        #region Setup

        [TestInitialize]
        public void InitializeTest()
        {
            _httpContext = new Mock<HttpContextBase>();
            _response = new Mock<HttpResponseBase>();
            _httpContext.Setup(x => x.Response).Returns(_response.Object);
            _controllerContext = new ControllerContext {
                HttpContext = _httpContext.Object
            };
        }

        #endregion

        [TestMethod]
        public void TestExecuteSetsResponseStatusCode()
        {
            var expectedStatusCode = HttpStatusCode.BadGateway;
            var target = new JsonHttpStatusCodeResult(expectedStatusCode);
            target.ExecuteResult(_controllerContext);
            _response.VerifySet(x => x.StatusCode = (int)expectedStatusCode);
        }

        [TestMethod]
        public void TestExecuteSetsStatusDescription()
        {
            var expectedDescription = "I'm a description";
            var target = new JsonHttpStatusCodeResult(1, expectedDescription);
            target.ExecuteResult(_controllerContext);
            _response.VerifySet(x => x.StatusDescription = expectedDescription);
        }

        [TestMethod]
        public void TestExecuteWritesStatusDescriptionToResponse()
        {
            var expectedDescription = "I'm a description";
            var target = new JsonHttpStatusCodeResult(1, expectedDescription);
            target.ExecuteResult(_controllerContext);
            _response.Verify(x => x.Write(expectedDescription));
        }

        [TestMethod]
        public void TestExecuteSetsResponseContentTypeToTextPlain()
        {
            var target = new JsonHttpStatusCodeResult(1, "meh");
            target.ExecuteResult(_controllerContext);
            _response.VerifySet(x => x.ContentType = "text/plain");
        }
    }
}
