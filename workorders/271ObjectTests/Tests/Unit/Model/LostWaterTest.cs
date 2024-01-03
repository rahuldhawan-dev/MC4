using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Subtext.TestLibrary;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for LostWaterTest
    /// </summary>
    [TestClass]
    public class LostWaterTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private LostWater _target;
        private IRepository<LostWater> _repository;
        #endregion

        [TestInitialize]
        public void LostWaterTestInitialize()
        {
            _simulator = new HttpSimulator();
            _simulator = _simulator.SimulateRequest();
            _target = new TestLostWaterBuilder();
            _repository = new MockLostWaterRepository();
        }

        [TestCleanup]
        public void LostWaterTestCleanup()
        {
            _simulator.Dispose();
        }

        #region Exposed Static Methods


        #endregion

        #region Private Methods


        #endregion

        [TestMethod]
        public void TestCreateNewLostWater()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));

            Assert.IsNotNull(_target);
            Assert.IsInstanceOfType(_target, typeof(LostWater));
        }

        [TestMethod]
        public void TestCannotSaveWithGallonsLessThanOne()
        {
            _target = new TestLostWaterBuilder().WithGallons(0);

            MyAssert.Throws(() => _repository.InsertNewEntity(_target),
                typeof(DomainLogicException),
                "Attempting to save a LostWater object with less than one Gallon should throw an exception");
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            _target = new TestLostWaterBuilder()
                .WithWorkOrder(null);
            MyAssert.Throws(() => _repository.InsertNewEntity((_target)),
                typeof(DomainLogicException),
                "Trying to save a LostWater object that's not linked to a WorkOrder should throw an exception");
        }

        [TestMethod]
        public void TestCannotChangeWorkOrderAfterSave()
        {
            _repository.InsertNewEntity(_target);
            var order = new WorkOrder();

            MyAssert.Throws(() => _target.WorkOrder = order,
                typeof(DomainLogicException),
                "Attempting to change the WorkOrder should throw an exception for a LostWater object that has already been saved");
        }
    }

    internal class TestLostWaterBuilder : TestDataBuilder<LostWater>
    {
        #region Constants

        private const short GALLONS = 1;

        #endregion

        #region Private Members

        private int? _gallons = GALLONS;
        private WorkOrder _workOrder = new WorkOrder(); // WorkOrderTest.GetValidWorkOrder();

        #endregion

        #region Private Methods

        private void SetGallons(LostWater lostWater)
        {
            var fi = lostWater.GetType().GetField("_gallons",
                BindingFlags.Instance | BindingFlags.NonPublic);
            fi.SetValue(lostWater, _gallons);
        }

        #endregion

        #region Exposed Methods

        public override LostWater Build()
        {
            var water = new LostWater();
            if (_gallons != null)
                SetGallons(water);
            if (_workOrder != null)
                water.WorkOrder = _workOrder;
            return water;
        }

        public TestLostWaterBuilder WithGallons(int? gallons)
        {
            _gallons = gallons;
            return this;
        }

        public TestLostWaterBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }

        #endregion
    }

    internal class MockLostWaterRepository : MockRepository<LostWater>
    { }

}
