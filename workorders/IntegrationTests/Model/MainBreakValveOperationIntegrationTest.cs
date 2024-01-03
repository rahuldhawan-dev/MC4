using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;
using _271ObjectTests.Tests.Unit.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MainBreakValveOperationTestTest
    /// </summary>
    [TestClass]
    public class MainBreakValveOperationIntegrationTest : WorkOrdersTestClass<MainBreakValveOperation>
    {
        #region Exposed Static Methods

        internal static TestMainBreakValveOperationBuilder GetValidMainBreakValveOperation()
        {
            return new TestMainBreakValveOperationBuilder();
        }

        public static void DeleteMainBreakValveOperation(MainBreakValveOperation entity)
        {
            var br = entity.MainBreak;
            MainBreakValveOperationRepository.Delete(entity);
            MainBreakIntegrationTest.DeleteMainBreak(br);
        }

        #endregion

        #region Private Methods

        protected override MainBreakValveOperation GetValidObject()
        {
            return GetValidMainBreakValveOperation();
        }

        protected override MainBreakValveOperation GetValidObjectFromDatabase()
        {
            var obj = GetValidObject();
            MainBreakValveOperationRepository.Insert(obj);
            return obj;
        }

        protected override void DeleteObject(MainBreakValveOperation entity)
        {
            DeleteMainBreakValveOperation(entity);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewMainBreakValveOperation()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.DoesNotThrow(() => MainBreakValveOperationRepository.Insert(target));

                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(MainBreakValveOperation));

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestCannotSaveWithoutMainBreak()
        {
            using (_simulator.SimulateRequest())
            {
                var target =
                    GetValidMainBreakValveOperation().WithMainBreak(null);

                MyAssert.Throws(
                    () => MainBreakValveOperationRepository.Insert(target),
                    typeof(DomainLogicException),
                    "Attempting to save a MainBreakValveOperation without a MainBreak should throw an exception");
            }
        }

        [TestMethod]
        public void TestCannotChangeMainBreakAfterSave()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();
                var mBreak = new MainBreak();

                MyAssert.Throws(() => target.MainBreak = mBreak,
                                typeof(DomainLogicException),
                                "Attempting to change the MainBreak of a given MainBreakValveOperation after it has been set should throw an exception");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestCannotSaveWithoutValve()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidMainBreakValveOperation().WithValve(null);

                MyAssert.Throws(() => MainBreakValveOperationRepository.Insert(target),
                                typeof(DomainLogicException),
                                "Attempting to save a MainBreakValveOperation without a Valve should throw an exception");
            }
        }
    }

    internal class TestMainBreakValveOperationBuilder : TestDataBuilder<MainBreakValveOperation>
    {
        #region Private Members

        private Valve _valve = ValveTest.GetValidValve();

        private MainBreak _mainBreak = MainBreakIntegrationTest.GetValidMainBreak();

        #endregion

        #region Exposed Methods

        public override MainBreakValveOperation Build()
        {
            var op = new MainBreakValveOperation();
            if (_valve != null)
                op.Valve = _valve;
            if (_mainBreak != null)
                op.MainBreak = _mainBreak;
            return op;
        }

        public TestMainBreakValveOperationBuilder WithMainBreak(MainBreak mainBreak)
        {
            _mainBreak = mainBreak;
            return this;
        }

        public TestMainBreakValveOperationBuilder WithValve(Valve valve)
        {
            _valve = valve;
            return this;
        }

        #endregion
    }
}