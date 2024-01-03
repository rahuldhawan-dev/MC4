using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.Common.MvcTest.Configuration
{
    [TestClass]
    public class AuditSelectFilterTest : InMemoryDatabaseTest<AuditLogEntry>
    {
        #region Private Members

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authenticationService;
        private User _user;
        private AuditSelectFilter _target;
        private FakeMvcApplicationTester _tester;
        private FakeMvcHttpHandler _request;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IAuthenticationService<User>>()
             .Use((_authenticationService = new Mock<IAuthenticationService<User>>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = _container.GetInstance<AuditSelectFilter>();

            _user = GetFactory<UserFactory>().Create();

            _authenticationService.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _authenticationService.Setup(x => x.CurrentUser).Returns(_user);

            _tester = _container.With(true).GetInstance<FakeMvcApplicationTester>();
            var controller = new CrudController();
            _tester.ControllerFactory.RegisterController(controller);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _tester.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestOnActionExecutedSkipsOverNewCreateEditUpdateDestroySearchActions()
        {
            var actions = new[] { "New", "Create", "Edit", "Update", "Destroy", "Search" };

            foreach (var action in actions)
            {
                _request = _tester.CreateRequestHandler(String.Format("~/Crud/{0}/", action));
                var controllerContext = _request.CreateControllerContext(new CrudController());
                var rc = new RouteContext(_request.RequestContext);

                var context = new ActionExecutedContext(controllerContext, rc.ActionDescriptor, false, null);

                MyAssert.DoesNotCauseIncrease(
                    () => _target.OnActionExecuted(context),
                    () => Session.Query<AuditLogEntry>().Count());
            }
        }

        [TestMethod]
        public void TestOnActionExecutedAddsAuditLogEntryForShow()
        {
            const string action = "Show";
            const int id = 626;
            _request = _tester.CreateRequestHandler("~/Crud/" + action + "/");
            var controllerContext = _request.CreateControllerContext(new CrudController());
            controllerContext.RouteData.Values["Id"] = id;
            var rc = new RouteContext(_request.RequestContext);
            var context = new ActionExecutedContext(controllerContext, rc.ActionDescriptor, false, null);
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);

            MyAssert.CausesIncrease(
                () => _target.OnActionExecuted(context),
                () => Session.Query<AuditLogEntry>().Count());

            var entry = Session.Query<AuditLogEntry>().First();

            Assert.AreEqual(_user.Id, entry.User.Id);
            Assert.AreEqual(action, entry.AuditEntryType);
            Assert.AreEqual("Crud", entry.EntityName);
            Assert.AreEqual(id, entry.EntityId);
            Assert.AreEqual(now, entry.Timestamp);
        }

        [TestMethod]
        public void TestOnActionExecutedAddsAuditLogEntryForShowWithoutIdWhenTheIdIsNotAValidInteger()
        {
            const string action = "Show";
            const string id = "626a";
            _request = _tester.CreateRequestHandler("~/Crud/" + action + "/");
            var controllerContext = _request.CreateControllerContext(new CrudController());
            controllerContext.RouteData.Values["Id"] = id;
            var rc = new RouteContext(_request.RequestContext);
            var context = new ActionExecutedContext(controllerContext, rc.ActionDescriptor, false, null);
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);

            MyAssert.CausesIncrease(
                () => _target.OnActionExecuted(context),
                () => Session.Query<AuditLogEntry>().Count());

            var entry = Session.Query<AuditLogEntry>().First();

            Assert.AreEqual(_user.Id, entry.User.Id);
            Assert.AreEqual(action, entry.AuditEntryType);
            Assert.AreEqual("Crud", entry.EntityName);
            Assert.AreEqual(0, entry.EntityId, "EntityId should not have been set.");
            Assert.AreEqual(now, entry.Timestamp);
        }

        [TestMethod]
        public void TestOnActionExecutedAddsAuditLogEntryForIndex()
        {
            const string action = "Index";
            _request = _tester.CreateRequestHandler("~/Crud/" + action + "/");
            var controllerContext = _request.CreateControllerContext(new CrudController());
            var rc = new RouteContext(_request.RequestContext);
            var context = new ActionExecutedContext(controllerContext, rc.ActionDescriptor, false, null);
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);

            MyAssert.CausesIncrease(
                () => _target.OnActionExecuted(context),
                () => Session.Query<AuditLogEntry>().Count());

            var entry = Session.Query<AuditLogEntry>().First();

            Assert.AreEqual(_user.Id, entry.User.Id);
            Assert.AreEqual(action, entry.AuditEntryType);
            Assert.AreEqual("Crud", entry.EntityName);
            Assert.AreEqual(now, entry.Timestamp);
        }

        #region Test Classes

        private class CrudController : System.Web.Mvc.Controller
        {
            public ActionResult Search()
            {
                return null;
            }

            public ActionResult New()
            {
                return null;
            }

            public ActionResult Create()
            {
                return null;
            }

            public ActionResult Destroy()
            {
                return null;
            }

            public ActionResult Edit()
            {
                return null;
            }

            public ActionResult Update()
            {
                return null;
            }

            public ActionResult Index()
            {
                return null;
            }

            public ActionResult Show()
            {
                return null;
            }
        }

        #endregion
    }
}
