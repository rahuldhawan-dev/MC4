using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINCTestImplementationTest.Presenters
{
    [TestClass]
    public class StateDetailPresenterTest : EventFiringTestClass
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
            var view = _mocks.CreateMock<IDetailView<State>>();
            var target = new StateDetailPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }

    [TestClass]
    public class StateResourcePresenterTest : EventFiringTestClass
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
            var view = _mocks.CreateMock<IResourceView>();
            var target = new StateResourcePresenter(view, null);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestConstructorSetsRepositoryFromArgument()
        {
            var repository = _mocks.CreateMock<IRepository<State>>();
            var target = new StateResourcePresenter(null, repository);

            Assert.AreSame(repository, target.Repository);

            _mocks.ReplayAll();
        }
    }

    [TestClass]
    public class StateListPresenterTest : EventFiringTestClass
    {
        #region Private Methods

        private MockRepository _mocks;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void StateResourcePresenterTestInitialize()
        {
            _mocks = new MockRepository();
        }

        [TestCleanup]
        public void StateDetailPresenterTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestConstructorSetsViewFromArgument()
        {
            var view = _mocks.CreateMock<IListView<State>>();
            var target = new StatesListPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }
}
