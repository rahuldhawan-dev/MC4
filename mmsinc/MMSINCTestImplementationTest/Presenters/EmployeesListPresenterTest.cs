using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINCTestImplementationTest.Presenters
{
    /// <summary>
    /// Summary description for EmployeesListPresenterTest
    /// </summary>
    [TestClass]
    public class EmployeesListPresenterTest : EventFiringTestClass
    {
        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestConstructorSetsViewFromArgument()
        {
            var view = _mocks.DynamicMock<IListView<Employee>>();
            var target = new EmployeesListPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }
}
