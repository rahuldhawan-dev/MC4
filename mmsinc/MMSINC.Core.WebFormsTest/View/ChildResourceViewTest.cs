using System.Reflection;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.View;
using MMSINCTestImplementation;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.View
{
    [TestClass]
    public class ChildResourceViewTest
    {
        /// <summary>
        /// Test that we can create of these objects.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var x = new MockChildResourceView();
            Assert.IsNotNull(x);
            Assert.IsInstanceOfType(x, typeof(ChildResourceView<Employee>));
        }
    }

    internal class TestChildResourceViewBuilder : TestDataBuilder<MockChildResourceView>
    {
        #region Private Members

        private IListView<Employee> _listView;
        private IDetailView<Employee> _detailView;
        private ISearchView<Employee> _searchView;
        private bool? _postBack;

        #endregion

        #region Private Methods

        private void SetPostBack(MockChildResourceView rv)
        {
            var isPostBack = rv.GetType().GetField("_isMvpPostBack",
                BindingFlags.Instance |
                BindingFlags.NonPublic);
            isPostBack.SetValue(rv, _postBack.Value);
        }

        #endregion

        #region Exposed Methods

        override public MockChildResourceView Build()
        {
            var rv = new MockChildResourceView();
            if (_postBack != null)
                SetPostBack(rv);
            if (_listView != null)
                rv.SetListView(_listView);
            if (_detailView != null)
                rv.SetDetailView(_detailView);
            if (_searchView != null)
                rv.SetSearchView(_searchView);
            return rv;
        }

        public TestChildResourceViewBuilder WithListView(IListView<Employee> listView)
        {
            _listView = listView;
            return this;
        }

        public TestChildResourceViewBuilder WithDetailView(IDetailView<Employee> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestChildResourceViewBuilder WithSearchView(ISearchView<Employee> searchView)
        {
            _searchView = searchView;
            return this;
        }

        public TestChildResourceViewBuilder WithPostBack(bool postBack)
        {
            _postBack = postBack;
            return this;
        }

        #endregion
    }

    internal class MockChildResourceView : ChildResourceView<Employee>
    {
        #region Private Members

        private IListView<Employee> _listView;
        private IDetailView<Employee> _detailView;
        private ISearchView<Employee> _searchView;

        #endregion

        #region Properties

        public override IListView<Employee> ListView
        {
            get { return _listView; }
        }

        public override IDetailView<Employee> DetailView
        {
            get { return _detailView; }
        }

        public override ISearchView<Employee> SearchView
        {
            get { return _searchView; }
        }

        #endregion

        #region Constructors

        public MockChildResourceView() { }

        public MockChildResourceView(IListView<Employee> listView) : this(listView, null, null) { }

        public MockChildResourceView(IDetailView<Employee> detailView) : this(null, detailView, null) { }

        public MockChildResourceView(ISearchView<Employee> searchView) : this(null, null, searchView) { }

        public MockChildResourceView(IListView<Employee> listView, IDetailView<Employee> detailView) : this(listView,
            detailView, null) { }

        public MockChildResourceView(IListView<Employee> listView, IDetailView<Employee> detailView,
            ISearchView<Employee> searchView)
        {
            SetListView(listView);
            SetDetailView(detailView);
            SetSearchView(searchView);
        }

        #endregion

        #region Exposed Methods

        public void SetListView(IListView<Employee> listView)
        {
            _listView = listView;
        }

        public void SetDetailView(IDetailView<Employee> detailView)
        {
            _detailView = detailView;
        }

        public void SetSearchView(ISearchView<Employee> searchView)
        {
            _searchView = searchView;
        }

        #endregion
    }
}
