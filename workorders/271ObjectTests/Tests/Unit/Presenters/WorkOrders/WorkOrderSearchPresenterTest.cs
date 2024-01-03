using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using WorkOrders.Presenters.WorkOrders;

namespace _271ObjectTests.Tests.Unit.Presenters.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderSearchPresenterTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderSearchPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private ISearchView<WorkOrder> _view;
        private TestWorkOrderSearchPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _view = _mocks.DynamicMock<ISearchView<WorkOrder>>();
            _target = new TestWorkOrderSearchPresenterBuilder(_view);
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

            MyAssert.DoesNotThrow(() =>
                                  _target = new TestWorkOrderSearchPresenter(_view));
        }
    }

    internal class TestWorkOrderSearchPresenterBuilder : TestDataBuilder<TestWorkOrderSearchPresenter>
    {
        #region Private Members

        private readonly ISearchView<WorkOrder> _view;

        #endregion

        #region Constructors

        private TestWorkOrderSearchPresenterBuilder()
        {
        }

        internal TestWorkOrderSearchPresenterBuilder(ISearchView<WorkOrder> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderSearchPresenter Build()
        {
            var obj = new TestWorkOrderSearchPresenter(_view);
            return obj;
        }

        #endregion
    }

    internal class TestWorkOrderSearchPresenter : WorkOrderSearchPresenter
    {
        #region Constructors

        public TestWorkOrderSearchPresenter(ISearchView<WorkOrder> view)
            : base(view)
        {
        }

        #endregion
    }
}