using System;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementationTest.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;
using Subtext.TestLibrary;

namespace MMSINC.Core.WebFormsTest.Presenter
{
    /// <summary>
    /// Summary description for EntityDetailPresenterTest
    /// </summary>
    [TestClass]
    public class DetailPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private MockRepository _mocks;
        private HttpSimulator _simulator;
        private IDetailView<Employee> _detailView;
        private MockDetailPresenter _target;
        private IRepository<Employee> _repository;

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void EntityDetailPresenterTestInitialize()
        {
            _mocks = new MockRepository();
            _simulator = new HttpSimulator();
            _detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            _repository = _mocks.DynamicMock<IRepository<Employee>>();
            _target =
                new TestDetailPresenterBuilder(_detailView).WithRepository(
                    _repository);
        }

        [TestCleanup]
        public void EntityDetailPresenterTestCleanup()
        {
            _mocks.VerifyAll();
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestOnViewInitializedSetsViewControlsInVisible()
        {
            using (_mocks.Record())
            {
                // if this method is not called with
                // the specified argument, an exception
                // will be thrown
                _detailView.SetViewControlsVisible(false);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewLoadedSetsEventHandlersOnViewAndRepository()
        {
            _target =
                new TestDetailPresenterBuilder(_detailView).WithRepository(_repository);

            using (_mocks.Record())
            {
                _detailView.DiscardChangesClicked += null;
                LastCall.IgnoreArguments();

                _detailView.EditClicked += null;
                LastCall.IgnoreArguments();

                _detailView.Inserting += null;
                LastCall.IgnoreArguments();

                _detailView.Updating += null;
                LastCall.IgnoreArguments();

                // expect repository.CurrentEntityChanged to be set
                _repository.CurrentEntityChanged += null;
                LastCall.IgnoreArguments();

                _detailView.DeleteClicked += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestRepositoryEntityChangedDisplaysEntityAndSetsViewReadOnly()
        {
            using (_simulator.SimulateRequest())
            {
                //TODO: Replace EmployeeRepository with something that uses IRepository
                _repository = new MockEmployeeRepository();
                _target =
                    new TestDetailPresenterBuilder(_detailView).WithRepository(
                        _repository);

                using (_mocks.Record())
                {
                    _detailView.SetViewControlsVisible(true);
                    _detailView.SetViewMode(DetailViewMode.ReadOnly);
                    _detailView.ShowEntity(null);
                    LastCall.IgnoreArguments();
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    _repository.RestoreFromPersistedState(1);
                }
            }
        }

        [TestMethod]
        public void TestEditCommandShowsCurrentEntityInEditMode()
        {
            using (_simulator.SimulateRequest())
            {
                _repository = new MockEmployeeRepository();
                _target =
                    new TestDetailPresenterBuilder(_detailView).WithRepository(
                        _repository);

                using (_mocks.Record())
                {
                    _detailView.ShowEntity(null);
                    LastCall.IgnoreArguments();
                    _detailView.SetViewControlsVisible(true);
                    _detailView.SetViewMode(DetailViewMode.Edit);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    _repository.RestoreFromPersistedState(1);
                    IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_detailView,
                        "EditClicked");
                    eventRaiser.Raise(null, EventArgs.Empty);
                }
            }
        }

        [TestMethod]
        public void TestDiscardChangesCommandSetsViewReadOnlyWhenRepositoryCurrentEntityIsNotNull()
        {
            using (_simulator.SimulateRequest())
            {
                _repository = new MockEmployeeRepository();
                _target =
                    new TestDetailPresenterBuilder(_detailView).WithRepository(
                        _repository);

                using (_mocks.Record())
                {
                    _detailView.ShowEntity(null);
                    LastCall.IgnoreArguments();
                    _detailView.SetViewMode(DetailViewMode.ReadOnly);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    _repository.RestoreFromPersistedState(1);
                    IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_detailView,
                        "DiscardChangesClicked");
                    eventRaiser.Raise(null, EventArgs.Empty);
                }
            }
        }

        [TestMethod]
        public void TestDiscardChangesCommandDoesNothingWhenRepositoryCurrentEntityIsNull()
        {
            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _detailView.ShowEntity(null));
                DoNotExpect.Call(() => _detailView.SetViewMode(DetailViewMode.ReadOnly));
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_detailView,
                    "DiscardChangesClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestUpdatingCommandUpdatesEntity()
        {
            var employee = new Employee();
            _target =
                new TestDetailPresenterBuilder(_detailView).WithRepository(
                    _repository);

            using (_mocks.Record())
            {
                _repository.UpdateCurrentEntity(employee);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_detailView, "Updating").Raise(
                    null, new EntityEventArgs<Employee>(employee));
            }
        }

        [TestMethod]
        public void TestUpdatingCommandMakesViewShowEntityReadOnly()
        {
            var employee = new Employee();
            _target =
                new TestDetailPresenterBuilder(_detailView).WithRepository(
                    _repository);

            using (_mocks.Record())
            {
                SetupResult.For(_repository.CurrentEntity).Return(employee);
                _detailView.ShowEntity(employee);
                _detailView.SetViewMode(DetailViewMode.ReadOnly);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_detailView, "Updating").Raise(
                    null, new EntityEventArgs<Employee>(employee));
            }
        }

        [TestMethod]
        public void TestInsertingCommandInsertsEntity()
        {
            _target =
                new TestDetailPresenterBuilder(_detailView).WithRepository(
                    _repository);

            using (_mocks.Record())
            {
                _repository.InsertNewEntity(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser(
                    (IMockedObject)_detailView, "Inserting");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        [TestMethod]
        public void TestInsertingCommandMakesViewShowEntityReadOnly()
        {
            _target =
                new TestDetailPresenterBuilder(_detailView).WithRepository(
                    _repository);

            using (_mocks.Record())
            {
                _detailView.ShowEntity(null);
                LastCall.IgnoreArguments();
                _detailView.SetViewMode(DetailViewMode.ReadOnly);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_detailView,
                    "Inserting");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        [TestMethod]
        public void TestDeletingCommandDeletesEntity()
        {
            _target =
                new TestDetailPresenterBuilder(_detailView).WithRepository(
                    _repository);

            using (_mocks.Record())
            {
                _repository.DeleteEntity(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_detailView, "DeleteClicked");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        // integration test
        public void TestDetailViewTogglableControlsCreateMenuViewMenuItems()
        {
            // 
        }
    }

    internal class TestDetailPresenterBuilder : TestDataBuilder<MockDetailPresenter>
    {
        #region Private Members

        private IDetailView<Employee> _detailView;
        private IRepository<Employee> _repository;

        #endregion

        #region Constructors

        public TestDetailPresenterBuilder(IDetailView<Employee> detailView)
        {
            _detailView = detailView;
        }

        #endregion

        #region Events

        #endregion

        #region Exposed Methods

        public override MockDetailPresenter Build()
        {
            var dp = new MockDetailPresenter(_detailView);
            if (_repository != null)
                dp.SetRepository(_repository);
            return dp;
        }

        public TestDetailPresenterBuilder WithRepository(IRepository<Employee> repository)
        {
            _repository = repository;
            return this;
        }

        #endregion
    }

    internal class MockDetailPresenter : DetailPresenter<Employee>
    {
        #region Private Members

        private IDetailView<Employee> _detailView;
        private IRepository<Employee> _repository;

        #endregion

        #region Constructors

        public MockDetailPresenter(IDetailView<Employee> view)
            : base(view) { }

        #endregion

        #region Delegates

        public delegate void OnDisposeHandler(MockDetailPresenter dp);

        #endregion

        #region Properties

        public override IRepository<Employee> Repository
        {
            get { return _repository; }
        }

        #endregion

        #region Exposed Methods

        public void SetRepository(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        #endregion
    }
}
