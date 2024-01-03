using LINQTo271.Views.Employees;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.Employees
{
    /// <summary>
    /// Summary description for EmployeesServiceViewTest.
    /// </summary>
    [TestClass]
    public class EmployeesServiceViewTest
    {
        #region Private Members

        private TestEmployeesServiceView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void EmployeesServiceViewTestInitialize()
        {
            _target = new TestEmployeesServiceViewBuilder();
        }

        #endregion
    }

    internal class TestEmployeesServiceViewBuilder : TestDataBuilder<TestEmployeesServiceView>
    {
        #region Exposed Methods

        public override TestEmployeesServiceView Build()
        {
            var obj = new TestEmployeesServiceView();
            return obj;
        }

        #endregion
    }

    internal class TestEmployeesServiceView : EmployeesServiceView
    {

    }
}
