using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for LeakReportingSourceTestTest
    /// </summary>
    [TestClass]
    public class LeakReportingSourceTest : WorkOrdersTestClass<LeakReportingSource>
    {
        #region Exposed Static Methods

        public static LeakReportingSource GetValidLeakReportingSource()
        {
            return LeakReportingSourceRepository.FieldServiceRep;
        }

        public static void DeleteLeakReportingSource(LeakReportingSource entity)
        {
            LeakReportingSourceRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override LeakReportingSource GetValidObject()
        {
            return GetValidLeakReportingSource();
        }

        protected override LeakReportingSource GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(LeakReportingSource entity)
        {
            DeleteLeakReportingSource(entity);
        }

        #endregion

        [TestMethod]
        public void TestCannotCreateNewLeakReportingSource()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new LeakReportingSource {
                                                         Description = "TestDescription"
                                                     };

                MyAssert.Throws(
                    () => LeakReportingSourceRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotAlterLeakReportingSource()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();
                target.Description = "TestDescription";

                MyAssert.Throws(
                    () => LeakReportingSourceRepository.Update(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteLeakReportingSource()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();

                MyAssert.Throws(
                    () => LeakReportingSourceRepository.Delete(target),
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