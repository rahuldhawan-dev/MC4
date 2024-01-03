using LINQTo271.Views.WorkOrders.ReadOnly;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.ReadOnly
{
    /// <summary>
    /// Summary description for WorkOrderReadOnlyDetailViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderReadOnlyDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private IObjectContainerDataSource _dataSource;
        private IDetailControl _detailControl;
        private TestWorkOrderReadOnlyDetailView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks
                .DynamicMock(out _detailControl)
                .DynamicMock(out _dataSource);
            _target = new TestWorkOrderReadOnlyDetailViewBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestDetailControlPropertyReturnDetailControl()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_detailControl, _target.DetailControl);
        }

        [TestMethod]
        public void TestDataSourcePropertyReturnsDataSource()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_dataSource, _target.DataSource);
        }

        [TestMethod]
        public void TestPhasePropertyDenotesFinalization()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);
        }
    }

    internal class TestWorkOrderReadOnlyDetailViewBuilder : TestDataBuilder<TestWorkOrderReadOnlyDetailView>
    {
        #region Private Members

        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderReadOnlyDetailView Build()
        {
            var obj = new TestWorkOrderReadOnlyDetailView();
            if (_detailControl != null)
                obj.SetDetailControl(_detailControl);
            if (_dataSource != null)
                obj.SetDataSource(_dataSource);
            return obj;
        }

        public TestWorkOrderReadOnlyDetailViewBuilder WithDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            return this;
        }

        public TestWorkOrderReadOnlyDetailViewBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderReadOnlyDetailView : WorkOrderReadOnlyDetailView
    {
        #region Exposed Methods

        public void SetDetailControl(IDetailControl detailControl)
        {
            fvWorkOrder = detailControl;
        }

        public void SetDataSource(IObjectContainerDataSource dataSource)
        {
            odsWorkOrder = dataSource;
        }

        #endregion
    }
}