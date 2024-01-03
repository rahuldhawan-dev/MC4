using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using NHibernate.Linq;
using StructureMap;
using System.Linq;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Tests.Configuration
{
    [TestClass]
    public class AuditImageFilterTest : InMemoryDatabaseTest<TapImage>
    {
        #region Fields

        private MapCallMvcApplicationTester _appTester;
        private Mock<IDateTimeProvider> _dateProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        private TestAuditImageFilter _target;
        private ActionExecutedContext _actionContext;
        private FakeMvcHttpHandler _request;
        private AssetImagePdfResult _result;
        private TapImage _model;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            e.For<IDateTimeProvider>().Use((_dateProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IRoleService>().Use(new Mock<IRoleService>().Object);
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Use(new Mock<IImageToPdfConverter>().Object);
            e.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            _dateProvider = e.For<IDateTimeProvider>().Mock();
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IViewModelFactory>().Use<ViewModelFactory>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TestAuditImageFilter(_container) {
                EnableBaseLogging = true 
            };

            _user = GetFactory<UserFactory>().Create();

            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _model = GetFactory<TapImageFactory>().Create();
            _result = new AssetImagePdfResult(new byte[] { 1 }, _model);

            _appTester = _container.With(true).GetInstance<MapCallMvcApplicationTester>();
            _request = _appTester.CreateRequestHandler("~/TapImage/Show/42.pdf");

            _actionContext =
                new ActionExecutedContext(
                    _request.CreateAndInitializeController<TapImageController>().ControllerContext,
                    _request.RouteContext.ActionDescriptor, false, null);
            _actionContext.Result = _result;
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _appTester.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestOnActionExecutedDoesNothingIfRequestQueryStringForLogviewEqualsZero()
        {
            _target.EnableBaseLogging = false;
            _request.RequestQueryString["logview"] = "0";
            _target.OnActionExecuted(_actionContext);
            
            Assert.IsFalse(_target.DoLoggingWasCalled);
        }

        [TestMethod]
        public void TestOnActionExecutedDoesLogIfRequestQueryStringForLogViewHasAnythingOtherthanZero()
        {
            _target.EnableBaseLogging = false;
            _request.RequestQueryString["logview"] = "what";
            _target.OnActionExecuted(_actionContext);
            Assert.IsTrue(_target.DoLoggingWasCalled);
        }

        [TestMethod]
        public void TestOnActionExecutedDoesLogIfRequestQueryStringDoesNotHaveLogview()
        {
            _target.EnableBaseLogging = false;
            _request.RequestQueryString.Remove("logview");
            _target.OnActionExecuted(_actionContext);
            Assert.IsTrue(_target.DoLoggingWasCalled);
        }

        [TestMethod]
        public void TestOnActionExecutedDoesNotLogIfActionExecutedContextIsCanceled()
        {
            _target.EnableBaseLogging = false;
            _actionContext.Canceled = true;
            _target.OnActionExecuted(_actionContext);
            Assert.IsFalse(_target.DoLoggingWasCalled);
        }

        [TestMethod]
        public void TestOnActionExecutedDoesNotLogIfResultIsNotAssetImagePdfResult()
        {
            // Using PdfResult since we're only working with a subset of it.
            _actionContext.Result = new PdfResult(new byte[] {1});
            _target.OnActionExecuted(_actionContext);
            Assert.IsFalse(_target.DoLoggingWasCalled);
        }


        [TestMethod]
        public void TestOnActionExecutedCreatesNewUserViewedEntryForTapImages()
        {
            var model = GetFactory<TapImageFactory>().Create();
            _actionContext.Result = new AssetImagePdfResult(new byte[] { 1 }, model);

            _target.OnActionExecuted(_actionContext);
            var viewed = Session.Query<UserViewed>().Single();
            Assert.AreSame(_user, viewed.User);
            Assert.AreSame(model, viewed.TapImage);
        }

        [TestMethod]
        public void TestOnActionExecutedCreatesNewUserViewedEntryForValveImages()
        {
            var model = GetFactory<ValveImageFactory>().Create();
            _actionContext.Result = new AssetImagePdfResult(new byte[] { 1 }, model);

            _target.OnActionExecuted(_actionContext);
            var viewed = Session.Query<UserViewed>().Single();
            Assert.AreSame(_user, viewed.User);
            Assert.AreSame(model, viewed.ValveImage);
        }

        [TestMethod]
        public void TestOnActionExecutedCreatesNewUserViewedEntryForAsBuiltImages()
        {
            var model = GetFactory<AsBuiltImageFactory>().Create();
            _actionContext.Result = new AssetImagePdfResult(new byte[] { 1 }, model);

            _target.OnActionExecuted(_actionContext);
            var viewed = Session.Query<UserViewed>().Single();
            Assert.AreSame(_user, viewed.User);
            Assert.AreSame(model, viewed.AsBuiltImage);
        }

        #endregion

        #region Test class

        private class TestAuditImageFilter : AuditImageFilter
        {
            public bool EnableBaseLogging { get; set; }
            public bool DoLoggingWasCalled { get; set; }

            protected override void DoLogging(ActionExecutedContext filterContext, AssetImagePdfResult pdf)
            {
                DoLoggingWasCalled = true;
                if (EnableBaseLogging)
                {
                    base.DoLogging(filterContext, pdf);
                }
            }

            public TestAuditImageFilter(IContainer container) : base(container) { }
        }

        #endregion
    }
}
