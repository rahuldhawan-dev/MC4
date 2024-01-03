using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Interface;
using MMSINC.Testing;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using WorkOrders.Model;
using WorkOrders.Presenters.ContractorCrewAssignments;

namespace _271ObjectTests.Tests.Unit.Presenters.ContractorCrewAssignments
{ 
    [TestClass]
    public class ContractorCrewAssignmentsSearchPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private ISearchView<CrewAssignment> _view;
        private TestContractorCrewAssignmentSearchPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view);

            _target = new TestContractorCrewAssignmentSearchPresenterBuilder(_view);
        }

        [TestInitialize]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion
    }

    internal class TestContractorCrewAssignmentSearchPresenterBuilder : TestDataBuilder<TestContractorCrewAssignmentSearchPresenter>
    {
        #region Private Members

        private readonly ISearchView<CrewAssignment> _view;

        #endregion

        #region Constructors

        internal TestContractorCrewAssignmentSearchPresenterBuilder(ISearchView<CrewAssignment> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestContractorCrewAssignmentSearchPresenter Build()
        {
            var obj = new TestContractorCrewAssignmentSearchPresenter(_view);
            return obj;
        }

        #endregion
    }

    internal class TestContractorCrewAssignmentSearchPresenter : ContractorCrewAssignmentSearchPresenter
    {
        #region Constructors

        public TestContractorCrewAssignmentSearchPresenter(ISearchView<CrewAssignment> view)
            : base(view)
        {
        }

        #endregion
    }
}
