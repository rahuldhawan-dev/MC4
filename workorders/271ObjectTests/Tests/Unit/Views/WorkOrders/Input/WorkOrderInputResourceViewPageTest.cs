using LINQTo271.Views.WorkOrders.Input;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Input
{
    /// <summary>
    /// Summary description for WorkOrderInputResourceViewPageTest
    /// </summary>
    [TestClass]
    public class WorkOrderInputResourceViewPageTest
    {
        #region Private Members

        private WorkOrderInputResourceViewPage _target;

	    #endregion

        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            MyAssert.DoesNotThrow(
                () => _target = new WorkOrderInputResourceViewPage());
            Assert.IsInstanceOfType(_target,
                typeof(WorkOrderInputResourceViewPage));
        }
    }
}
