using LINQTo271.Views.WorkOrders.SupervisorApproval;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SupervisorApproval
{
    /// <summary>
    /// Summary description for WorkOrderSupervisorApprovalResourceViewPageTest.
    /// </summary>
    [TestClass]
    public class WorkOrderSupervisorApprovalResourceViewPageTest
    {
        #region Private Members

        private TestWorkOrderSupervisorApprovalResourceViewPage _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderSupervisorApprovalResourceViewPageTestInitialize()
        {
            _target = new TestWorkOrderSupervisorApprovalResourceViewPageBuilder();
        }

        #endregion

        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            WorkOrderSupervisorApprovalResourceViewPage target;
            MyAssert.DoesNotThrow(
                () => target = new WorkOrderSupervisorApprovalResourceViewPage());
        }
    }

    internal class TestWorkOrderSupervisorApprovalResourceViewPageBuilder : TestDataBuilder<TestWorkOrderSupervisorApprovalResourceViewPage>
    {
        #region Exposed Methods

        public override TestWorkOrderSupervisorApprovalResourceViewPage Build()
        {
            var obj = new TestWorkOrderSupervisorApprovalResourceViewPage();
            return obj;
        }

        #endregion
    }

    internal class TestWorkOrderSupervisorApprovalResourceViewPage : WorkOrderSupervisorApprovalResourceViewPage
    {
    }
}
