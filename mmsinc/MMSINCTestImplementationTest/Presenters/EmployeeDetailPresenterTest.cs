using System;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using MMSINCTestImplementation.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;

namespace MMSINCTestImplementationTest.Presenters
{
    /// <summary>
    /// Summary description for EmployeeDetailPresenterTestTest
    /// </summary>
    [TestClass]
    public class EmployeeDetailPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IRepository<Employee> _repository;
        private IEmployeeDetailView _view;
        private TestEmployeeDetailPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _view = _mocks.DynamicMock<IEmployeeDetailView>();
            _repository = _mocks.DynamicMock<IRepository<Employee>>();
            _target = new TestEmployeeDetailPresenterBuilder(_view)
               .WithRepository(_repository);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestGenericConstructorSetsViewProperty()
        {
            var view = _mocks.CreateMock<IDetailView<Employee>>();
            _target = new TestEmployeeDetailPresenter(view);

            Assert.AreSame(view, _target.View);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSpecificConstructorSetsEmployeeDetailViewProperty()
        {
            _view = _mocks.CreateMock<IEmployeeDetailView>();
            _target = new TestEmployeeDetailPresenter(_view);

            Assert.AreSame(_view, _target.EmployeeDetailView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnViewLoadedWiresEventHandlersToView()
        {
            using (_mocks.Record())
            {
                _view.MenuEmployeesClicked += null;
                LastCall.IgnoreArguments();

                _view.MenuEmployeeClicked += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestMenuEmployeeClickCommandCallsToggleControlOnViewWithProperArguments()
        {
            using (_mocks.Record())
            {
                _view.ToggleControl(EmployeeDetailPresenter.Controls.DETAIL, true, true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var menuEmployeeClickedRaiser =
                    new EventRaiser((IMockedObject)_view, "MenuEmployeeClicked");
                menuEmployeeClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestMenuEmployeesClickCommandCallsToggleControlOnViewWithProperArguments()
        {
            using (_mocks.Record())
            {
                _view.ToggleControl(EmployeeDetailPresenter.Controls.C_EMPLOYEES, true, true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var menuEmployeesClickedRaiser =
                    new EventRaiser((IMockedObject)_view, "MenuEmployeesClicked");
                menuEmployeesClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestMenuTerritoriesClickCommandCallsToggleControlOnViewWithProperArguments()
        {
            using (_mocks.Record())
            {
                _view.ToggleControl(EmployeeDetailPresenter.Controls.C_TERRITORIES, true, true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var menuTerritoriesClickedRaiser =
                    new EventRaiser((IMockedObject)_view, "MenuTerritoriesClicked");
                menuTerritoriesClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestMenuOrdersClickCommandCallsToggleControlOnViewWithProperArguments()
        {
            using (_mocks.Record())
            {
                _view.ToggleControl(EmployeeDetailPresenter.Controls.C_ORDERS, true, true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var menuOrdersClickedRaiser =
                    new EventRaiser((IMockedObject)_view, "MenuOrdersClicked");
                menuOrdersClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestViewInsertingCommandCallsToggleControlOnViewWithProperArguments()
        {
            using (_mocks.Record())
            {
                _view.ToggleControl(EmployeeDetailPresenter.Controls.MENU, true, false);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var insertingRaiser =
                    new EventRaiser((IMockedObject)_view, "Inserting");
                insertingRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }
    }

    internal class TestEmployeeDetailPresenterBuilder : TestDataBuilder<TestEmployeeDetailPresenter>
    {
        #region Private Members

        private IRepository<Employee> _repository;
        private readonly IEmployeeDetailView _view;

        #endregion

        #region Constructors

        public TestEmployeeDetailPresenterBuilder(IEmployeeDetailView view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestEmployeeDetailPresenter Build()
        {
            var obj = new TestEmployeeDetailPresenter(_view);
            if (_repository != null)
                obj.Repository = _repository;
            return obj;
        }

        public TestEmployeeDetailPresenterBuilder WithRepository(IRepository<Employee> repository)
        {
            _repository = repository;
            return this;
        }

        #endregion
    }

    internal class TestEmployeeDetailPresenter : EmployeeDetailPresenter
    {
        #region Constructors

        public TestEmployeeDetailPresenter(IDetailView<Employee> view)
            : base(view) { }

        public TestEmployeeDetailPresenter(IEmployeeDetailView view)
            : base(view) { }

        #endregion
    }
}
