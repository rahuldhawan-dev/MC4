using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.RestorationTypeCosts;

namespace _271ObjectTests.Tests.Unit.Presenters.RestorationTypeCosts
{
    /// <summary>
    /// Summary description for RestorationTypeCostResourcePresenterTest.
    /// </summary>
    [TestClass]
    public class RestorationTypeCostResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceView<RestorationTypeCost> _view;
        private IRepository<RestorationTypeCost> _repository;
        private RestorationTypeCostResourcePresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationTypeCostResourcePresenterTestInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository);

            _target = new TestRestorationTypeCostResourcePresenterBuilder(_view, _repository);
        }

        #endregion

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourcePresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersAdminResourcePresenter<RestorationTypeCost>),
                "ResourcePresenters in this project should inherit from WorkOrdersResourcePresenter, lest bad tings happen.");
        }
    }

    internal class TestRestorationTypeCostResourcePresenterBuilder : TestDataBuilder<RestorationTypeCostResourcePresenter>
    {
        #region Private Members

        private IResourceView<RestorationTypeCost> _view;
        private IRepository<RestorationTypeCost> _repository;

        #endregion

        #region Constructors

        public TestRestorationTypeCostResourcePresenterBuilder(IResourceView<RestorationTypeCost> view, IRepository<RestorationTypeCost> repository)
        {
            _view = view;
            _repository = repository;
        }
#endregion
        #region Exposed Methods

        public override RestorationTypeCostResourcePresenter Build()
        {
            var obj = new RestorationTypeCostResourcePresenter(_view, _repository);
            return obj;
        }

        #endregion
    }

    //internal class TestRestorationTypeCostResourcePresenter : RestorationTypeCostResourcePresenter
    //{
    //}
}
