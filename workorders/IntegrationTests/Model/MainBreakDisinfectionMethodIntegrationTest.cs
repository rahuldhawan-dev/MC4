using System;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MainBreakDisinfectionMethodIntegrationTest
    /// </summary>
    [TestClass]
    public class MainBreakDisinfectionMethodIntegrationTest : WorkOrdersTestClass<MainBreakDisinfectionMethod>
    {
        public static MainBreakDisinfectionMethod GetValidMainBreakDisinfectionMethod()
        {
            return MainBreakDisinfectionMethodRepository.DisinfectionOfFittingsPipe;
        }

        #region Overrides of LinqUnitTestClass<MainBreakDisinfectionMethod>

        protected override MainBreakDisinfectionMethod GetValidObject()
        {
            return GetValidMainBreakDisinfectionMethod();
        }

        protected override MainBreakDisinfectionMethod GetValidObjectFromDatabase()
        {
           return GetValidObject();
        }

        protected override void DeleteObject(MainBreakDisinfectionMethod entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        [TestMethod]
        public void TestCannotCreateNewMainBreakDisinfectionMethodWithoutDescription()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new MainBreakDisinfectionMethod {
                    Description = string.Empty
                };

                MyAssert.Throws(
                    () => MainBreakDisinfectionMethodRepository.Insert(target),
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
