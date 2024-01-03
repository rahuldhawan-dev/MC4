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
    /// Summary description for CrewAssignmentSearchPresenterTest.
    /// </summary>
    [TestClass]
    public class CrewAssignmentSearchPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private ISearchView<CrewAssignment> _view;
        private TestCrewAssignmentSearchPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view);

            _target = new TestCrewAssignmentSearchPresenterBuilder(_view);
        }

        [TestInitialize]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion
    }

    internal class TestCrewAssignmentSearchPresenterBuilder : TestDataBuilder<TestCrewAssignmentSearchPresenter>
    {
        #region Private Members

        private readonly ISearchView<CrewAssignment> _view;

        #endregion

        #region Constructors

        internal TestCrewAssignmentSearchPresenterBuilder(ISearchView<CrewAssignment> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestCrewAssignmentSearchPresenter Build()
        {
            var obj = new TestCrewAssignmentSearchPresenter(_view);
            return obj;
        }

        #endregion
    }

    internal class TestCrewAssignmentSearchPresenter : CrewAssignmentSearchPresenter
    {
        #region Constructors

        public TestCrewAssignmentSearchPresenter(ISearchView<CrewAssignment> view) : base(view)
        {
        }

        #endregion
    }
}
