using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.CrewAssignments;

namespace _271ObjectTests.Tests.Unit.Presenters.CrewAssignments
{
    /// <summary>
    /// Summary description for CrewAssignmentResourceRPCPresenterTest.
    /// </summary>
    [TestClass]
    public class CrewAssignmentResourceRPCPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceRPCView<CrewAssignment> _view;
        private IRepository<CrewAssignment> _repository;
        private CrewAssignmentResourceRPCPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository);

            _target = new TestCrewAssignmentResourceRPCPresenterBuilder(_view, _repository);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestOnViewLoadedSetsViewModeToList()
        {
            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourceRPCPresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersResourceRPCPresenter<CrewAssignment>),
                "ResourcePresenters in this project should inherit from WorkOrdersResourceRPCPresenter, lest bad tings happen.");
        }
    }

    internal class TestCrewAssignmentResourceRPCPresenterBuilder : TestDataBuilder<CrewAssignmentResourceRPCPresenter>
    {
        #region Private Members

        private IResourceRPCView<CrewAssignment> _view;
        private IRepository<CrewAssignment> _repository;

        #endregion

        #region Constructors

        public TestCrewAssignmentResourceRPCPresenterBuilder(IResourceRPCView<CrewAssignment> view, IRepository<CrewAssignment> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override CrewAssignmentResourceRPCPresenter Build()
        {
            var obj = new CrewAssignmentResourceRPCPresenter(_view, _repository);
            return obj;
        }

        #endregion
    }
}
