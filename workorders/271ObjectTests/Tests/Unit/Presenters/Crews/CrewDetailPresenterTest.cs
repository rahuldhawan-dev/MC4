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
    /// Summary description for CrewDetailPresenterTestTest
    /// </summary>
    [TestClass]
    public class CrewDetailPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailView<Crew> _view;
        private TestCrewDetailPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _view = _mocks.DynamicMock<IDetailView<Crew>>();
            _target = new TestCrewDetailPresenterBuilder(_view);
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
                () => _target = new TestCrewDetailPresenter(_view));
        }
    }

    internal class TestCrewDetailPresenterBuilder : TestDataBuilder<TestCrewDetailPresenter>
    {
        #region Private Members

        private readonly IDetailView<Crew> _view;

        #endregion

        #region Constructors

        private TestCrewDetailPresenterBuilder()
        {
        }

        internal TestCrewDetailPresenterBuilder(IDetailView<Crew> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestCrewDetailPresenter Build()
        {
            var obj = new TestCrewDetailPresenter(_view);
            return obj;
        }

        #endregion
    }

    internal class TestCrewDetailPresenter : CrewDetailPresenter
    {
        #region Constructors

        internal TestCrewDetailPresenter(IDetailView<Crew> view)
            : base(view)
        {
        }

        #endregion
    }
}