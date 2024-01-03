using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using WorkOrders.Presenters.CrewAssignments;

namespace _271ObjectTests.Tests.Unit.Presenters.CrewAssignments
{
    /// <summary>
    /// Summary description for CrewAssignmentResourcePresenterTest.
    /// </summary>
    [TestClass]
    public class CrewAssignmentResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceView _view;
        private IRepository<CrewAssignment> _repository;
        private TestCrewAssignmentResourcePresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository);

            _target = new TestCrewAssignmentResourcePresenterBuilder(_view, _repository);
        }

        #endregion
    }

    internal class TestCrewAssignmentResourcePresenterBuilder : TestDataBuilder<TestCrewAssignmentResourcePresenter>
    {
        #region Private Members

        private readonly IResourceView _view;
        private readonly IRepository<CrewAssignment> _repository;

        #endregion

        #region Constructors

        internal TestCrewAssignmentResourcePresenterBuilder(IResourceView view, IRepository<CrewAssignment> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestCrewAssignmentResourcePresenter Build()
        {
            var obj = new TestCrewAssignmentResourcePresenter(_view, _repository);
            return obj;
        }

        #endregion
    }

    internal class TestCrewAssignmentResourcePresenter : CrewAssignmentResourcePresenter
    {
        #region Constructors

        public TestCrewAssignmentResourcePresenter(IResourceView view, IRepository<CrewAssignment> repository) : base(view, repository)
        {
        }

        #endregion
    }
}
