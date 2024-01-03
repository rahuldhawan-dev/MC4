using System.Collections.Specialized;
using System.Web.UI.WebControls;
using LINQTo271.Views.Restorations;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;

namespace _271ObjectTests.Tests.Unit.Views.Restorations
{
    /// <summary>
    /// Summary description for RestorationDetailViewTestTest
    /// </summary>
    [TestClass]
    public class RestorationDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private IObjectContainerDataSource _dataSource;
        private IDetailControl _detailControl;
        private TestRestorationDetailView _target;
        private ISecurityService _securityService;
        private IPage _iPage;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks
                .DynamicMock(out _dataSource)
                .DynamicMock(out _detailControl)
                .DynamicMock(out _securityService)
                .DynamicMock(out _iPage);

            _target = new TestRestorationDetailViewBuilder()
                .WithDataSource(_dataSource)
                .WithDetailControl(_detailControl)
                .WithSecurityService(_securityService)
                .WithIPage(_iPage);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestDataSourcePropertyReturnsDataSource()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_dataSource, _target.DataSource);
        }

        [TestMethod]
        public void TestDetailControlPropertyReturnsDetailControl()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_detailControl, _target.DetailControl);
        }

        #region Event Handlers
        [TestMethod]
        public void TestfvRestorationInsertingSetsInputParameters()
        {
            int expectedWorkOrderID = 1;
            int currentUserID = 1;
            string dateApproved = "1/1/2000";
            string dateRejected = "1/2/2000";

            var args =
                new FormViewInsertEventArgs(new OrderedDictionary());

            _target.WorkOrderID = expectedWorkOrderID;
            args.Values[RestorationDetailView.EntityKeys.DATE_APPROVED] = dateApproved;
            args.Values[RestorationDetailView.EntityKeys.DATE_REJECTED] = dateRejected;
            using (_mocks.Record())
            {
                //Setup results for shiz.
                SetupResult.For(_iPage.IsValid).Return(true);
                SetupResult.For(_securityService.GetEmployeeID()).Return(currentUserID);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvRestoration_ItemInserting",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(expectedWorkOrderID, args.Values[RestorationDetailView.EntityKeys.WORK_ORDER_ID]);
                Assert.AreEqual(currentUserID, args.Values[RestorationDetailView.EntityKeys.APPROVED_BY_ID]);
                Assert.AreEqual(currentUserID, args.Values[RestorationDetailView.EntityKeys.REJECTED_BY_ID]);
            }
        }

        [TestMethod]
        public void TestfvRestorationUpdatingSetsApprovedAndRejectedByIDs()
        {
            int expectedWorkOrderID = 1;
            int currentUserID = 1;
            string oldDateApproved = "1/1/2000";
            string newDateApproved = "2/2/2000";

            string oldDateRejected = "1/1/2000";
            string newDateRejected = "2/2/2000";


            var e =
                new FormViewUpdateEventArgs(new OrderedDictionary());

            _target.WorkOrderID = expectedWorkOrderID;
            e.OldValues[RestorationDetailView.EntityKeys.DATE_APPROVED] = oldDateApproved;
            e.NewValues[RestorationDetailView.EntityKeys.DATE_APPROVED] = newDateApproved;
            e.OldValues[RestorationDetailView.EntityKeys.DATE_REJECTED] = oldDateRejected;
            e.NewValues[RestorationDetailView.EntityKeys.DATE_REJECTED] = newDateRejected;


            using (_mocks.Record())
            {
                //Setup results for shiz.
                SetupResult.For(_iPage.IsValid).Return(true);
                SetupResult.For(_securityService.GetEmployeeID()).Return(currentUserID);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvRestoration_ItemUpdating",
                    new object[] {
                        null, e
                    });

                Assert.AreEqual(currentUserID, e.NewValues[RestorationDetailView.EntityKeys.APPROVED_BY_ID]);
                Assert.AreEqual(currentUserID, e.NewValues[RestorationDetailView.EntityKeys.REJECTED_BY_ID]);
            }
        }
        #endregion
    }

    internal class TestRestorationDetailViewBuilder : TestDataBuilder<TestRestorationDetailView>
    {
        #region Private Members

        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private ISecurityService _securityService;
        private IPage _iPage;

        #endregion

        #region Exposed Methods

        public override TestRestorationDetailView Build()
        {
            var obj = new TestRestorationDetailView();
            if (_detailControl != null)
                obj.SetDetailControl(_detailControl);
            if (_dataSource != null)
                obj.SetDataSource(_dataSource);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            if (_iPage != null)
                obj.SetIPage(_iPage);
            return obj;
        }

        public TestRestorationDetailViewBuilder WithDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            return this;
        }

        public TestRestorationDetailViewBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        public TestRestorationDetailViewBuilder WithSecurityService(ISecurityService service)
        {
            _securityService = service;
            return this;
        }

        public TestRestorationDetailViewBuilder WithIPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        #endregion

    }

    internal class TestRestorationDetailView : RestorationDetailView
    {
        #region Exposed Methods

        public void SetDetailControl(IDetailControl detailControl)
        {
            fvRestoration = detailControl;
        }

        public void SetDataSource(IObjectContainerDataSource dataSource)
        {
            odsRestoration = dataSource;
        }

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        public void SetIPage(IPage page)
        {
            _iPage = page;
        }
        #endregion
    }
}