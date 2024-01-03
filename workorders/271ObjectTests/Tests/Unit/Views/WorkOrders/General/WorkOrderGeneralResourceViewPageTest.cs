using LINQTo271.Views.WorkOrders.General;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.General
{
    /// <summary>
    /// Summary description for WorkOrderGeneralResourceViewPageTest.
    /// </summary>
    [TestClass]
    public class WorkOrderGeneralResourceViewPageTest
    {
        #region Private Members

        private WorkOrderGeneralResourceViewPage _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderGeneralResourceViewPageTestInitialize()
        {
            _target = new TestWorkOrderGeneralResourceViewPageBuilder();
        }

        #endregion

        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            MyAssert.DoesNotThrow(() =>
                                  _target =
                                  new WorkOrderGeneralResourceViewPage());
        }
    }

    internal class TestWorkOrderGeneralResourceViewPageBuilder : TestDataBuilder<TestWorkOrderGeneralResourceViewPage>
    {
        #region Exposed Methods

        public override TestWorkOrderGeneralResourceViewPage Build()
        {
            var obj = new TestWorkOrderGeneralResourceViewPage();
            return obj;
        }

        #endregion
    }

    internal class TestWorkOrderGeneralResourceViewPage : WorkOrderGeneralResourceViewPage
    {
    }
}
