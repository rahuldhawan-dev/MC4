using System.Web.Mvc;
using MMSINC.ClassExtensions;
using StructureMap;
#if DEBUG
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Sorting;
using MMSINC.View;
using MMSINCTestImplementation.Model;
using Rhino.Mocks;
using Subtext.TestLibrary;
#endif
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;

namespace MMSINC.Core.WebFormsTest.View
{
    /// <summary>
    /// Summary description for ListViewTest
    /// </summary>
    [TestClass]
    public class ListViewTest : EventFiringTestClass
    {
#if DEBUG

        #region Private Members

        private MockListView _target;
        private IListPresenter<Employee> _presenter;
        private IListControl _listControl;
        private HttpSimulator _simulator;

        #endregion

        #region Private Static Methods

        private static void InvokePageLoad(MockListView lv)
        {
            InvokeEventByName(lv, "Page_Load");
        }

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _presenter = _mocks.DynamicMock<IListPresenter<Employee>>();
            _listControl = _mocks.DynamicMock<IListControl>();
            _target = new TestListViewBuilder().WithListControl(_listControl);
            _container.Inject(_presenter);
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
            _simulator.Dispose();
        }

        #endregion

        #region Event Handler Tests

        #region Page Events

        [TestMethod]
        public void TestPageInitCallsPresenterOnViewInit()
        {
            using (_mocks.Record())
            {
                _presenter.OnViewInit();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Init");
            }
        }

        [TestMethod]
        public void TestPageLoadCallsPresenterOnViewInitializedWhenIsNotPostBack()
        {
            _target = new TestListViewBuilder().WithPostBack(false);

            using (_mocks.Record())
            {
                _presenter.OnViewInitialized();
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadDoesNotCallPresenterOnViewInitializedWhenIsPostBack()
        {
            _target = new TestListViewBuilder().WithPostBack(true);

            using (_mocks.Record())
            {
                DoNotExpect.Call(_presenter.OnViewInitialized);
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadFiresUserControlLoadedEvent()
        {
            using (_target = new TestListViewBuilder()
               .WithUserControlLoadedEventHandler(_testableHandler))
            {
                _mocks.ReplayAll();

                InvokePageLoad(_target);

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestPageLoadCallsPresenterOnViewLoaded()
        {
            using (_mocks.Record())
            {
                _presenter.OnViewLoaded();
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadCompleteFiresLoadCompleteEvent()
        {
            using (_target = new TestListViewBuilder()
               .WithLoadCompleteEventHandler(_testableHandler))
            {
                _mocks.ReplayAll();

                InvokeEventByName(_target, "Page_LoadComplete");

                Assert.IsTrue(_called);
            }
        }

        #endregion

        #region Child Control Events

        [TestMethod]
        public void TestCreateClickedFiresOnCreateClickedEvent()
        {
            using (_target = new TestListViewBuilder()
               .WithCreateClickedEventHandler(_testableHandler))
            {
                _mocks.ReplayAll();

                InvokeEventByName(_target, "btnCreate_Click");

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestDataSourceCreatingFiresOdsObjectCreatingEvent()
        {
            using (_target = new TestListViewBuilder()
               .WithDataSourceCreatingEventHandler((sender, e) => _called = true))
            {
                _mocks.ReplayAll();

                InvokeEventByName(_target, "ods_ObjectCreating");

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestListControlSortingFiresSortingEvent()
        {
            using (_target = new TestListViewBuilder()
               .WithSortingEventHandler((sender, e) => _called = true))
            {
                _mocks.ReplayAll();
                InvokeEventByName(_target, "ListControl_Sorting",
                    new[] {null, new GridViewSortEventArgs("Test", SortDirection.Ascending)});
                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestListControlSortingSetsExpressionAndDirection()
        {
            var sortExpression = "foo";
            var sortDirection = SortDirection.Ascending;
            var args = new GridViewSortEventArgs(sortExpression, sortDirection);
            _mocks.ReplayAll();
            InvokeEventByName(_target, "ListControl_Sorting", new[] {null, args});
            Assert.AreEqual(_target.SortExpression, sortExpression);
            Assert.AreEqual(_target.PreviousSortDirection, sortDirection.ToString());
            sortDirection = SortDirection.Descending;
            InvokeEventByName(_target, "ListControl_Sorting", new[] {null, args});
            Assert.AreEqual(_target.PreviousSortDirection, sortDirection.ToString());
        }

        [TestMethod]
        public void TestListControlSelectedIndexChangedFiresSelectedIndexChanged()
        {
            using (_target = new TestListViewBuilder()
               .WithSelectedIndexChangedEventHandler(_testableHandler))
            {
                _mocks.ReplayAll();

                InvokeEventByName(_target, "ListControl_SelectedIndexChanged");

                Assert.IsTrue(_called);
            }
        }

        #endregion

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestSelectedDataKeyReturnsNullWhenListControlDataKeyIsNull()
        {
            SetupResult.For(_listControl.SelectedDataKey).Return(null);

            _mocks.ReplayAll();

            Assert.IsNull(_target.SelectedDataKey);
        }

        [TestMethod]
        public void TestSelectedDataKeyReturnsListControlSelectedDataKey()
        {
            var expected = "foo";
            var key = new MockDataKey(expected);
            SetupResult.For(_listControl.SelectedDataKey).Return(key);

            _mocks.ReplayAll();

            Assert.AreEqual(expected, _target.SelectedDataKey);
        }

        [TestMethod]
        public void TestSelectedDataKeyReturnsNullWhenListControlIsNull()
        {
            _target = new TestListViewBuilder().WithListControl(null);

            _mocks.ReplayAll();

            Assert.IsNull(_target.SelectedDataKey);
        }

        [TestMethod]
        public void TestSelectedIndexPropertyPassesThroughToListControlSelectedIndex()
        {
            var expectedSet = 1;
            var expectedGet = 2;
            int actual;
            SetupResult.For(_listControl.SelectedIndex).Return(expectedGet);

            using (_mocks.Record())
            {
                _listControl.SelectedIndex = expectedSet;
            }

            using (_mocks.Playback())
            {
                actual = _target.SelectedIndex;
                _target.SelectedIndex = expectedSet;
            }

            Assert.AreEqual(expectedGet, actual);
        }

        [TestMethod]
        public void TestSortExpressionSet()
        {
            var expected = "FieldName";
            _mocks.ReplayAll();
            _target.SortExpression = expected;
            Assert.AreSame(expected, _target.SortExpression);
        }

        [TestMethod]
        public void TestSqlSortDirection()
        {
            const string expected = "FieldName";
            const string direction = "DESC";
            _mocks.ReplayAll();

            _target.SortExpression = expected;
            _target.PreviousSortDirection = SortDirection.Descending.ToString();
            Assert.AreEqual(_target.SqlSortExpression,
                expected + " " + direction);
        }

        [TestMethod]
        public void TestSettingVisibilityFiresVisibilityChangedEventWhenWired()
        {
            _mocks.ReplayAll();
            using (
                _target =
                    new TestListViewBuilder()
                       .WithVisibilityChangedEventHandler((sender, args) => _called = true))
            {
                _target.Visible = false;
                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestSettingVisibilityDoesNotThrowExceptionWhenVisibilityChangedEventHandlerIsNotWired()
        {
            _mocks.ReplayAll();
            _target = new TestListViewBuilder()
               .WithVisibilityChangedEventHandler(null);

            MyAssert.DoesNotThrow(() => _target.Visible = false);
        }

        [TestMethod]
        public void TestSettingVisibilityToSameValueDoesNotFireVisibilityChangedEvent()
        {
            _mocks.ReplayAll();
            using (
                _target =
                    new TestListViewBuilder()
                       .WithVisibilityChangedEventHandler((sender, args) => _called = true))
            {
                _target.Visible = true;
                Assert.IsTrue(_called);
            }
        }

        #endregion

        #region Exposed Method Tests

        [TestMethod]
        public void TestDataBindCallsListControlDataBind()
        {
            using (_mocks.Record())
            {
                _listControl.DataBind();
            }

            using (_mocks.Playback())
            {
                _target.DataBind();
            }
        }

        [TestMethod]
        public void TestSetListDataSetsListControlDataSourceWithNoSortExpression()
        {
            using (_simulator.SimulateRequest())
            {
                var data = _mocks.DynamicMock<IEnumerable<Employee>>();
                using (_mocks.Record())
                {
                    _target.ListControl.DataSource = data;
                    _target.ListControl.DataSourceID = null;
                }

                using (_mocks.Playback())
                {
                    _target.SetListData(data);
                }
            }
        }

        [TestMethod]
        public void TestSetListDataSetsListControlDataSourceWithSortExpressionDescending()
        {
            var sortable = _mocks.DynamicMock<ISorter>();
            var data = _mocks.DynamicMock<IEnumerable<Employee>>();
            IEnumerableExtensions.SetSortingFactory(set => sortable);
            _target.SortExpression = "EmployeeID";
            _target.PreviousSortDirection = SortDirection.Descending.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(sortable.Sort<Employee>(_target.SqlSortExpression)).Return
                    (data);
            }

            using (_mocks.Playback())
            {
                _target.SetListData(data);
            }

            IEnumerableExtensions.ResetSortingFactory();
        }

        [TestMethod]
        public void TestGetAndDisplayCountDoesNothingToPassedDataByDefault()
        {
            var data = _mocks.CreateMock<IEnumerable<Employee>>();
            var methodInfo = _target.GetType().GetMethod("GetAndDisplayCount",
                BindingFlags.NonPublic | BindingFlags.Instance);
            using (_mocks.Record()) { }

            using (_mocks.Playback())
            {
                methodInfo.Invoke(_target, new object[] {
                    data
                });
            }
        }

        #endregion

#else
        [TestMethod]
        public void TestThatYouAreRunningYourTestsInDebugMode()
        {
            Assert.Fail("DOH! Switch to DEBUG mode and run all your tests again.");
        }
#endif
    }

#if DEBUG

    internal class MockListView : ListView<Employee>
    {
        #region Private Members

        private IListControl _listControl;

        #endregion

        #region Properties

        public override IListControl ListControl
        {
            get { return _listControl; }
        }

        #endregion

        #region Delegates

        public delegate void OnDisposeHandler(MockListView lv);

        #endregion

        #region Events

        public OnDisposeHandler OnDispose;

        #endregion

        #region Exposed Methods

        public void SetListControl(IListControl listControl)
        {
            _listControl = listControl;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (OnDispose != null)
                OnDispose(this);
        }

        public override void SetViewControlsVisible(bool visible)
        {
            throw new NotImplementedException();
        }

        public void OnUserControlLoaded()
        {
            base.OnUserControlLoaded(EventArgs.Empty);
        }

        #endregion
    }

    internal class MockDataKey : DataKey
    {
        #region Constructors

        internal MockDataKey(object value) : base(GenerateDictionary(value)) { }

        #endregion

        #region Private Static Methods

        private static IOrderedDictionary GenerateDictionary(object value)
        {
            // i don't even understand this syntax
            var dictionary = new OrderedDictionary {
                {
                    value, value
                }
            };
            return dictionary;
        }

        #endregion
    }

    internal class TestListViewBuilder : TestDataBuilder<MockListView>
    {
        #region Private Members

        private bool? _postBack = true;

        private EventHandler _onUserControlLoaded,
                             _onSelectedIndexChanged,
                             _onCreateClicked,
                             _onLoadComplete;

        private EventHandler<ObjectDataSourceEventArgs> _onDataSourceCreating;
        private EventHandler<VisibilityChangeEventArgs> _onVisibilityChanged;

        private IListControl _listControl;
        private EventHandler<GridViewSortEventArgs> _onSorting;

        #endregion

        #region Private Methods

        private void SetListControl(MockListView lv)
        {
            lv.SetListControl(_listControl);
        }

        private void SetPostBack(MockListView lv)
        {
            var isPostBack = lv.GetType().GetField("_isMvpPostBack",
                BindingFlags.Instance |
                BindingFlags.NonPublic);
            isPostBack.SetValue(lv, _postBack.Value);
        }

        private void ListView_Dispose(MockListView lv)
        {
            if (_onUserControlLoaded != null)
                lv.UserControlLoaded -= _onUserControlLoaded;
            if (_onSelectedIndexChanged != null)
                lv.SelectedIndexChanged -= _onSelectedIndexChanged;
            if (_onCreateClicked != null)
                lv.CreateClicked -= _onCreateClicked;
            if (_onDataSourceCreating != null)
                lv.DataSourceCreating -= _onDataSourceCreating;
            if (_onLoadComplete != null)
                lv.LoadComplete -= _onLoadComplete;
            if (_onVisibilityChanged != null)
                lv.VisibilityChanged -= _onVisibilityChanged;
        }

        #endregion

        #region Exposed Methods

        public override MockListView Build()
        {
            var lv = new MockListView();
            if (_postBack != null)
                SetPostBack(lv);
            if (_onUserControlLoaded != null)
                lv.UserControlLoaded += _onUserControlLoaded;
            if (_onSelectedIndexChanged != null)
                lv.SelectedIndexChanged += _onSelectedIndexChanged;
            if (_onCreateClicked != null)
                lv.CreateClicked += _onCreateClicked;
            if (_onDataSourceCreating != null)
                lv.DataSourceCreating += _onDataSourceCreating;
            if (_onSorting != null)
                lv.Sorting += _onSorting;
            if (_onLoadComplete != null)
                lv.LoadComplete += _onLoadComplete;
            if (_onVisibilityChanged != null)
                lv.VisibilityChanged += _onVisibilityChanged;
            if (_listControl != null)
                SetListControl(lv);
            lv.OnDispose += ListView_Dispose;
            return lv;
        }

        public TestListViewBuilder WithCreateClickedEventHandler(EventHandler onCreateClicked)
        {
            _onCreateClicked = onCreateClicked;
            return this;
        }

        public TestListViewBuilder WithDataSourceCreatingEventHandler(
            EventHandler<ObjectDataSourceEventArgs> onDataSourceCreating)
        {
            _onDataSourceCreating = onDataSourceCreating;
            return this;
        }

        public TestListViewBuilder WithSortingEventHandler(EventHandler<GridViewSortEventArgs> onSorting)
        {
            _onSorting = onSorting;
            return this;
        }

        public TestListViewBuilder WithSelectedIndexChangedEventHandler(EventHandler onSelectedIndexChanged)
        {
            _onSelectedIndexChanged = onSelectedIndexChanged;
            return this;
        }

        public TestListViewBuilder WithUserControlLoadedEventHandler(EventHandler onUserControlLoaded)
        {
            _onUserControlLoaded = onUserControlLoaded;
            return this;
        }

        public TestListViewBuilder WithPostBack(bool postBack)
        {
            _postBack = postBack;
            return this;
        }

        public TestListViewBuilder WithLoadCompleteEventHandler(EventHandler onLoadComplete)
        {
            _onLoadComplete = onLoadComplete;
            return this;
        }

        public TestListViewBuilder WithListControl(IListControl listControl)
        {
            _listControl = listControl;
            return this;
        }

        public TestListViewBuilder WithVisibilityChangedEventHandler(EventHandler<VisibilityChangeEventArgs> handler)
        {
            _onVisibilityChanged = handler;
            return this;
        }

        #endregion
    }

#endif
}
