using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Data.Linq;
using Moq;
using StructureMap;
#if DEBUG
using System;
using System.Linq.Expressions;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
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
    /// Summary description for ResourceRPCViewTestTest
    /// </summary>
    [TestClass]
    public class ResourceRPCViewTest : EventFiringTestClass
    {
#if DEBUG

        #region Constants

        private const string TEST_URL_FORMAT = "http://foo?{0}={1}&{2}={3}";
        private const string TEST_COMMAND = "Foo";
        private const string TEST_ARGUMENT = "Bar";

        #endregion

        #region Private Members

        private IUser _currentUser;
        private IListView<Employee> _listView;
        private IDetailView<Employee> _detailView;
        private ISearchView<Employee> _searchView;
        private IResourceRPCPresenter<Employee> _presenter;
        private HttpSimulator _simulator;
        private TestResourceRPCView _target;

        #endregion

        #region Private Static Methods

        private static string BuildTestUrl()
        {
            return String.Format(TEST_URL_FORMAT, RPCQueryStringValues.COMMAND,
                TEST_COMMAND, RPCQueryStringValues.ARGUMENT, TEST_ARGUMENT);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _listView)
               .DynamicMock(out _detailView)
               .DynamicMock(out _searchView)
               .DynamicMock(out _currentUser)
               .DynamicMock(out _presenter);

            _container.Inject(_presenter);
            _container.Inject(new Mock<IRepository<Employee>>().Object);

            _target = new TestResourceRPCViewBuilder()
                     .WithListView(_listView)
                     .WithDetailView(_detailView)
                     .WithSearchView(_searchView)
                     .WithCurrentUser(_currentUser);

            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestCommandPropertyReturnsCommandValueFromQueryString()
        {
            using (_simulator.SimulateRequest(new Uri(BuildTestUrl())))
            {
                Assert.AreEqual(TEST_COMMAND, _target.Command);
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestArgumentPropertyReturnsArgumentValueFromQueryString()
        {
            using (_simulator.SimulateRequest(new Uri(BuildTestUrl())))
            {
                Assert.AreEqual(TEST_ARGUMENT, _target.Argument);
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestPresenterPropertyReturnsNull()
        {
            Assert.IsNull(_target.Presenter);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestShowEntityOnDetailViewPassesEntityToDetailViewShowEntity()
        {
            var expected = new Employee();

            using (_mocks.Record())
            {
                _detailView.ShowEntity(expected);
                _detailView.ShowEntity(null);
            }

            using (_mocks.Playback())
            {
                _target.ShowEntityOnDetailView(expected);
                _target.ShowEntityOnDetailView(null);
            }
        }

        [TestMethod]
        public void TestShowDetailViewControlsCallsDetailViewSetViewControlsVisible()
        {
            using (_mocks.Record())
            {
                _detailView.SetViewControlsVisible(true);
                _detailView.SetViewControlsVisible(false);
            }

            using (_mocks.Playback())
            {
                _target.ShowDetailViewControls(true);
                _target.ShowDetailViewControls(false);
            }
        }

        [TestMethod]
        public void TestPageInitCallsRPCPresenterOnViewInitWithIUser()
        {
            using (_mocks.Record())
            {
                _presenter.OnViewInit(_target.IUser);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Init");
            }
        }

        [TestMethod]
        public void TestPageLoadPassesViewsToRPCPresenter()
        {
            using (_mocks.Record())
            {
                _presenter.ListView = _listView;
                _presenter.DetailView = _detailView;
                _presenter.SearchView = _searchView;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadCallsRPCPresenterOnViewInitializedWhenNotPostBack()
        {
            _target = new TestResourceRPCViewBuilder()
               .WithPostBack(false);

            using (_mocks.Record())
            {
                _presenter.OnViewInitialized();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadDoesNotCallRPCPresenterOnViewInitializedWhenIsPostBack()
        {
            _target = new TestResourceRPCViewBuilder()
               .WithPostBack(true);

            using (_mocks.Record())
            {
                DoNotExpect.Call(_presenter.OnViewInitialized);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestRPCCommandThrowsExceptionWhenCommandIsNull()
        {
            RPCCommands expected;
            using (_mocks.Record()) { }

            using (_mocks.Playback())
            {
                MyAssert.Throws(() => expected = _target.RPCCommand,
                    typeof(NullReferenceException));
            }
        }

        [TestMethod]
        public void TestRPCCommandReturnsCorrectRPCCommand()
        {
            foreach (RPCCommands command in Enum.GetValues(typeof(RPCCommands)))
            {
                var uri = new Uri(String.Format(TEST_URL_FORMAT,
                    RPCQueryStringValues.COMMAND,
                    command.RPCCommandToString(),
                    RPCQueryStringValues.ARGUMENT, TEST_ARGUMENT));

                using (_simulator.SimulateRequest(uri))
                {
                    Assert.AreEqual(command, _target.RPCCommand);
                }

                _mocks.ReplayAll();
            }
        }
    }

    internal class TestResourceRPCViewBuilder : TestDataBuilder<TestResourceRPCView>
    {
        #region Private Members

        private IListView<Employee> _listView;
        private IDetailView<Employee> _detailView;
        private ISearchView<Employee> _searchView;
        private bool? _isPostBack = true;
        private IUser _iUser;

        #endregion

        #region Exposed Methods

        public override TestResourceRPCView Build()
        {
            var obj = new TestResourceRPCView();
            if (_listView != null)
                obj.SetListView(_listView);
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            if (_searchView != null)
                obj.SetSearchView(_searchView);
            if (_isPostBack != null)
                obj.SetPostBack(_isPostBack.Value);
            if (_iUser != null)
                obj.SetUser(_iUser);
            return obj;
        }

        public TestResourceRPCViewBuilder WithListView(IListView<Employee> listView)
        {
            _listView = listView;
            return this;
        }

        public TestResourceRPCViewBuilder WithDetailView(IDetailView<Employee> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestResourceRPCViewBuilder WithSearchView(ISearchView<Employee> searchView)
        {
            _searchView = searchView;
            return this;
        }

        public TestResourceRPCViewBuilder WithPostBack(bool isPostBack)
        {
            _isPostBack = isPostBack;
            return this;
        }

        public TestResourceRPCViewBuilder WithCurrentUser(IUser user)
        {
            _iUser = user;
            return this;
        }

        #endregion
    }

    internal class TestResourceRPCView : ResourceRPCView<Employee>
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

        public void SetPostBack(bool postBack)
        {
            _isMvpPostBack = postBack;
        }

        public void SetUser(IUser user)
        {
            _iUser = user;
        }

        public override Expression<Func<Employee, bool>> GenerateExpression()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

#else
        [TestMethod]
        public void TestRunningInDebugMode()
        {
            Assert.Fail("This, and many other tests in this project must be run in DEBUG mode.");
        }
    }
#endif
}
