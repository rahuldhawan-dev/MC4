using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MainBreakTestTest
    /// </summary>
    [TestClass]
    public class MainBreakIntegrationTest : WorkOrdersTestClass<MainBreak>, IWorkOrderDependentObjectTest
    {
        #region Additional Test Attributes

        [TestInitialize]
        public void MainBreakIntegrationTestInitialize()
        {
        }

        [TestCleanup]
        public void MainBreakIntegrationTestCleanup()
        {
        }

        #endregion
        #region Exposed Static Methods

        internal static TestMainBreakBuilder GetValidMainBreak()
        {
            return new TestMainBreakBuilder();
        }

        public static void DeleteMainBreak(MainBreak entity)
        {
            var order = entity.WorkOrder;
            //var size = entity.MainSize;
            MainBreakRepository.Delete(entity);
            WorkOrderIntegrationTest.DeleteWorkOrder(order);
            //MainSizeTest.DeleteMainSize(size);
        }

        #endregion

        #region Private Methods

        protected override MainBreak GetValidObject()
        {
            return GetValidMainBreak();
        }

        protected override MainBreak GetValidObjectFromDatabase()
        {
            var br = GetValidObject();
            MainBreakRepository.Insert(br);
            return br;
        }

        protected override void DeleteObject(MainBreak entity)
        {
            DeleteMainBreak(entity);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewMainBreak()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.DoesNotThrow(() => MainBreakRepository.Insert(target));

                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(MainBreak));

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidMainBreak().WithWorkOrder(null);

                MyAssert.Throws(() => MainBreakRepository.Insert(target),
                                typeof(DomainLogicException),
                                "Trying to save a MainBreak without a linked WorkOrder should throw an exception");
            }
        }

        [TestMethod]
        public void TestCannotChangeWorkOrderAfterSave()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();
                var order = new WorkOrder();

                MyAssert.Throws(() => target.WorkOrder = order,
                                typeof(DomainLogicException),
                                "Attempting to change the WorkOrder should throw an exception for a MainBreak object that has already been saved");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestCannotSaveWithoutMaterialValue()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidMainBreak().WithMaterial(null);

                MyAssert.Throws(() => MainBreakRepository.Insert(target),
                                typeof(DomainLogicException),
                                "Trying to save a MainBreak without a valid Material value should throw an exception");
            }
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestCannotSaveWithoutMainCondition()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidMainBreak().WithMainCondition(null);

                MyAssert.Throws(() => MainBreakRepository.Insert(target),
                                typeof(DomainLogicException),
                                "Attempting to save a MainBreak record without a MainCondition should throw an exception");
            }
        }
    }

    internal class TestMainBreakBuilder : TestDataBuilder<MainBreak>
    {
        #region Constants

        
        #endregion

        #region Private Members

        private WorkOrder _workOrder = WorkOrderIntegrationTest.GetValidWorkOrder();
        private MainCondition _mainCondition = MainConditionIntegrationTest.GetValidMainCondition();
        //TODO: these are probably wrong
        private MainBreakDisinfectionMethod _mainBreakDisinfectionMethod = MainBreakDisinfectionMethodIntegrationTest.GetValidMainBreakDisinfectionMethod();
        private MainBreakFlushMethod _mainBreakFlushMethod = MainBreakFlushMethodIntegrationTest.GetValidMainBreakFlushMethod();
        private MainBreakMaterial _mainBreakMaterial = MainBreakMaterialIntegrationTest.GetValidMainBreakMaterial();
        private MainFailureType _mainFailureType = MainFailureTypeIntegrationTest.GetValidMainFailureType();
        private MainBreakSoilCondition _mainBreakSoilCondition = MainBreakSoilConditionIntegrationTest.GetValidMainBreakSoilCondition();
        private decimal _depth = 1;
        private ServiceSize _serviceSize = new ServiceSize {
            Main = true
        };
        #endregion

        #region Exposed Methods

        public override MainBreak Build()
        {
            var mBreak = new MainBreak();
            
            if (_workOrder != null)
                mBreak.WorkOrder = _workOrder;
            if (_mainCondition != null)
                mBreak.MainCondition = _mainCondition;
            if (_mainBreakDisinfectionMethod != null)
                mBreak.MainBreakDisinfectionMethod = _mainBreakDisinfectionMethod;
            if (_mainBreakFlushMethod != null)
                mBreak.MainBreakFlushMethod = _mainBreakFlushMethod;
            if (_mainBreakMaterial != null)
                mBreak.MainBreakMaterial = _mainBreakMaterial;
            if (_mainFailureType != null)
                mBreak.MainFailureType = _mainFailureType;
            if (_mainBreakSoilCondition != null)
                mBreak.MainBreakSoilCondition = _mainBreakSoilCondition;
            if (_depth > 0)
                mBreak.Depth = _depth;
            if (_serviceSize != null)
                mBreak.ServiceSize = _serviceSize;
            return mBreak;
        }

        public TestMainBreakBuilder WithMainCondition(MainCondition mainCondition)
        {
            _mainCondition = mainCondition;
            return this;
        }

        public TestMainBreakBuilder WithMaterial(MainBreakMaterial material)
        {
            _mainBreakMaterial = material;
            return this;
        }

        public TestMainBreakBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }

        #endregion
    }
}