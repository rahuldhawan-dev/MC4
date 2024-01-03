using System;
using System.Web.UI.WebControls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.RestorationAccountingCodes;
using WorkOrders.Views.RestorationAccountingCodes;
using MMSINC.Testing.MSTest.TestExtensions;

namespace _271ObjectTests.Tests.Unit.Presenters.RestorationAccountingCodes
{
    [TestClass]
    public class RestorationAccountingCodeListPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IRestorationAccountingCodeListView _view;
        private IRepository<RestorationAccountingCode> _repository;
        private TestRestorationAccountingCodeListPresenter _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository);

            _target = new TestRestorationAccountingCodeListPresenterBuilder(_view);
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
            _target = new TestRestorationAccountingCodeListPresenterBuilder(_view).WithRepository(_repository);
            var id = 1;
            var rac = new RestorationAccountingCode() { RestorationAccountingCodeID = id};
            var eventArgs = new GridViewDeleteEventArgs(id);
            eventArgs.Keys.Add("RestorationAccountingCodeID", id);

            using(_mocks.Record())
            {
                SetupResult.For(_repository.Get(id)).Return(rac);
                _view.ErrorMessage = RestorationAccountingCodeListPresenter.RECORD_DELETED;
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
                new TestRestorationAccountingCodeListPresenterBuilder(_view).
                    WithRepository(_repository);
            var id = 1;
            var rac = new RestorationAccountingCode() {
                RestorationAccountingCodeID = id
            };
            rac.PrimaryWorkDescriptions.Add(new WorkDescription() { WorkDescriptionID = 42});
            var eventArgs = new GridViewDeleteEventArgs(id);
            eventArgs.Keys.Add("RestorationAccountingCodeID", id);

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
                new TestRestorationAccountingCodeListPresenterBuilder(_view).
                    WithRepository(_repository);
            var code = "1111";
            var subCode = "11";
            var eventArgs = new RestorationAccountingInsertEventArgs(code, subCode);
            
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

    internal class TestRestorationAccountingCodeListPresenterBuilder : TestDataBuilder<TestRestorationAccountingCodeListPresenter>
    {
        #region Private Members

        private readonly IListView<RestorationAccountingCode> _view;
        private IRepository<RestorationAccountingCode> _repository;

        #endregion

        #region Constructors

        internal TestRestorationAccountingCodeListPresenterBuilder(IListView<RestorationAccountingCode> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestRestorationAccountingCodeListPresenter Build()
        {
            var obj = new TestRestorationAccountingCodeListPresenter(_view);
            if (_repository != null)
                obj.Repository = _repository;
            return obj;
        }

        public TestRestorationAccountingCodeListPresenterBuilder WithRepository(IRepository<RestorationAccountingCode> repository)
        {
            _repository = repository;
            return this;
        }

        #endregion

    }

    internal class TestRestorationAccountingCodeListPresenter : RestorationAccountingCodeListPresenter
    {
        #region Constructors

        public TestRestorationAccountingCodeListPresenter(
            IListView<RestorationAccountingCode> view) : base(view) {}

        #endregion
    }
}
