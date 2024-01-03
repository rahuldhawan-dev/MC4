using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Core.MvcTest.Data;
using MMSINC.Data.NHibernate;
using MMSINC.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using MMSINC.Data;

namespace MMSINC.Core.MvcTest.Controllers
{
    [TestClass]
    public class ControllerBaseWithPersistenceTest
    {
        #region Private Members

        private TestControllerWithPersistence _target;
        private Mock<IRepository<TestUser>> _repositoryMock;
        private Mock<IAuthenticationService<TestUser>> _authenticationService;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(e => e.For<IViewModelFactory>().Use<ViewModelFactory>());
            _container.Inject((_repositoryMock = new Mock<IRepository<TestUser>>()).Object);
            _container.Inject((_authenticationService = new Mock<IAuthenticationService<TestUser>>()).Object);
            _target = _container.GetInstance<TestControllerWithPersistence>();
        }

        #endregion

        #region DoRedirectionToAction

        [TestMethod]
        public void TestDoRedirectionToActionRedirectsToAction()
        {
            const string expected = "expected";
            // ReSharper disable once Mvc.ActionNotResolved
            var result = _target.DoRedirectionToAction("Foo", new {Bar = expected});

            Assert.AreEqual("Foo", result.RouteValues["Action"]);
            Assert.AreEqual(expected, result.RouteValues["Bar"]);
        }

        #endregion
    }

    public class TestControllerWithPersistence : ControllerBaseWithPersistence<TestUser, TestUser>
    {
        #region Constructors

        public TestControllerWithPersistence(
            ControllerBaseWithPersistenceArguments<IRepository<TestUser>, TestUser, TestUser> args) : base(args) { }

        #endregion
    }
}
