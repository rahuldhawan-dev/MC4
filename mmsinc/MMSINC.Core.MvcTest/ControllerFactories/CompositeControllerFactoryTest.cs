using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using MMSINC.ControllerFactories;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

// ReSharper disable Mvc.ControllerNotResolved

namespace MMSINC.Core.MvcTest.ControllerFactories
{
    [TestClass]
    public class CompositeControllerFactoryTest
    {
        #region Fields

        private CompositeControllerFactory _target;

        private Mock<ICompositableControllerFactory> _factoryOne,
                                                     _factoryTwo;

        private Mock<IController> _controller;
        private RequestContext _requestContext;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new CompositeControllerFactory();
            _requestContext = new RequestContext();
            _controller = new Mock<IController>();
            _factoryOne = new Mock<ICompositableControllerFactory>();
            _factoryTwo = new Mock<ICompositableControllerFactory>();
            _target.Factories.Add(_factoryOne.Object);
            _target.Factories.Add(_factoryTwo.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestCreateControllerReturnsControllerFromExpectedFactory()
        {
            _factoryOne.Setup(x => x.CanHandleController(_requestContext, "controllerName")).Returns(true);
            _factoryOne.Setup(x => x.CreateController(_requestContext, "controllerName")).Returns(_controller.Object);

            Assert.AreSame(_controller.Object, _target.CreateController(_requestContext, "controllerName"));

            var otherController = new Mock<IController>().Object;
            _factoryTwo.Setup(x => x.CanHandleController(_requestContext, "other")).Returns(true);
            _factoryTwo.Setup(x => x.CreateController(_requestContext, "other")).Returns(otherController);

            Assert.AreSame(otherController, _target.CreateController(_requestContext, "other"));
        }

        [TestMethod]
        public void TestCreateControllerThrowsExceptionIfCanNotCreateControllerFromRegisteredFactories()
        {
            _factoryOne.Setup(x => x.CanHandleController(_requestContext, "controllerName")).Returns(false);
            _factoryTwo.Setup(x => x.CanHandleController(_requestContext, "controllerName")).Returns(false);
            MyAssert.Throws<HttpException>(() => _target.CreateController(_requestContext, "controllerName"),
                "This must be HttpException for proper 404 pretty page handling.");
            _factoryOne.VerifyAll();
            _factoryTwo.VerifyAll();
        }

        [TestMethod]
        public void TestGetControllerSessionBehaviorReturnsValueFromExpectedFactory()
        {
            _factoryOne.Setup(x => x.CanHandleController(_requestContext, "controllerName")).Returns(true);
            _factoryOne.Setup(x => x.GetControllerSessionBehavior(_requestContext, "controllerName"))
                       .Returns(SessionStateBehavior.Required);

            Assert.AreEqual(SessionStateBehavior.Required,
                _target.GetControllerSessionBehavior(_requestContext, "controllerName"));

            _factoryTwo.Setup(x => x.CanHandleController(_requestContext, "other")).Returns(true);
            _factoryTwo.Setup(x => x.GetControllerSessionBehavior(_requestContext, "other"))
                       .Returns(SessionStateBehavior.Disabled);

            Assert.AreEqual(SessionStateBehavior.Disabled,
                _target.GetControllerSessionBehavior(_requestContext, "other"));
        }

        [TestMethod]
        public void TestGetControllerSessionBehaviorReturnsSessionBehavoirDefaultIfNoRegisteredFactoriesCanHandle()
        {
            _factoryOne.Setup(x => x.CanHandleController(_requestContext, "controllerName")).Returns(false);
            _factoryTwo.Setup(x => x.CanHandleController(_requestContext, "controllerName")).Returns(false);
            var result = _target.GetControllerSessionBehavior(_requestContext, "controllerName");
            Assert.AreEqual(SessionStateBehavior.Default, result);
            _factoryOne.VerifyAll();
            _factoryTwo.VerifyAll();
        }

        [TestMethod]
        public void TestReleaseControllerDisposesControllerIfItIsIdisposable()
        {
            var controller = new Mock<IDisposableController>();
            _target.ReleaseController(controller.Object);
            controller.Verify(x => x.Dispose());

            var notDisposable = new Mock<IController>();
            MyAssert.DoesNotThrow(() => _target.ReleaseController(notDisposable.Object));
        }

        #endregion

        #region Test classes

        public interface IDisposableController : IController, IDisposable { }

        #endregion
    }

    public class WhatController : Controller { }
}
