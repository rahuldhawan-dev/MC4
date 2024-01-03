using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINCTestImplementationTest.Presenters
{
    [TestClass]
    public class CityDetailPresenterTest : EventFiringTestClass
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
            var view = _mocks.CreateMock<IDetailView<City>>();
            var target = new CityDetailPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }

    [TestClass]
    public class CityResourcePresenterTest : EventFiringTestClass
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
            var target = new CityResourcePresenter(view, null);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestConstructorSetsRepositoryFromArgument()
        {
            var repository = _mocks.CreateMock<IRepository<City>>();
            var target = new CityResourcePresenter(null, repository);

            Assert.AreSame(repository, target.Repository);

            _mocks.ReplayAll();
        }
    }

    [TestClass]
    public class CityListPresenterTest : EventFiringTestClass
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
            var view = _mocks.CreateMock<IListView<City>>();
            var target = new CitysListPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }
}
