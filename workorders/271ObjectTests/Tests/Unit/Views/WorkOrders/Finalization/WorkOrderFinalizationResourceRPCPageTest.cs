using LINQTo271.Views.WorkOrders.Finalization;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Finalization
{
    /// <summary>
    /// Summary description for WorkOrderFinalizationResourceRPCPageTest.
    /// </summary>
    [TestClass]
    public class WorkOrderFinalizationResourceRPCPageTest
    {
        #region Private Members

        private WorkOrderFinalizationResourceRPCPage _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderFinalizationResourceRPCPageTestInitialize()
        {
            _target = new TestWorkOrderFinalizationResourceRPCPageBuilder();
        }

        #endregion

        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            MyAssert.DoesNotThrow(
                () => _target = new WorkOrderFinalizationResourceRPCPage());
        }
    }

    internal class TestWorkOrderFinalizationResourceRPCPageBuilder : TestDataBuilder<TestWorkOrderFinalizationResourceRPCPage>
    {
        #region Exposed Methods

        public override TestWorkOrderFinalizationResourceRPCPage Build()
        {
            var obj = new TestWorkOrderFinalizationResourceRPCPage();
            return obj;
        }

        #endregion
    }

    internal class TestWorkOrderFinalizationResourceRPCPage : WorkOrderFinalizationResourceRPCPage
    {
    }
}
