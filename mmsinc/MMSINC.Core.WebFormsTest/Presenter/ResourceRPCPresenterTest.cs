using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.UI;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINCTestImplementation;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;
using StructureMap;
using StructureMap.Pipeline;
using Subtext.TestLibrary;

namespace MMSINC.Core.WebFormsTest.Presenter
{
    /// <summary>
    /// Summary description for EntityResourceRPCPresenterTest
    /// </summary>
    [TestClass]
    public class ResourceRPCPresenterTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private MockRepository _mocks;
        private IRepository<Employee> _repository;
        private IDetailView<Employee> _detailView;
        private IListView<Employee> _listView;
        private IResourceRPCView<Employee> _view;
        private EmployeeResourceRPCPresenter _target;

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void EntityResourceRPCPresenterTestInitialize()
        {
            _mocks = new MockRepository();
            _simulator = new HttpSimulator();
            _mocks
               .DynamicMock(out _repository)
               .DynamicMock(out _detailView)
               .DynamicMock(out _listView)
               .DynamicMock(out _view);
            _target = new EmployeeResourceRPCPresenter(_view, _repository) {
                ListView = _listView,
                DetailView = _detailView
            };
        }

        [TestCleanup]
        public void EntityResourceRPCPresenterTestCleanup()
        {
            _mocks.VerifyAll();
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestViewCommandDisplaysDetailAndNotList()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.VIEW, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            _mocks.ReplayAll();

            _target.OnViewLoaded();

            Assert.IsNotNull(((MockedResourceRPCView<Employee>)_view).DetailVisible);
            Assert.IsTrue(((MockedResourceRPCView<Employee>)_view).DetailVisible.Value);
            Assert.IsNotNull(((MockedResourceRPCView<Employee>)_view).DetailVisible);
            Assert.IsFalse(((MockedResourceRPCView<Employee>)_view).ListVisible.Value);
        }

        [TestMethod]
        public void TestCreateCommandDisplaysDetailAndNotList()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.CREATE, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            _mocks.ReplayAll();

            _target.OnViewLoaded();

            Assert.IsNotNull(((MockedResourceRPCView<Employee>)_view).DetailVisible);
            Assert.IsTrue(((MockedResourceRPCView<Employee>)_view).DetailVisible.Value);
            Assert.IsNotNull(((MockedResourceRPCView<Employee>)_view).DetailVisible);
            Assert.IsFalse(((MockedResourceRPCView<Employee>)_view).ListVisible.Value);
        }

        [TestMethod]
        public void TestListCommandDisplaysListAndNotDetail()
        {
            // this argument is technically invalid for this command
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.LIST, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            _mocks.ReplayAll();

            _target.OnViewLoaded();

            Assert.IsNotNull(((MockedResourceRPCView<Employee>)_view).DetailVisible);
            Assert.IsFalse(((MockedResourceRPCView<Employee>)_view).DetailVisible.Value);
            Assert.IsNotNull(((MockedResourceRPCView<Employee>)_view).DetailVisible);
            Assert.IsTrue(((MockedResourceRPCView<Employee>)_view).ListVisible.Value);
        }

        [TestMethod]
        public void TestUpdateCommandDisplaysDetailAndNotList()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.UPDATE, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            _mocks.ReplayAll();

            _target.OnViewLoaded();

            Assert.IsNotNull(((MockedResourceRPCView<Employee>)_view).DetailVisible);
            Assert.IsTrue(((MockedResourceRPCView<Employee>)_view).DetailVisible.Value);
            Assert.IsNotNull(((MockedResourceRPCView<Employee>)_view).DetailVisible);
            Assert.IsFalse(((MockedResourceRPCView<Employee>)_view).ListVisible.Value);
        }

        [TestMethod]
        public void TestUpdateCommandSetsSelectedEntityDataKeyOnRepository()
        {
            Expression<Func<Employee, bool>> expression = o => true;
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.UPDATE, 1.ToString(), _detailView, expression);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _repository.SetSelectedDataKeyForRPC("1", _view.GenerateExpression());
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestUpdateCommandCallsShowEntityOnDetailView()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.UPDATE, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _detailView.ShowEntity(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestUpdateCommandSetsDetailsViewButtonsVisible()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.UPDATE, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _detailView.SetViewControlsVisible(true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestViewCommandSetsSelectedEntityDataKeyOnRepository()
        {
            Expression<Func<Employee, bool>> expression = o => true;
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.VIEW, 1.ToString(), _detailView, expression);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _repository.SetSelectedDataKeyForRPC("1", expression);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestViewCommandCallsShowEntityOnDetailView()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.VIEW, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _detailView.ShowEntity(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestViewCommandSetsDetailsViewButtonsVisible()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.VIEW, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _detailView.SetViewControlsVisible(true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestUpdateCommandSetsDetailViewToEditMode()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.UPDATE, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _detailView.SetViewMode(DetailViewMode.Edit);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestCreateCommandSetsDetailViewToInsertMode()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.CREATE, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _detailView.SetViewMode(DetailViewMode.Insert);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestViewCommandSetsDetailViewReadOnly()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.VIEW, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _detailView.SetViewMode(DetailViewMode.ReadOnly);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestOnViewLoadedWiresUpChildViewEventHandlers()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.LIST, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            using (_mocks.Record())
            {
                _listView.SelectedIndexChanged += null;
                LastCall.IgnoreArguments();
                _view.BackToListClicked += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestListViewSelectedIndexChangedCallsSetRPCViewMode()
        {
            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Detail);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_listView,
                    "SelectedIndexChanged");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestSetRPCViewModeDetailsCallsRPCToggleListRPCToggleDetail()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.LIST, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            _mocks.ReplayAll();
            _view.SetViewMode(ResourceViewMode.Detail);

            Assert.IsFalse(((MockedResourceRPCView<Employee>)_view).ListVisible.Value);
            Assert.IsTrue(((MockedResourceRPCView<Employee>)_view).DetailVisible.Value);
        }

        [TestMethod]
        public void TestSetRPCViewModeListCallsRPCToggleListRPCToggleDetail()
        {
            _view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.LIST, 1.ToString(), _detailView);
            _target =
                new EmployeeResourceRPCPresenter(_view, _repository) {
                    ListView = _listView,
                    DetailView = _detailView
                };

            _mocks.ReplayAll();
            _view.SetViewMode(ResourceViewMode.List);

            Assert.IsTrue(((MockedResourceRPCView<Employee>)_view).ListVisible.Value);
            Assert.IsFalse(((MockedResourceRPCView<Employee>)_view).DetailVisible.Value);
        }

        [TestMethod]
        public void TestBackToListClickedCallsSetRPCViewMode()
        {
            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_view,
                    "BackToListClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestChangeViewCommandRedirectsViewWithNewCommand()
        {
            string setupUrl = "cmd=" + RPCCommandNames.LIST,
                   expectedUrl = "cmd=" + RPCCommandNames.VIEW;

            using (_mocks.Record())
            {
                SetupResult.For(_view.Command).Return(RPCCommandNames.LIST);
                SetupResult.For(_view.RelativeUrl).Return(setupUrl);
                _view.Redirect(expectedUrl);
            }

            using (_mocks.Playback())
            {
                _target.ChangeViewCommand(RPCCommands.View);
            }
        }
    }

    internal class MockedResourceRPCView : IResourceRPCView
    {
        #region Private Members

        private readonly NameValueCollection _queryString;

        private bool? _detailVisible,
                      _listVisible,
                      _detailReadOnly,
                      _detailControlsVisible;

        #endregion

        #region Properties

        public ResourceViewMode CurrentMode { get; protected set; }
        public virtual string RedirectURL { get; set; }

        public IPage IPage
        {
            get { throw new NotImplementedException(); }
        }

        public string RelativeUrl
        {
            get { throw new NotImplementedException(); }
        }

        public bool? DetailControlsVisible
        {
            get { return _detailControlsVisible; }
        }

        public bool? DetailReadOnly
        {
            get { return _detailReadOnly; }
        }

        public bool? DetailVisible
        {
            get { return _detailVisible; }
        }

        public bool? ListVisible
        {
            get { return _listVisible; }
        }

        #endregion

        #region Constructors

        public MockedResourceRPCView(string command, string argument)
        {
            _queryString = new NameValueCollection(2);
            _queryString[RPCQueryStringValues.COMMAND] = command;
            _queryString[RPCQueryStringValues.ARGUMENT] = argument;
        }

        #endregion

        #region IResourceView Members

        public string Command
        {
            get { return _queryString[RPCQueryStringValues.COMMAND]; }
        }

        public string Argument
        {
            get { return _queryString[RPCQueryStringValues.ARGUMENT]; }
        }

        public RPCCommands RPCCommand
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler LoadComplete;
        public event EventHandler BackToListClicked;

        public virtual void ToggleList(bool visible)
        {
            _listVisible = visible;
        }

        public virtual void ToggleDetail(bool visible)
        {
            _detailVisible = visible;
        }

        public virtual void SetDetailMode(DetailViewMode mode)
        {
            switch (mode)
            {
                case DetailViewMode.ReadOnly:
                    _detailReadOnly = true;
                    break;
                case DetailViewMode.Edit:
                case DetailViewMode.Insert:
                    _detailReadOnly = false;
                    break;
            }
        }

        public virtual void ShowDetailViewControls(bool show)
        {
            _detailControlsVisible = show;
        }

        public void SetViewMode(ResourceViewMode mode)
        {
            switch (mode)
            {
                case ResourceViewMode.List:
                    ToggleList(true);
                    ToggleDetail(false);
                    break;
                default: // ResourceViewMode.Detail:
                    ToggleList(false);
                    ToggleDetail(true);
                    break;
            }

            CurrentMode = mode;
        }

        public void Redirect(string url) { }

        #endregion

        #region IControl Members

        public bool Visible
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ClientID
        {
            get { throw new NotImplementedException(); }
        }

        public bool EnableViewState
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Control FindControl(string id)
        {
            throw new NotImplementedException();
        }

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            throw new NotImplementedException();
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IView Members

        public IServer IServer
        {
            get { throw new NotImplementedException(); }
        }

        public string AppRelativeVirtualPath
        {
            get { throw new NotImplementedException(); }
        }

        public IRoles IRoles
        {
            get { throw new NotImplementedException(); }
        }

        public void AddControl(Control control)
        {
            throw new NotImplementedException();
        }

        public void DataBind()
        {
            throw new NotImplementedException();
        }

        public string ResolveClientUrl(string url)
        {
            throw new NotImplementedException();
        }

        public IClientScriptManager ClientScriptManager
        {
            get { throw new NotImplementedException(); }
        }

        public virtual ISessionState ISession
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }

    internal class MockedResourceRPCView<TEntity> : MockedResourceRPCView, IResourceRPCView<TEntity>
        where TEntity : class
    {
        #region Private Members

        private readonly IDetailView<TEntity> _detailView;
        private IResourceRPCPresenter<TEntity> _rpcPresenter;
        private Expression<Func<TEntity, bool>> _generateExpression;

        #endregion

        #region Properties

        public IRepository<TEntity> Repository
        {
            get { throw new NotImplementedException(); }
        }

        public IResourcePresenter<TEntity> Presenter
        {
            get { throw new NotImplementedException(); }
        }

        public IListView<TEntity> ListView
        {
            get { throw new NotImplementedException(); }
        }

        public IDetailView<TEntity> DetailView
        {
            get { return _detailView; }
        }

        public ISearchView<TEntity> SearchView
        {
            get { throw new NotImplementedException(); }
        }

        public IResourceRPCPresenter<TEntity> RPCPresenter
        {
            get
            {
                if (_rpcPresenter == null)
                    _rpcPresenter =
                        DependencyResolver.Current.GetService<IContainer>().GetInstance<IResourceRPCPresenter<TEntity>>(
                            new ExplicitArguments(new Dictionary<string, object> {
                                {"view", this},
                                {"repository", Repository}
                            }));
                return _rpcPresenter;
            }
        }

        #endregion

        #region Constructors

        public MockedResourceRPCView(
            string command, string argument, IDetailView<TEntity> detailView)
            : base(command, argument)
        {
            _detailView = detailView;
        }

        public MockedResourceRPCView(
            string command, string argument, IDetailView<TEntity> detailView,
            Expression<Func<TEntity, bool>> expression)
            : base(command, argument)
        {
            _detailView = detailView;
            _generateExpression = expression;
        }

        #endregion

        #region IResourceView Members

        public override void SetDetailMode(DetailViewMode mode)
        {
            base.SetDetailMode(mode);
            DetailView.SetViewMode(mode);
        }

        public void ShowEntityOnDetailView(TEntity entity)
        {
            DetailView.ShowEntity(entity);
        }

        public Expression<Func<TEntity, bool>> GenerateExpression()
        {
            Expression<Func<TEntity, bool>> expression = o => true;
            return _generateExpression ?? expression;
        }

        public override void ShowDetailViewControls(bool show)
        {
            base.ShowDetailViewControls(show);
            DetailView.SetViewControlsVisible(show);
        }

        #endregion
    }
}
