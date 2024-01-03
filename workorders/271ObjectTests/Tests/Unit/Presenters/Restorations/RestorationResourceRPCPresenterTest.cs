using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.Restorations;

namespace _271ObjectTests.Tests.Unit.Presenters.Restorations
{
    /// <summary>
    /// Summary description for RestorationResourceRPCPresenterTest.
    /// </summary>
    [TestClass]
    public class RestorationResourceRPCPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceRPCView<Restoration> _view;
        private IRepository<Restoration> _repository;
        private TestRestorationResourceRPCPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository);

            _target = new TestRestorationResourceRPCPresenterBuilder(_view, _repository);
        }

        #endregion

        [TestMethod]
        public void TestDetailViewEditCommandChangesViewCommandToUpdate()
        {
            // using full mocks to ensure that only what's expected happens
            _mocks
                .CreateMock(out _view)
                .CreateMock(out _repository);

            _target = new TestRestorationResourceRPCPresenterBuilder(_view,
                _repository);

            string currentUrl = RPCQueryStringValues.COMMAND + "=" +
                                RPCCommandNames.VIEW,
                   expectedUrl = RPCQueryStringValues.COMMAND + "=" +
                                 RPCCommandNames.UPDATE;

            using (_mocks.Record())
            {
                SetupResult.For(_view.Command).Return(RPCCommandNames.VIEW);
                SetupResult.For(_view.RelativeUrl).Return(currentUrl);
                _view.Redirect(expectedUrl);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "DetailView_EditClicked");
            }
        }

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourceRPCPresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersResourceRPCPresenter<Restoration>),
                "ResourcePresenters in this project should inherit from WorkOrdersResourceRPCPresenter, lest bad tings happen.");
        }
    }

    internal class TestRestorationResourceRPCPresenterBuilder : TestDataBuilder<TestRestorationResourceRPCPresenter>
    {
        #region Private Members

        private IResourceRPCView<Restoration> _view;
        private IRepository<Restoration> _repository;

        #endregion

        #region Constructors

        public TestRestorationResourceRPCPresenterBuilder(IResourceRPCView<Restoration> view, IRepository<Restoration> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestRestorationResourceRPCPresenter Build()
        {
            var obj = new TestRestorationResourceRPCPresenter(_view, _repository);
            return obj;
        }

        #endregion
    }

    internal class TestRestorationResourceRPCPresenter : RestorationResourceRPCPresenter
    {
        #region Constructors

        public TestRestorationResourceRPCPresenter(IResourceRPCView<Restoration> view, IRepository<Restoration> repository) : base(view, repository)
        {
        }

        #endregion
    }
}
