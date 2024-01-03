using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINCTestImplementationTest.Presenters
{
    /// <summary>
    /// Summary description for EmployeeResourceRPCPresenterTest
    /// </summary>
    [TestClass]
    public class EmployeeResourceRPCPresenterTest : EventFiringTestClass
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
            var view = _mocks.DynamicMock<IResourceRPCView<Employee>>();
            var target = new EmployeeResourceRPCPresenter(view, null);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestConstructorSetsRepositoryFromArgument()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var target = new EmployeeResourceRPCPresenter(null, repository);

            Assert.AreSame(repository, target.Repository);

            _mocks.ReplayAll();
        }
    }
}
