using System;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MainBreakMaterialIntegrationTest
    /// </summary>
    [TestClass]
    public class MainBreakMaterialIntegrationTest : WorkOrdersTestClass<MainBreakMaterial>
    {
        public static MainBreakMaterial GetValidMainBreakMaterial()
        {
            return MainBreakMaterialRepository.AsbestosCement;
        }

        #region Overrides of LinqUnitTestClass<MainBreakMaterial>

        protected override MainBreakMaterial GetValidObject()
        {
            return GetValidMainBreakMaterial();
        }

        protected override MainBreakMaterial GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(MainBreakMaterial entity)
        {
            throw new NotImplementedException();
        }

        #endregion
        [TestMethod]
        public void TestCannotCreateNewMainBreakMaterialWithoutDescription()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new MainBreakMaterial
                {
                    Description = string.Empty
                };

                MyAssert.Throws(
                    () => MainBreakMaterialRepository.Insert(target),
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
