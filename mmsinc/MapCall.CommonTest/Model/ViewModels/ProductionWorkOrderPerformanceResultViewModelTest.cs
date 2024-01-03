using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.ViewModels
{
    [TestClass]
    public class ProductionWorkOrderPerformanceResultViewModelTest
    {
        #region Fields

        private ProductionWorkOrderPerformanceResultViewModel _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new ProductionWorkOrderPerformanceResultViewModel();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPercentUnscheduledIsPercentageOfUnscheduledCreatedOrders()
        {
            _target.NumberCreated = 10;
            _target.NumberUnscheduled = 5;
            Assert.AreEqual(0.5m, _target.PercentUnscheduled);
        }

        [TestMethod]
        public void TestPercentCanceledIsPercentageOfCanceledCreatedOrders()
        {
            _target.NumberCreated = 10;
            _target.NumberCanceled = 5;
            Assert.AreEqual(0.5m, _target.PercentCanceled);
        }

        [TestMethod]
        public void TestTestPercentCompletedIsPercentageOfCompletedCreatedOrders()
        {
            _target.NumberCreated = 10;
            _target.NumberCompleted = 5;
            Assert.AreEqual(0.5m, _target.PercentCompleted);
        }

        #endregion
    }
}
