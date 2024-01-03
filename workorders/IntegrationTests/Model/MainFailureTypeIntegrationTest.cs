using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MainFailureTypeTestTest
    /// </summary>
    [TestClass]
    public class MainFailureTypeIntegrationTest : WorkOrdersTestClass<MainFailureType>
    {
        #region Exposed Static Methods

        public static MainFailureType GetValidMainFailureType()
        {
            return MainFailureTypeRepository.Circular;
        }

        #endregion

        #region Private Methods

        protected override MainFailureType GetValidObject()
        {
            return GetValidMainFailureType();
        }

        protected override MainFailureType GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(MainFailureType entity)
        {
            MainFailureTypeRepository.Delete(entity);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewMainFailureType()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new MainFailureType { Description = "Test Description" };

                MyAssert.DoesNotThrow(() => MainFailureTypeRepository.Insert(target));
                
                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(MainFailureType));

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestCannotSaveWithoutDescription()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new MainFailureType {
                    Description = null
                };

                MyAssert.Throws(() => MainFailureTypeRepository.Insert(target),
                                typeof(DomainLogicException),
                                "Attempting to save a MainFailureType without a Description should throw an exception");
            }
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }
    }
}