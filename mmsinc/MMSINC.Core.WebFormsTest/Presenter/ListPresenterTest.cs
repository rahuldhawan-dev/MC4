using System;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;

namespace MMSINC.Core.WebFormsTest.Presenter
{
    /// <summary>
    /// Summary description for EntityListPresenterTest
    /// </summary>
    [TestClass]
    public class ListPresenterTest
    {
        #region Private Members

        private MockRepository _mocks;
        private IListView<Employee> _view;
        private IRepository<Employee> _repository;
        private IListPresenter<Employee> _target;

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void EntityListPresenterTestInitialize()
        {
            _mocks = new MockRepository();
            _mocks
               .DynamicMock(out _view)
               .DynamicMock(out _repository);
            _target = new EmployeesListPresenter(_view) {
                Repository = _repository
            };
        }

        [TestCleanup]
        public void EntityListPresenterTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestOnViewInitDoesNothing()
        {
            _mocks
               .CreateMock(out _view)
               .CreateMock(out _repository);
            _target = new EmployeesListPresenter(_view) {
                Repository = _repository
            };

            _mocks.ReplayAll();

            _target.OnViewInit();
        }

        [TestMethod]
        public void TestOnViewLoadedSetsEventHandlersOnViewAndRepository()
        {
            using (_mocks.Record())
            {
                _view.SelectedIndexChanged += null;
                LastCall.IgnoreArguments();

                _view.DataSourceCreating += null;
                LastCall.IgnoreArguments();

                _view.CreateClicked += null;
                LastCall.IgnoreArguments();

                _repository.EntityInserted += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestViewLoadCompleteDoesNotCallRepositorySetDataKeyWhenViewDataKeyIsNull()
        {
            const object key = null;
            SetupResult.For(_view.SelectedDataKey).Return(key);

            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _repository.SetSelectedDataKey(key));
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var loadComplete = new EventRaiser((IMockedObject)_view,
                    "LoadComplete");
                loadComplete.Raise(null, null);
            }
        }

        [TestMethod]
        public void TestOnViewLoadedCallsSetViewControlsVisible()
        {
            using (_mocks.Record())
            {
                _view.SetViewControlsVisible(true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestOnViewInitializedDoesNothing()
        {
            // need full mocks here to make sure we know what's happening
            _mocks
               .CreateMock(out _view)
               .CreateMock(out _repository);
            var presenter = new EmployeesListPresenter(_view) {
                Repository = _repository
            };

            _mocks.ReplayAll();
            presenter.OnViewInitialized();
        }

        [TestMethod]
        public void TestRepositoryEntityInsertedSetsViewControlsVisible()
        {
            using (_mocks.Record())
            {
                _view.SetViewControlsVisible(true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_repository,
                    "EntityInserted");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        [TestMethod]
        public void TestOnSelectedIndexChangedCommandSetsSelectedEntityDataKey() // jmd - 10/20/08
        {
            var keyVal = "key";
            SetupResult.For(_view.SelectedDataKey).Return(keyVal);

            //record
            using (_mocks.Record())
            {
                _repository.SetSelectedDataKey(keyVal);
            }

            //playback
            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_view,
                    "SelectedIndexChanged");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestOnSelectedIndexChangedCommandShowsCreateButton()
        {
            using (_mocks.Record())
            {
                _view.SetViewControlsVisible(true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_view,
                    "SelectedIndexChanged");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestDataSourceCreatingCommandSetsObjectInstanceToRepository()
        {
            // this test method should technically replace one of the two below it.
            var e = new ObjectDataSourceEventArgs(null);

            _mocks.ReplayAll();

            _target.OnViewLoaded();
            IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_view,
                "DataSourceCreating");
            eventRaiser.Raise(null, e);
            Assert.AreSame(_repository, e.ObjectInstance);
        }

        [TestMethod]
        public void TestDataSourceCreatingCommandThrowsExceptionWithNullArgument()
        {
            _mocks.ReplayAll();

            _target.OnViewLoaded();
            IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_view,
                "DataSourceCreating");
            MyAssert.Throws<ArgumentNullException>(
                () => eventRaiser.Raise(null, null));
        }

        [TestMethod]
        public void TestCreateCommandHidesCreateButton()
        {
            using (_mocks.Record())
            {
                _view.SetViewControlsVisible(false);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_view,
                    "CreateClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }
    }
}
