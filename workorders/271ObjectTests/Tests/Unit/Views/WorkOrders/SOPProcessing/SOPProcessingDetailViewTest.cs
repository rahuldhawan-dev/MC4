using LINQTo271.Views.WorkOrders.SOPProcessing;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SOPProcessing
{
    /// <summary>
    /// Summary description for SOPProcessingDetailViewTest.
    /// </summary>
    [TestClass]
    public class SOPProcessingDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestSOPProcessingDetailView _target;
        private IObjectContainerDataSource _dataSource;
        private IDetailControl _detailControl;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SOPProcessingDetailViewTestInitialize()
        {
            _mocks.DynamicMock(out _dataSource)
                .DynamicMock(out _detailControl);

            _target = new TestSOPProcessingDetailViewBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource);
        }

        #endregion

        [TestMethod]
        public void TestPhasePropertyDenotesFinalization()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);
        }

        [TestMethod]
        public void TestDataSourceProperty()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(_target.DataSource, _dataSource);
        }

        [TestMethod]
        public void TestDetailControlProperty()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(_target.DetailControl, _detailControl);
        }
    }

    internal class TestSOPProcessingDetailViewBuilder : TestDataBuilder<TestSOPProcessingDetailView>
    {
        private IObjectContainerDataSource _odsWorkOrder;
        private IDetailControl _detailControl;

        #region Exposed Methods

        public override TestSOPProcessingDetailView Build()
        {
            var obj = new TestSOPProcessingDetailView();
            if (_odsWorkOrder != null)
                obj.SetODSWorkOrder(_odsWorkOrder);
            if(_detailControl != null)
                obj.SetDetailControl(_detailControl);
            return obj;
        }

        #endregion

        internal TestSOPProcessingDetailViewBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _odsWorkOrder = dataSource;
            return this;
        }

        internal TestSOPProcessingDetailViewBuilder WithDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            return this;
        }
    }

    internal class TestSOPProcessingDetailView : SOPProcessingDetailView
    {
        public void SetODSWorkOrder(IObjectContainerDataSource ods)
        {
            odsWorkOrder = ods;
        }

        internal void SetDetailControl(IDetailControl detailControl)
        {
            fvWorkOrder = detailControl;
        }
    }
}
