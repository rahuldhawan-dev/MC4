using System;
using System.Web.UI.WebControls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.RestorationProductCodes;
using WorkOrders.Views.RestorationProductCodes;
using MMSINC.Testing.MSTest.TestExtensions;

namespace _271ObjectTests.Tests.Unit.Presenters.RestorationProductCodes
{
    [TestClass]
    public class RestorationProductCodeListPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IRestorationProductCodeListView _view;
        private IRepository<RestorationProductCode> _repository;
        private TestRestorationProductCodeListPresenter _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository);

            _target = new TestRestorationProductCodeListPresenterBuilder(_view);
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestOnViewLoadedWiresUpCommands()
        {
            using (_mocks.Record())
            {
                _view.DeleteCommand += null;
                LastCall.IgnoreArguments();
                _view.InsertCommand += null;
                LastCall.IgnoreArguments();
            }
            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestDeleteCommandDeletesEntity()
        {
            _target = new TestRestorationProductCodeListPresenterBuilder(_view).WithRepository(_repository);
            var id = 1;
            var rac = new RestorationProductCode() { RestorationProductCodeID = id };
            var eventArgs = new GridViewDeleteEventArgs(id);
            eventArgs.Keys.Add("RestorationProductCodeID", id);

            using (_mocks.Record())
            {
                SetupResult.For(_repository.Get(id)).Return(rac);
                _view.ErrorMessage = RestorationProductCodeListPresenter.RECORD_DELETED;
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_DeleteCommand", new[] {
                    new object(), eventArgs
                });
                Assert.IsFalse(eventArgs.Cancel);
            }
        }

        [TestMethod]
        public void TestDeleteCommandDoesNotDeleteEntity()
        {
            _target =
                new TestRestorationProductCodeListPresenterBuilder(_view).
                    WithRepository(_repository);
            var id = 1;
            var rac = new RestorationProductCode()
            {
                RestorationProductCodeID = id
            };
            rac.PrimaryWorkDescriptions.Add(new WorkDescription() { WorkDescriptionID = 42 });
            var eventArgs = new GridViewDeleteEventArgs(id);
            eventArgs.Keys.Add("RestorationProductCodeID", id);

            using (_mocks.Record())
            {
                SetupResult.For(_repository.Get(id)).Return(rac);
                _view.ErrorMessage = rac.DeletingErrorMessage;
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_DeleteCommand", new[] {
                    new object(), eventArgs
                });
                Assert.IsTrue(eventArgs.Cancel);
            }
        }

        [TestMethod]
        public void TestInsertCommandInsertsNewEntity()
        {
            _target =
                new TestRestorationProductCodeListPresenterBuilder(_view).
                    WithRepository(_repository);
            var code = "1111";
            var eventArgs = new RestorationProductInsertEventArgs(code);

            using (_mocks.Record())
            {
                _repository.InsertNewEntity(null);
                LastCall.IgnoreArguments();
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_InsertCommand", new[] {
                    new object(), eventArgs
                });
            }
        }

        #endregion
    }

    internal class TestRestorationProductCodeListPresenterBuilder : TestDataBuilder<TestRestorationProductCodeListPresenter>
    {
        #region Private Members

        private readonly IListView<RestorationProductCode> _view;
        private IRepository<RestorationProductCode> _repository;

        #endregion

        #region Constructors

        internal TestRestorationProductCodeListPresenterBuilder(IListView<RestorationProductCode> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestRestorationProductCodeListPresenter Build()
        {
            var obj = new TestRestorationProductCodeListPresenter(_view);
            if (_repository != null)
                obj.Repository = _repository;
            return obj;
        }

        public TestRestorationProductCodeListPresenterBuilder WithRepository(IRepository<RestorationProductCode> repository)
        {
            _repository = repository;
            return this;
        }

        #endregion

    }

    internal class TestRestorationProductCodeListPresenter : RestorationProductCodeListPresenter
    {
        #region Constructors

        public TestRestorationProductCodeListPresenter(
            IListView<RestorationProductCode> view)
            : base(view) { }

        #endregion
    }
}
