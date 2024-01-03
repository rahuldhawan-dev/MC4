using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.Materials;
using WorkOrders.Views.Materials;

namespace _271ObjectTests.Tests.Unit.Presenters.Materials
{
    [TestClass]
    public class MaterialResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceView<Material> _view;
        private IRepository<Material> _repository;
        private IMaterialListView _listView;
        private IMaterialSearchView _searchView;
        private TestMaterialResourcePresenter _target;
        
        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository)
                .DynamicMock(out _searchView)
                .DynamicMock(out _listView);

            _target =
                new TestMaterialResourcePresenterBuilder(_view, _repository)
                    .WithListView(_listView)
                    .WithSearchView(_searchView);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourcePresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersAdminResourcePresenter<Material>),
                "ResourcePresenters in this project should inherit from WorkOrdersResourcePresenter, lest bad tings happen.");
        }

        [TestMethod]
        public void TestViewLoadCompleteSetsListData()
        {
            IEnumerable<Material> result = new Material[10];
            var expr = PredicateBuilder.True<Material>();
            using (_mocks.Record())
            {
                SetupResult.For(_listView.Visible).Return(true);
                SetupResult.For(_searchView.GenerateExpression()).Return(expr);
                SetupResult.For(_repository.GetFilteredSortedData(expr,null)).Return(result);
                _listView.SetListData(result);
                _listView.DataBind();
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_LoadComplete");
            }
        }
    }

    internal class TestMaterialResourcePresenterBuilder : TestDataBuilder<TestMaterialResourcePresenter>
    {
        #region Private Members

        private IResourceView<Material> _view;
        private IListView<Material> _listView;
        private ISearchView<Material> _searchView;
        private IRepository<Material> _repository;

        #endregion

        #region Constructors

        public TestMaterialResourcePresenterBuilder(IResourceView<Material> view, IRepository<Material> repository)
        {
            _view = view;
            _repository = repository;
        }
        
        #endregion
        
        #region Exposed Methods

        public override TestMaterialResourcePresenter Build()
        {
            var obj = new TestMaterialResourcePresenter(_view, _repository);
            if (_searchView!=null)
                obj.SearchView = _searchView;
            if (_listView != null)
                obj.ListView = _listView;
            return obj;
        }

        public TestMaterialResourcePresenterBuilder WithSearchView(ISearchView<Material> view)
        {
            _searchView = view;
            return this;
        }

        public TestMaterialResourcePresenterBuilder WithListView(IListView<Material> view)
        {
            _listView = view;
            return this;
        }


        #endregion
    }

    internal class TestMaterialResourcePresenter : MaterialResourcePresenter
    {
        public TestMaterialResourcePresenter(IResourceView view, IRepository<Material> repository)
            : base(view, repository)
        {
            
        }

        public void TestSetListViewData(Expression<Func<Material, bool>> expression)
        {
            base.SetListViewData(expression);
        }
    }
}
