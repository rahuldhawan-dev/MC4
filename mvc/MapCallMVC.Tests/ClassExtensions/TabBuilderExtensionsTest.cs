using System.Web.Mvc;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using Moq;
using NHibernate.Intercept;
using StructureMap;

namespace MapCallMVC.Tests.ClassExtensions
{
    [TestClass]
    public class TabBuilderExtensionsTest
    {
        #region Consts

        private const string PARTIAL_VIEW_NAME = "Partial",
                             PARTIAL_VIEW_CONTENT = "content of partial view";

        #endregion

        #region Fields

        private FakeMvcApplicationTester _application;
        private FakeMvcHttpHandler _request;
        private TabBuilder _target;
        private HtmlHelper _helper;
        private WithTabModel _model;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _application = new FakeMvcApplicationTester(new Container());
            _request = _application.CreateRequestHandler();
            _model = new WithTabModel();
            _helper = _request.CreateHtmlHelper<WithTabModel>(_model);
            _target = new TabBuilder(_helper);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _application.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestWithLogReturnsAnAjaxTabWithTheExpectedModelAndView()
        {
            _model.Id = 42;
            _target.WithLog();
            var logTab = _target.GetTabById("LogTab"); // WithLog defaults to using "Log" which then becomes "LogTab".
            Assert.IsTrue(logTab.IsAjaxTab);
            Assert.IsFalse(logTab.UseParentViewModel);

            var tabModel = (SecureSearchAuditLogEntryForSingleRecord)logTab.PartialModel;
            Assert.AreEqual(42, tabModel.EntityId);
            Assert.AreEqual("WithTabModel", tabModel.EntityTypeName);
            Assert.AreEqual("Crud", tabModel.ControllerName);
        }

        [TestMethod]
        public void TestWithLogWithProxyEntityLoadsForEntityWithTheCorrectName()
        {
            // we need to make this class pretend it's an nHibernate proxy for GuessClass
            var accessor = new Mock<IFieldInterceptorAccessor>();
            var fieldInterceptor = new Mock<IFieldInterceptor>();
            fieldInterceptor.Setup(x => x.MappedClass).Returns(typeof(WithTabModel));
            accessor.SetupGet(x => x.FieldInterceptor).Returns(fieldInterceptor.Object);

            var model = new WithTabModelProxy(accessor.Object);
            _helper = _request.CreateHtmlHelper<WithTabModel>(model);
            _target = new TabBuilder(_helper);
            _target.WithLog();
            var logTab = _target.GetTabById("LogTab"); // WithLog defaults to using "Log" which then becomes "LogTab".

            var tabModel = (SecureSearchAuditLogEntryForSingleRecord)logTab.PartialModel;
            Assert.AreEqual("WithTabModel", tabModel.EntityTypeName);
            Assert.AreEqual("Crud", tabModel.ControllerName);
        }

        [TestMethod]
        public void TestWithActionItemsReturnsAnAjaxTabWithTheExpectedModelAndView()
        {
            _model.Id = 404;
            _target.WithActionItems();
            var actionItemsTab = _target.GetTabById("ActionItemsTab");
            Assert.IsTrue(actionItemsTab.IsAjaxTab);
            Assert.IsTrue(actionItemsTab.UseParentViewModel);
        }

        [TestMethod]
        public void TestWithNotesDocumentsAndActionItemsReturnsAllThreeOnView()
        {
            _model.Id = 404;
            _target.WithNotesDocumentsAndActionItems();
            var ActionItems = _target.GetTabById("ActionItemsTab");
            var Documents = _target.GetTabById("DocumentsTab");
            var Notes = _target.GetTabById("NotesTab");

            Assert.IsNotNull(ActionItems);
            Assert.IsNotNull(Documents);
            Assert.IsNotNull(Notes);
        }

        #endregion

        #region Helper classes

        // This class must be public in order for the WithLog test to function.
        // WithLog uses dynamic to access the Id property which it can't do if
        // the class itself is non-public.
        public class WithTabModel
        {
            public int Id { get; set; }
        }

        public class WithTabModelProxy : WithTabModel, IFieldInterceptorAccessor
        {
            public IFieldInterceptor FieldInterceptor { get; set; }

            public WithTabModelProxy(IFieldInterceptorAccessor accessor)
            {
                FieldInterceptor = accessor.FieldInterceptor;
            }
        }
        
        #endregion
    }
}
