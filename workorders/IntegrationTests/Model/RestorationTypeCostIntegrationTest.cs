using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for RestorationTypeCostIntegrationTestTestTest
    /// </summary>
    [TestClass]
    public class RestorationTypeCostIntegrationTest
    {
        #region Constants

        public const short MIN_SAMPLE_COUNT = 8;

        #endregion

        #region Private Members

        private HttpSimulator _simulator;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationTypeCostIntegrationTestTestInitialize()
        {
            _simulator = new HttpSimulator().SimulateRequest();
        }

        [TestCleanup]
        public void TestorationMethodCostIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestSampleCount()
        {
            MyAssert.IsGreaterThanOrEqualTo(
                new RestorationTypeCostRepository().Count(), MIN_SAMPLE_COUNT);
        }
    }

    internal class TestRestorationTypeCostBuilder : TestDataBuilder<RestorationTypeCost>
    {
        #region Private Members

        private OperatingCenter _operatingCenter =
            OperatingCenterIntegrationTest.GetValidOperatingCenter();

        private RestorationType _restorationType =
            RestorationTypeIntegrationTest.GetValidRestorationType();

        private double? _cost = 1.0;
        private int? _finalCost = 2;

        #endregion

        #region Exposed Methods

        public override RestorationTypeCost Build()
        {
            var obj = new RestorationTypeCost();
            if (_operatingCenter != null)
                obj.OperatingCenter = _operatingCenter;
            if (_restorationType != null)
                obj.RestorationType = _restorationType;
            if (_cost != null)
                obj.Cost = _cost.Value;
            if (_finalCost != null)
                obj.FinalCost = _finalCost.Value;
            return obj;
        }

        public TestRestorationTypeCostBuilder WithOperatingCenter(OperatingCenter operatingCenter)
        {
            _operatingCenter = operatingCenter;
            return this;
        }

        public TestRestorationTypeCostBuilder WithRestorationType(RestorationType restorationType)
        {
            _restorationType = restorationType;
            return this;
        }

        public TestRestorationTypeCostBuilder WithCost(double? cost)
        {
            _cost = cost;
            return this;
        }

        public TestRestorationTypeCostBuilder WithFinalCost(int finalCost)
        {
            _finalCost = finalCost;
            return this;
        }

        #endregion
    }
}