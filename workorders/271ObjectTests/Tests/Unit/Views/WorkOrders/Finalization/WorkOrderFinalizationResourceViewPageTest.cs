using LINQTo271.Views.WorkOrders.Finalization;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Finalization
{
    /// <summary>
    /// Summary description for WorkOrderFinalizationResourceViewPageTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderFinalizationResourceViewPageTest
    {
        #region Private Members

        private TestWorkOrderFinalizationResourceViewPage _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderFinalizationResourceViewPageTestInitialize()
        {
            _target = new TestWorkOrderFinalizationResourceViewPageBuilder();
        }

        #endregion

        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            WorkOrderFinalizationResourceViewPage target;
            MyAssert.DoesNotThrow(
                () => target = new WorkOrderFinalizationResourceViewPage());
        }
    }

    internal class TestWorkOrderFinalizationResourceViewPageBuilder : TestDataBuilder<TestWorkOrderFinalizationResourceViewPage>
    {
        #region Exposed Methods

        public override TestWorkOrderFinalizationResourceViewPage Build()
        {
            var obj = new TestWorkOrderFinalizationResourceViewPage();
            return obj;
        }

        #endregion
    }

    internal class TestWorkOrderFinalizationResourceViewPage : WorkOrderFinalizationResourceViewPage
    {
    }
}