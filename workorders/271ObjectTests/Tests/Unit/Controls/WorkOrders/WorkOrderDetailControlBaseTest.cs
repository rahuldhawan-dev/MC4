using System;
using LINQTo271.Controls.WorkOrders;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Views.WorkOrders;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderControlBaseTest.
    /// </summary>
    [TestClass]
    public class WorkOrderDetailControlBaseTest : EventFiringTestClass
    {
        #region Private Members

        private ISecurityService _securityService;
        private IViewState _viewState;
        private TestWorkOrderControlBase _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _viewState)
                .DynamicMock(out _securityService);

            _target = new TestWorkOrderControlBaseBuilder()
                .WithViewState(_viewState)
                .WithSecurityService(_securityService);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests
        
        [TestMethod]
        public void TestSecurityServicePropertyReturnsMockedValueIfPresent()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_securityService,
                _target.GetPropertyValueByName("SecurityService"));
        }

        [TestMethod]
        public void TestSecurityServicePropertyGetsSecurityServiceSingletonInstance()
        {
            _mocks.ReplayAll();

            _target.SetSecurityService(null);

            Assert.AreSame(SecurityService.Instance,
                _target.GetPropertyValueByName("SecurityService"));
        }

        [TestMethod]
        public void TestSettingWorkOrderIDSetsValueInViewStateAndCallsSetDataSource()
        {
            var expected = 5;

            using (_mocks.Record())
            {
                _viewState.SetValue(
                    WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID,
                    expected);
            }

            using (_mocks.Playback())
            {
                _target.WorkOrderID = expected;

                Assert.IsTrue(_target.SetDataSourceCalled);
                Assert.IsTrue(_target.SetDataSourceArgMatched);
            }
        }

        [TestMethod]
        public void TestGettingWorkOrderIDRetrievesValueFromViewState()
        {
            var expected = 6;

            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.WorkOrderID);
            }
        }

        [TestMethod]
        public void TestCurrentMvpModeObservesClassDefaultViewMode()
        {
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                using (_mocks.Record())
                {
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(null);
                }


                using (_mocks.Playback())
                {
                    _target.SetClassDetaultViewMode(mode);

                    Assert.AreEqual(mode, _target.CurrentMvpMode);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDefaultDetailViewModeIsReadOnly()
        {
            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.
                            CURRENT_MVP_MODE)).Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(DetailViewMode.ReadOnly, _target.CurrentMvpMode);
            }
        }

        [TestMethod]
        public void TestSettingInitialModeOverridesDetault()
        {
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {

                using (_mocks.Record())
                {
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(null);
                }

                using (_mocks.Playback())
                {
                    _target.InitialMode = mode;

                    Assert.AreEqual(mode, _target.CurrentMvpMode);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSettingMvpModeOverridesInitialAndDefault()
        {
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                using (_mocks.Record())
                {
                    _viewState.SetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.
                            CURRENT_MVP_MODE, mode);
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(mode);
                }

                using (_mocks.Playback())
                {
                    _target.InitialMode = DetailViewMode.ReadOnly;
                    _target.ChangeMvpMode(mode);


                    Assert.AreEqual(mode, _target.CurrentMvpMode);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDataKeyPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.DataKey);
        }

        [TestMethod]
        public void TestDataItemPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.DataItem);
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestChangeCurrentMvpModeUsesViewStateToTrackMode()
        {
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                using (_mocks.Record())
                {
                    _viewState.SetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.
                            CURRENT_MVP_MODE, mode);
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(mode);
                }

                using (_mocks.Playback())
                {
                    _target.ChangeMvpMode(mode);

                    Assert.AreEqual(mode, _target.CurrentMvpMode);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDeleteItemNotImplemented()
        {
            _mocks.ReplayAll();

            MyAssert.Throws(() => _target.DeleteItem());
        }

        [TestMethod]
        public void TestInsertItemNotImplemented()
        {
            _mocks.ReplayAll();

            MyAssert.Throws(() => _target.InsertItem(false));
            MyAssert.Throws(() => _target.InsertItem(true));
        }

        [TestMethod]
        public void TestUpdateItemNotImplemented()
        {
            _mocks.ReplayAll();

            MyAssert.Throws(() => _target.UpdateItem(false));
            MyAssert.Throws(() => _target.UpdateItem(true));
        }

        #endregion
    }

    internal class TestWorkOrderControlBaseBuilder : TestDataBuilder<TestWorkOrderControlBase>
    {
        #region Private Members

        private ISecurityService _securityService;
        private IViewState _viewState;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderControlBase Build()
        {
            var obj = new TestWorkOrderControlBase();
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestWorkOrderControlBaseBuilder WithViewState(IViewState viewState)
        {
            _viewState = viewState;
            return this;
        }

        public TestWorkOrderControlBaseBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderControlBase : WorkOrderDetailControlBase
    {
        #region Private Members

        private int? _expectedWorkOrderID;

        #endregion

        #region Properties

        public bool SetDataSourceCalled { get; protected set; }
        public bool SetDataSourceArgMatched { get; protected set; }

        #endregion

        #region Private Methods

        protected override void SetDataSource(int workOrderID)
        {
            SetDataSourceCalled = true;
            SetDataSourceArgMatched = (_expectedWorkOrderID == null ||
                                       _expectedWorkOrderID.Value == workOrderID);
        }

        #endregion

        #region Exposed Methods

        public void ExpectWorkOrderID(int workOrderID)
        {
            _expectedWorkOrderID = workOrderID;
        }

        public void SetViewState(IViewState viewState)
        {
            _iViewState = viewState;
        }

        public void SetClassDetaultViewMode(DetailViewMode viewMode)
        {
            _classDefaultViewMode = viewMode;
        }

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        #endregion
    }
}
