using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class BaseContractorOverrideLaborCostViewModelTest : MapCallMvcInMemoryDatabaseTestBase<ContractorOverrideLaborCost>
    {
        #region Fields

        private ViewModelTester<BaseContractorOverrideLaborCostViewModel, ContractorOverrideLaborCost> _vmTester;
        private BaseContractorOverrideLaborCostViewModel _viewModel;
        private ContractorOverrideLaborCost _entity;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IContractorLaborCostRepository>().Use<ContractorLaborCostRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _entity = GetEntityFactory<ContractorOverrideLaborCost>().Create();
            _viewModel = new BaseContractorOverrideLaborCostViewModel(_container);
            _vmTester =
                new ViewModelTester<BaseContractorOverrideLaborCostViewModel, ContractorOverrideLaborCost>(_viewModel,
                    _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Cost);
            _vmTester.CanMapBothWays(x => x.EffectiveDate);
        }

        [TestMethod]
        public void TestContractorCanMapBothWays()
        {
            var contractor = GetEntityFactory<Contractor>().Create();
            _entity.Contractor = contractor;

            _vmTester.MapToViewModel();

            Assert.AreEqual(contractor.Id, _viewModel.Contractor);

            _entity.Contractor = null;
            _vmTester.MapToEntity();
            Assert.AreSame(contractor, _entity.Contractor);
        }

        [TestMethod]
        public void TestContractorLaborCostCanMapBothWays()
        {
            var contractor = GetEntityFactory<ContractorLaborCost>().Create();
            _entity.ContractorLaborCost = contractor;

            _vmTester.MapToViewModel();

            Assert.AreEqual(contractor.Id, _viewModel.ContractorLaborCost);

            _entity.ContractorLaborCost = null;
            _vmTester.MapToEntity();
            Assert.AreSame(contractor, _entity.ContractorLaborCost);
        }

        [TestMethod]
        public void TestOperatingCenterLaborCostCanMapBothWays()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenter = opc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Contractor);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ContractorLaborCost);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.Cost, (decimal)1.5, x => x.Percentage, null, 1);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EffectiveDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
        }

        [TestMethod]
        public void TestCostHasMinValueOfZeroRequirement()
        {
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.Cost, 0m, error: "Cost must be greater than or equal to zero.");
        }
        
        #endregion

        #endregion
    }
}
