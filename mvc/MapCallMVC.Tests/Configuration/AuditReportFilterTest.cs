using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System.Linq;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;

namespace MapCallMVC.Tests.Configuration
{
    [TestClass]
    public class AuditReportFilterTest : InMemoryDatabaseTest<Restoration>
    {
        #region Fields

        private MapCallMvcApplicationTester _appTester;
        private Mock<IDateTimeProvider> _dateProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        private AuditReportFilter _target;
        private ActionExecutedContext _actionContext;
        private FakeMvcHttpHandler _request;
        private User _user;
        private ViewResult _result;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IViewModelFactory>().Use<ViewModelFactory>();
            e.For<IRestorationRepository>().Use<RestorationRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            _dateProvider = e.For<IDateTimeProvider>().Mock();
            _authServ = e.For<IAuthenticationService<User>>().Mock();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new AuditReportFilter(_container);
            _user = GetFactory<UserFactory>().Create();

            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _result = new ViewResult();

            _appTester = _container.With(true).GetInstance<MapCallMvcApplicationTester>();
            _request = _appTester.CreateRequestHandler("~/Reports/RestorationAccrualReport/Index");

            _actionContext =
                new ActionExecutedContext(
                    _request.CreateAndInitializeController<RestorationAccrualReportController>().ControllerContext,
                    _request.RouteContext.ActionDescriptor, false, null) {
                    Result = _result
                };
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _appTester.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestOnActionExecutedCreatesNewReportViewedForReport()
        {
            var now = DateTime.Now;
            _dateProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var model = GetFactory<RestorationFactory>().Create();
            _actionContext.Result = new ViewResult();
            var attribute = _actionContext.ActionDescriptor.GetCustomAttributes(typeof(AuditReportAttribute), true)
                                          .FirstOrDefault() as AuditReportAttribute;

            _target.OnActionExecuted(_actionContext);

            var viewed = Session.Query<ReportViewing>().Single();
            Assert.AreSame(_user, viewed.User);
            Assert.AreEqual(attribute.ReportName, viewed.ReportName);
            Assert.AreEqual(now, viewed.DateViewed);
        }

        #endregion
    }
}
