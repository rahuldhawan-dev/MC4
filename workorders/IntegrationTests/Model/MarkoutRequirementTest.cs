using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MarkoutRequirementTestTest
    /// </summary>
    [TestClass]
    public class MarkoutRequirementTest : WorkOrdersTestClass<MarkoutRequirement>
    {
        #region Exposed Static Methods

        public static MarkoutRequirement GetValidMarkoutRequirement()
        {
            return MarkoutRequirementRepository.Routine;
        }

        public static void DeleteMarkoutRequirement(MarkoutRequirement entity)
        {
            MarkoutRequirementRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override MarkoutRequirement GetValidObject()
        {
            return GetValidMarkoutRequirement();
        }

        protected override MarkoutRequirement GetValidObjectFromDatabase()
        {
            var obj = GetValidObject();
            MarkoutRequirementRepository.Insert(obj);
            return obj;
        }

        protected override void DeleteObject(MarkoutRequirement entity)
        {
            DeleteMarkoutRequirement(entity);
        }

        #endregion

        [TestMethod]
        public void TestCannotCreateNewMarkoutRequirement()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new MarkoutRequirement {
                    Description = "Test",
                };

                MyAssert.Throws(
                    () => MarkoutRequirementRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotAlterMarkoutRequirement()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.Description = "Test";

                MyAssert.Throws(
                    () => MarkoutRequirementRepository.Update(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteMarkoutRequirement()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(
                    () => MarkoutRequirementRepository.Delete(target),
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