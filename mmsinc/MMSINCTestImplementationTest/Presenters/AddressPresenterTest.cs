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
    public class AddressDetailPresenterTest : EventFiringTestClass
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
            var view = _mocks.CreateMock<IDetailView<Address>>();
            var target = new AddressDetailPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }

    [TestClass]
    public class AddressResourcePresenterTest : EventFiringTestClass
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
            var target = new AddressResourcePresenter(view, null);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestConstructorSetsRepositoryFromArgument()
        {
            var repository = _mocks.CreateMock<IRepository<Address>>();
            var target = new AddressResourcePresenter(null, repository);

            Assert.AreSame(repository, target.Repository);

            _mocks.ReplayAll();
        }
    }

    [TestClass]
    public class AddressListPresenterTest : EventFiringTestClass
    {
        #region Private Methods

        private MockRepository _mocks;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void AddressResourcePresenterTestInitialize()
        {
            _mocks = new MockRepository();
        }

        [TestCleanup]
        public void AddressDetailPresenterTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestConstructorSetsViewFromArgument()
        {
            var view = _mocks.CreateMock<IListView<Address>>();
            var target = new AddresssListPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }
}
