using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Testing;
using StructureMap;

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class ActionDescriptorExtensionsTest
    {
        #region Fields

        private FakeMvcApplicationTester _appTester;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _appTester = new FakeMvcApplicationTester(new Container());
            _appTester.ControllerFactory.RegisterController(new HttpController());
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _appTester.Dispose();
        }

        #endregion

        #region Tests

        private void AssertVerb(string requestPath, HttpVerbs expectedVerb)
        {
            var request = _appTester.CreateRequestHandler(requestPath);
            Assert.AreEqual(expectedVerb, request.RouteContext.ActionDescriptor.GetHttpVerb());
        }

        [TestMethod]
        public void TestGetHttpVerbReturnsPostIfHttpPost()
        {
            AssertVerb("~/Http/PostAction", HttpVerbs.Post);
        }

        [TestMethod]
        public void TestGetHttpVerbReturnsPutIfHttpPut()
        {
            AssertVerb("~/Http/PutAction", HttpVerbs.Put);
        }

        [TestMethod]
        public void TestGetHttpVerbReturnsDeleteIfHttpDelete()
        {
            AssertVerb("~/Http/DeleteAction", HttpVerbs.Delete);
        }

        [TestMethod]
        public void TestGetHttpVerbReturnsGetIfHttpGet()
        {
            AssertVerb("~/Http/GetAction", HttpVerbs.Get);
        }

        [TestMethod]
        public void TestGetHttpVerbReturnsGetIfThereIsNoAttribute()
        {
            AssertVerb("~/Http/GetWithoutAttributeAction", HttpVerbs.Get);
        }

        #endregion

        #region Test classes

        private class HttpController : Controller
        {
            [HttpPost]
            public ActionResult PostAction()
            {
                return null;
            }

            [HttpPut]
            public ActionResult PutAction()
            {
                return null;
            }

            [HttpDelete]
            public ActionResult DeleteAction()
            {
                return null;
            }

            [HttpGet]
            public ActionResult GetAction()
            {
                return null;
            }

            public ActionResult GetWithoutAttributeAction()
            {
                return null;
            }
        }

        #endregion
    }
}
