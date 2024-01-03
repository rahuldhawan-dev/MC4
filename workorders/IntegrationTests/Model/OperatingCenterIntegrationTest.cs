using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for OperatingCenterTestTest
    /// </summary>
    [TestClass]
    public class OperatingCenterIntegrationTest : WorkOrdersTestClass<OperatingCenter>
    {
        #region Constants

        // NJ7
        private const int REFERENCE_OPERATING_CENTER_ID = 10;

        #endregion

        #region Exposed Static Methods

        public static OperatingCenter GetValidOperatingCenter()
        {
            return
                OperatingCenterRepository.GetEntity(
                    REFERENCE_OPERATING_CENTER_ID);
        }

        public static void DeleteOperatingCenter(OperatingCenter entity)
        {
            OperatingCenterRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override OperatingCenter GetValidObject()
        {
            return new TestOperatingCenterBuilder();
        }

        protected override OperatingCenter GetValidObjectFromDatabase()
        {
            return GetValidOperatingCenter();
        }

        protected override void DeleteObject(OperatingCenter entity)
        {
            DeleteOperatingCenter(entity);
        }

        #endregion

        [TestMethod]
        public void TestCannotCreateNewOperatingCenter()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(() => OperatingCenterRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotAlterOperatingCenter()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();

                target.OpCntr = "NJ15";

                MyAssert.Throws(() => OperatingCenterRepository.Update(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteOperatingCenter()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();

                MyAssert.Throws(() => OperatingCenterRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestOperatingCenterHasTowns()
        {
            using (_simulator.SimulateRequest())
            {
                var opcntr = GetValidObjectFromDatabase();
                MyAssert.IsGreaterThan(opcntr.OperatingCentersTowns.Count, 1);
                var town = TownIntegrationTest.GetValidTown();
                Assert.IsTrue(opcntr.Towns.Contains(town));
            }
        }
    }

    public class TestOperatingCenterBuilder : TestDataBuilder<OperatingCenter>
    {
        #region Exposed Methods

        public override OperatingCenter Build()
        {
            var cntr = new OperatingCenter();
            return cntr;
        }

        #endregion
    }
}