using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for MarkoutStatusTestTest
    /// </summary>
    [TestClass]
    public class MarkoutStatusTest : WorkOrdersTestClass<MarkoutStatus>
    {
        #region Exposed Static Methods

        public static MarkoutStatus GetValidMarkoutStatus()
        {
            return MarkoutStatusRepository.Pending;
        }

        public static void DeleteMarkoutStatus(MarkoutStatus entity)
        {
            MarkoutStatusRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override MarkoutStatus GetValidObject()
        {
            return GetValidMarkoutStatus();
        }

        protected override MarkoutStatus GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(MarkoutStatus entity)
        {
            DeleteMarkoutStatus(entity);
        }

        #endregion

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }
    }
}