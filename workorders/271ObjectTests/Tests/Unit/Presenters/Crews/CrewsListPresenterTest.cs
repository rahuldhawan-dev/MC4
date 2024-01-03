using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using WorkOrders.Presenters.Crews;

namespace _271ObjectTests.Tests.Unit.Presenters.Crews
{
    /// <summary>
    /// Summary description for CrewsListPresenterTestTest
    /// </summary>
    [TestClass]
    public class CrewsListPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IListView<Crew> _view;
        private TestCrewsListPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _view = _mocks.DynamicMock<IListView<Crew>>();
            _target = new TestCrewsListPresenterBuilder(_view);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            _mocks.ReplayAll();

            MyAssert.DoesNotThrow(
                () => _target = new TestCrewsListPresenter(_view));
        }
    }

    internal class TestCrewsListPresenterBuilder : TestDataBuilder<TestCrewsListPresenter>
    {
        #region Private Members

        private readonly IListView<Crew> _view;

        #endregion

        #region Constructors

        private TestCrewsListPresenterBuilder()
        {
        }

        internal TestCrewsListPresenterBuilder(IListView<Crew> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestCrewsListPresenter Build()
        {
            var obj = new TestCrewsListPresenter(_view);
            return obj;
        }
        #endregion
    }

    internal class TestCrewsListPresenter : CrewsListPresenter
    {
        #region Constructors

        internal TestCrewsListPresenter(IListView<Crew> view)
            : base(view)
        {
        }

        #endregion
    }
}