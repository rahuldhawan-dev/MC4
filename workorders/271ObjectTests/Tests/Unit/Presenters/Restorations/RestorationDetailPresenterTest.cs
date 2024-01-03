using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using WorkOrders.Presenters.Restorations;

namespace _271ObjectTests.Tests.Unit.Presenters.Restorations
{
    /// <summary>
    /// Summary description for RestorationDetailPresenterTest.
    /// </summary>
    [TestClass]
    public class RestorationDetailPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailView<Restoration> _view;
        private TestRestorationDetailPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks.DynamicMock(out _view);

            _target = new TestRestorationDetailPresenterBuilder(_view);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestOnViewInitializedDoesNothing()
        {
            _mocks.CreateMock(out _view);

            _target = new TestRestorationDetailPresenterBuilder(_view);

            using (_mocks.Record())
            {
                // expect nothing!!!
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }
    }

    internal class TestRestorationDetailPresenterBuilder : TestDataBuilder<TestRestorationDetailPresenter>
    {
        #region Private Members

        private IDetailView<Restoration> _view;

        #endregion

        #region Constructors

        public TestRestorationDetailPresenterBuilder(IDetailView<Restoration> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestRestorationDetailPresenter Build()
        {
            var obj = new TestRestorationDetailPresenter(_view);
            return obj;
        }

        #endregion
    }

    internal class TestRestorationDetailPresenter : RestorationDetailPresenter
    {
        #region Constructors

        public TestRestorationDetailPresenter(IDetailView<Restoration> view) : base(view)
        {
        }

        #endregion
    }
}
