using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MainConditionTestTest
    /// </summary>
    [TestClass]
    public class MainConditionIntegrationTest : WorkOrdersTestClass<MainCondition>
    {
        #region Exposed Static Methods

        public static MainCondition GetValidMainCondition()
        {
            return MainConditionRepository.Good;
        }

        public static void DeleteMainCondition(MainCondition entity)
        {
            MainConditionRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override MainCondition GetValidObject()
        {
            return GetValidMainCondition();
        }

        protected override MainCondition GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(MainCondition entity)
        {
            DeleteMainCondition(entity);
        }
        
        #endregion

        [TestMethod]
        public void TestCannotCreateNewMainCondition()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new MainCondition {
                    Description = "TestCondition"
                };

                MyAssert.Throws(
                    () => MainConditionRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotAlterMainCondition()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.Description = "Test Description";

                MyAssert.Throws(() => MainConditionRepository.Update(target),
                                typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteMainCondition()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(() => MainConditionRepository.Delete(target),
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