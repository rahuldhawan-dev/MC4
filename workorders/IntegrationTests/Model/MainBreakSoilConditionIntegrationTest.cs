using System;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MainBreakSoilConditionIntegrationTest
    /// </summary>
    [TestClass]
    public class MainBreakSoilConditionIntegrationTest : WorkOrdersTestClass<MainBreakSoilCondition>
    {
        public static MainBreakSoilCondition GetValidMainBreakSoilCondition()
        {
            return MainBreakSoilConditionRepository.Clay;
        }

        #region Overrides of LinqUnitTestClass<MainBreakSoilCondition>

        protected override MainBreakSoilCondition GetValidObject()
        {
            return GetValidMainBreakSoilCondition();
        }

        protected override MainBreakSoilCondition GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(MainBreakSoilCondition entity)
        {
            throw new NotImplementedException();
        }

        #endregion
        [TestMethod]
        public void TestCannotCreateNewMainBreakSoilConditionWithoutDescription()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new MainBreakSoilCondition
                {
                    Description = string.Empty
                };

                MyAssert.Throws(
                    () => MainBreakSoilConditionRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }
    }
}
