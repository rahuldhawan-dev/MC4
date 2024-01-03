using System;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Abstract
{
    /// <summary>
    /// Summary description for WorkOrderApprovalDetailViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderApprovalDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private TestWorkOrderApprovalDetailView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _detailControl)
                .DynamicMock(out _dataSource);

            _target = new TestWorkOrderApprovalDetailViewBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource);
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestPhasePropertyDenotesApproval()
        {
            Assert.AreEqual(WorkOrderPhase.Approval, _target.Phase);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDetailControlPropertyReturnsDetailControl()
        {
            Assert.AreSame(_detailControl, _target.DetailControl);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDataSourcePropertyReturnsDataSource()
        {
            Assert.AreSame(_dataSource, _target.DataSource);

            _mocks.ReplayAll();
        }

	    #endregion

    }

    internal class TestWorkOrderApprovalDetailViewBuilder : TestDataBuilder<TestWorkOrderApprovalDetailView>
    {
        #region Private Members

        private IDetailControl _detailControl = new MvpFormView();
        private IObjectContainerDataSource _dataSource =
            new MvpObjectContainerDataSource();
        private EventHandler _onSaveClicked;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderApprovalDetailView Build()
        {
            var obj = new TestWorkOrderApprovalDetailView();
            if (_detailControl != null)
                obj.SetDetailControl(_detailControl);
            if (_dataSource != null)
                obj.SetDataSource(_dataSource);

            return obj;
        }

        public TestWorkOrderApprovalDetailViewBuilder WithDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            return this;
        }

        public TestWorkOrderApprovalDetailViewBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        public TestWorkOrderApprovalDetailViewBuilder WithSaveClickedEventHandler(EventHandler onSaveClicked)
        {
            _onSaveClicked = onSaveClicked;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderApprovalDetailView : WorkOrderApprovalDetailView
    {
        #region Delegates

        public delegate void OnDisposeHandler(TestWorkOrderApprovalDetailView dv);

        #endregion

        #region Events

        public OnDisposeHandler OnDispose;

        #endregion

        #region Exposed Methods

        public void SetDetailControl(IDetailControl control)
        {
            fvWorkOrder = control;
        }

        public void SetDataSource(IObjectContainerDataSource source)
        {
            odsWorkOrder = source;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (OnDispose != null)
                OnDispose(this);
        }

        #endregion
    }
}
