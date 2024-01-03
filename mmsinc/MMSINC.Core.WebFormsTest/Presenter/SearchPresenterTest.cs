using System;
using System.Linq.Expressions;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;

namespace MMSINC.Core.WebFormsTest.Presenter
{
    [TestClass]
    public class SearchPresenterTest
    {
        #region Private Members

        private MockRepository _mocks;
        private ISearchView<Employee> _view;
        private ISearchPresenter<Employee> _target;
        private IRepository<Employee> _repository;

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void EntityDetailPresenterTestInitialize()
        {
            _mocks = new MockRepository();
            _view = _mocks.DynamicMock<ISearchView<Employee>>();
            _repository = _mocks.DynamicMock<IRepository<Employee>>();
            _target = new EmployeeSearchPresenter(_view) {
                Repository = _repository
            };
        }

        [TestCleanup]
        public void EntityDetailPresenterTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestOnViewInitializedDoesNothing()
        {
            // need to do full mocks to ensure that nothing is called
            // on them
            _view = _mocks.CreateMock<ISearchView<Employee>>();
            _repository = _mocks.CreateMock<IRepository<Employee>>();
            _target = new EmployeeSearchPresenter(_view) {
                Repository = _repository
            };

            _mocks.ReplayAll();

            _target.OnViewInitialized();
        }

        [TestMethod]
        public void TestOnViewLoadedWiresUpEventHandlers()
        {
            using (_mocks.Record())
            {
                _view.SearchClicked += null;
                LastCall.IgnoreArguments();

                _view.CancelClicked += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestSearchCommandDoesNothing()
        {
            // need full mocks to see what's really going on
            _view = _mocks.CreateMock<ISearchView<Employee>>();
            _repository = _mocks.CreateMock<IRepository<Employee>>();
            _target = new EmployeeSearchPresenter(_view) {
                Repository = _repository
            };

            using (_mocks.Record())
            {
                _view.SearchClicked += null;
                LastCall.IgnoreArguments();

                _view.CancelClicked += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var searchClickRaiser = new EventRaiser((IMockedObject)_view,
                    "SearchClicked");
                searchClickRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestCancelCommandDoesNothing()
        {
            // need full mocks to see what's really going on
            _view = _mocks.CreateMock<ISearchView<Employee>>();
            _repository = _mocks.CreateMock<IRepository<Employee>>();
            _target = new EmployeeSearchPresenter(_view) {
                Repository = _repository
            };

            using (_mocks.Record())
            {
                _view.SearchClicked += null;
                LastCall.IgnoreArguments();

                _view.CancelClicked += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var cancelClickRaiser = new EventRaiser((IMockedObject)_view,
                    "CancelClicked");
                cancelClickRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestGenerateExpressionReturnsGeneratedExpressionFromView()
        {
            Expression<Func<Employee, bool>> expected = o => true;
            Expression<Func<Employee, bool>> actual = null;

            using (_mocks.Record())
            {
                SetupResult.For(_view.GenerateExpression()).Return(expected);
            }

            using (_mocks.Playback())
            {
                actual = _target.GenerateExpression();
            }

            Assert.AreSame(expected, actual);
        }
    }
}
