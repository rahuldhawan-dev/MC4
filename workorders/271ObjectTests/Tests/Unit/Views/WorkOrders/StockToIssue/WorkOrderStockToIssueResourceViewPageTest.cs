using LINQTo271.Views.WorkOrders.StockToIssue;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.StockToIssue
{
    /// <summary>
    /// Summary description for WorkOrderStockToIssueResourceViewPageTest.
    /// </summary>
    [TestClass]
    public class WorkOrderStockToIssueResourceViewPageTest
    {
        #region Private Members

        private TestWorkOrderStockToIssueResourceViewPage _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderStockToIssueResourceViewPageTestInitialize()
        {
            _target = new TestWorkOrderStockToIssueResourceViewPageBuilder();
        }

        #endregion

        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            WorkOrderStockToIssueResourceViewPage target;
            MyAssert.DoesNotThrow(
                () => target = new WorkOrderStockToIssueResourceViewPage());
        }
    }

    internal class TestWorkOrderStockToIssueResourceViewPageBuilder : TestDataBuilder<TestWorkOrderStockToIssueResourceViewPage>
    {
        #region Exposed Methods

        public override TestWorkOrderStockToIssueResourceViewPage Build()
        {
            var obj = new TestWorkOrderStockToIssueResourceViewPage();
            return obj;
        }

        #endregion
    }

    internal class TestWorkOrderStockToIssueResourceViewPage : WorkOrderStockToIssueResourceViewPage
    {
    }
}
