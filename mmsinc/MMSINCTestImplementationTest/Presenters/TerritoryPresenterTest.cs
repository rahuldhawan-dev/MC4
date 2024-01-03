using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINCTestImplementationTest.Presenters
{
    [TestClass]
    public class TerritoryPresenterTest : EventFiringTestClass
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
            var view = _mocks.CreateMock<IDetailView<Territory>>();
            var target = new TerritoryDetailPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }

    [TestClass]
    public class TerritoryResourcePresenterTest : EventFiringTestClass
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
            var target = new TerritoryResourcePresenter(view, null);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestConstructorSetsRepositoryFromArgument()
        {
            var repository = _mocks.CreateMock<IRepository<Territory>>();
            var target = new TerritoryResourcePresenter(null, repository);

            Assert.AreSame(repository, target.Repository);

            _mocks.ReplayAll();
        }
    }

    [TestClass]
    public class TerritoryListPresenterTest : EventFiringTestClass
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
            var view = _mocks.CreateMock<IListView<Territory>>();
            var target = new TerritoriesListPresenter(view);

            Assert.AreSame(view, target.View);

            _mocks.ReplayAll();
        }
    }
}
