using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Utility
{
    [TestClass]
    public class MapCallUrlHelperTest
    {
        private MapCallUrlHelper _target;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new MapCallUrlHelper();
        }

        #endregion

        [TestMethod]
        public void TestReturnsCorrectUrlForControllerAndAction()
        {
            var controller = "Controller";
            var action = "Action";
            var result = _target.Action(action, controller, new { });

            Assert.AreEqual($"{MapCallUrlHelper.BASE_MVC_PATH}/{controller}/{action}", result);
        }

        [TestMethod]
        public void TestReturnsCorrectUrlForControllerAndActionAndArea()
        {
            var controller = "Controller";
            var action = "Action";
            var area = "AREA";
            var result = _target.Action(action, controller, new {area});

            Assert.AreEqual($"{MapCallUrlHelper.BASE_MVC_PATH}/{area}/{controller}/{action}", result);
        }

        [TestMethod]
        public void TestReturnsCorrectUrlForControllerAndActionAndId()
        {
            var controller = "Controller";
            var action = "Action";
            var id = 1;
            var result = _target.Action(action, controller, new {id});

            Assert.AreEqual($"{MapCallUrlHelper.BASE_MVC_PATH}/{controller}/{action}/{id}", result);
        }

        [TestMethod]
        public void TestReturnsCorrectUrlForControllerAndActionAndIdAndFrag()
        {
            var controller = "Controller";
            var action = "Action";
            var id = 1;
            var frag = "JSON";
            var result = _target.Action(action, controller, new {id, frag});

            Assert.AreEqual($"{MapCallUrlHelper.BASE_MVC_PATH}/{controller}/{action}/{id}.{frag}", result);
        }

        [TestMethod]
        public void TestReturnsCorrectUrlForControllerAndActionAndAreaAndId()
        {
            var controller = "Controller";
            var action = "Action";
            var area = "AREA";
            var id = 1;
            var result = _target.Action(action, controller, new {id, area});

            Assert.AreEqual($"{MapCallUrlHelper.BASE_MVC_PATH}/{area}/{controller}/{action}/{id}", result);
        }

        [TestMethod]
        public void TestReturnsCorrectUrlForControllerAndActionAndAreaAndIdAndFrag()
        {
            var controller = "Controller";
            var action = "Action";
            var area = "AREA";
            var id = 1;
            var frag = "JSON";
            var result = _target.Action(action, controller, new {id, area, frag});

            Assert.AreEqual($"{MapCallUrlHelper.BASE_MVC_PATH}/{area}/{controller}/{action}/{id}.{frag}", result);
        }
    }
}
