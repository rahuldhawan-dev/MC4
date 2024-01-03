using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINCTestImplementationTest.Presenters
{
    /// <summary>
    /// Summary description for EmployeeSearchPresenterTest
    /// </summary>
    [TestClass]
    public class EmployeeSearchPresenterTest : EventFiringTestClass
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
            var view = _mocks.DynamicMock<ISearchView<Employee>>();
            var target = new EmployeeSearchPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }
}
