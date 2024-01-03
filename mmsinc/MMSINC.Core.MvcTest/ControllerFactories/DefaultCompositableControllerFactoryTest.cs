using System;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ControllerFactories;
using StructureMap;

// ReSharper disable Mvc.ControllerNotResolved

namespace MMSINC.Core.MvcTest.ControllerFactories
{
    [TestClass]
    public class DefaultCompositableControllerFactoryTest
    {
        #region Fields

        private TestDefaultCompositableControllerFactory _target;
        private IContainer _container;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _target = _container.GetInstance<TestDefaultCompositableControllerFactory>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestCanHandleControllerReturnsFalseIfGetControllerTypeReturnsNull()
        {
            var req = new RequestContext {RouteData = new RouteData()};
            _target.GetControllerTypeReturnValue = null;
            Assert.IsFalse(_target.CanHandleController(req, "uh"));
        }

        [TestMethod]
        public void TestCanHandleControllerReturnsTrueIfGetControllerTypeReturnsANotNullThingy()
        {
            var req = new RequestContext {RouteData = new RouteData()};
            _target.GetControllerTypeReturnValue = typeof(object);
            Assert.IsTrue(_target.CanHandleController(req, "uh"));
        }

        #endregion

        #region TestClass

        private class TestDefaultCompositableControllerFactory : DefaultCompositableControllerFactory
        {
            #region Properties

            public Type GetControllerTypeReturnValue { get; set; }

            #endregion

            #region Constructors

            public TestDefaultCompositableControllerFactory(IContainer container) : base(container) { }

            #endregion

            #region Private Methods

            protected override Type GetControllerType(RequestContext requestContext, string controllerName)
            {
                return GetControllerTypeReturnValue;
            }

            #endregion
        }

        #endregion
    }
}
