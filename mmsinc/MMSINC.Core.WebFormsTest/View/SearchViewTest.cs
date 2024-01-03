using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.View;
using MMSINCTestImplementation.Model;
using Rhino.Mocks;
using StructureMap;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using MMSINC.Utilities.StructureMap;

namespace MMSINC.Core.WebFormsTest.View
{
    /// <summary>
    /// Summary description for SearchViewTest
    /// </summary>
    [TestClass]
    public class SearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private ISearchPresenter<Employee> _presenter;
        private MockSearcView _target;

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks = new MockRepository();
            _presenter = _mocks.DynamicMock<ISearchPresenter<Employee>>();
            _target = new MockSearcView();
            _container.Configure(e => e.For<ISearchPresenter<Employee>>().Use(_presenter));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
            _mocks.VerifyAll();
            _presenter = null;
        }

        #endregion

        #region Private Static Methods

        private static void InvokePageLoad(object obj)
        {
            InvokePageLoad(obj, GetEventArgArray());
        }

        private static void InvokePageLoad(object obj, object[] eventArgsArray)
        {
            InvokeEventByName(obj, "Page_Load", eventArgsArray);
        }

        #endregion

        #region Event Handler Tests

        #region Page Events

        [TestMethod]
        public void TestPageLoadFiresUserControlLoadedEvent()
        {
            using (_target = new TestSearchViewBuilder()
               .WithOnUserControlLoadedHandler(_testableHandler))
            {
                _mocks.ReplayAll();

                InvokePageLoad(_target);

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestSearchClickedFiresSearchClickedEvent()
        {
            using (_target = new TestSearchViewBuilder()
               .WithOnSearchClickedHandler(_testableHandler))
            {
                _mocks.ReplayAll();

                InvokeEventByName(_target, "btnSearch_Click", new object[] {
                    null, EventArgs.Empty
                });

                Assert.IsTrue(_called);
            }
        }

        #endregion

        #region Child Control Events

        [TestMethod]
        public void TestCancelClickedFiresCancelClickedEvent()
        {
            using (_target = new TestSearchViewBuilder()
               .WithOnCancelClickedHandler(_testableHandler))
            {
                _mocks.ReplayAll();

                InvokeEventByName(_target, "btnCancel_Click", new object[] {
                    null, EventArgs.Empty
                });

                Assert.IsTrue(_called);
            }
        }

        #endregion

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestDefaultExpressionEvaluatesToTrue()
        {
            _mocks.ReplayAll();

            // the .Compile() method returns the lambda encapsulated
            // by the expression.  immediately after, null is passed
            // in to a call to that lambda
            Assert.IsTrue(_target.BaseExpression.Compile()(null));
        }

        #endregion
    }

    internal class TestSearchViewBuilder : TestDataBuilder<MockSearcView>
    {
        #region Private Members

        private bool? _postBack = true;

        private EventHandler _onSearchClicked,
                             _onUserControlLoaded,
                             _onCancelClicked;

        #endregion

        #region Private Methods

        private void SearchView_Dispose(MockSearcView sv)
        {
            if (_onSearchClicked != null)
                sv.SearchClicked -= _onSearchClicked;
            if (_onUserControlLoaded != null)
                sv.UserControlLoaded -= _onUserControlLoaded;
            if (_onCancelClicked != null)
                sv.CancelClicked -= _onCancelClicked;
        }

        #endregion

        #region Exposed Methods

        public override MockSearcView Build()
        {
            var sv = new MockSearcView();
            if (_onSearchClicked != null)
                sv.SearchClicked += _onSearchClicked;
            if (_onUserControlLoaded != null)
                sv.UserControlLoaded += _onUserControlLoaded;
            if (_onCancelClicked != null)
                sv.CancelClicked += _onCancelClicked;
            if (_postBack != null)
                sv.SetPostBack(_postBack);
            sv.OnDispose += SearchView_Dispose;
            return sv;
        }

        public TestSearchViewBuilder WithOnUserControlLoadedHandler(EventHandler onUserControlLoaded)
        {
            _onUserControlLoaded = onUserControlLoaded;
            return this;
        }

        public TestSearchViewBuilder WithOnSearchClickedHandler(EventHandler onSearchClicked)
        {
            _onSearchClicked = onSearchClicked;
            return this;
        }

        public TestSearchViewBuilder WithOnCancelClickedHandler(EventHandler onCancelClicked)
        {
            _onCancelClicked = onCancelClicked;
            return this;
        }

        #endregion
    }

    internal class MockSearcView : SearchView<Employee>
    {
        #region Delegates

        public delegate void OnDisposeHandler(MockSearcView dv);

        #endregion

        #region Events

        public OnDisposeHandler OnDispose;

        #endregion

        #region Exposed Methods

        public override void Dispose()
        {
            base.Dispose();
            if (OnDispose != null)
                OnDispose(this);
        }

        public override Expression<Func<Employee, bool>> GenerateExpression()
        {
            return BaseExpression;
        }

        public void SetPostBack(bool? postBack)
        {
            _isMvpPostBack = postBack;
        }

        #endregion
    }
}
