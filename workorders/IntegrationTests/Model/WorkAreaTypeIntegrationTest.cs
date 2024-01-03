using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for WorkAreaTypeTestTest
    /// </summary>
    [TestClass]
    public class WorkAreaTypeIntegrationTest : WorkOrdersTestClass<WorkAreaType>
    {
        #region Exposed Static Methods

        public static WorkAreaType GetValidWorkAreaType()
        {
            return new WorkAreaType {
                                        Description = "TestDescription"
                                    };
        }

        public static void DeleteWorkAreaType(WorkAreaType entity)
        {
            WorkAreaTypeRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override WorkAreaType GetValidObject()
        {
            return GetValidWorkAreaType();
        }

        protected override WorkAreaType GetValidObjectFromDatabase()
        {
            var type = GetValidObject();
            WorkAreaTypeRepository.Insert(type);
            return type;
        }

        protected override void DeleteObject(WorkAreaType entity)
        {
            DeleteWorkAreaType(entity);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewWorkAreaType()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.DoesNotThrow(
                    () => WorkAreaTypeRepository.Insert(target));

                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(WorkAreaType));

                DeleteObject(target);
            }
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestCannotSaveWithoutDescription()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.Description = null;

                MyAssert.Throws(() => WorkAreaTypeRepository.Insert(target),
                                typeof(DomainLogicException),
                                "Attempting to save a WorkAreaType without a value for Description should throw an exception");
            }
        }
    }
}