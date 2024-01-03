using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreatePlanningPlantPublicWaterSupplyTest : MapCallMvcInMemoryDatabaseTestBase<PlanningPlantPublicWaterSupply>
    {
        #region Fields

        private ViewModelTester<CreatePlanningPlantPublicWaterSupply, PlanningPlantPublicWaterSupply> _vmTester;
        private CreatePlanningPlantPublicWaterSupply _viewModel;
        private PlanningPlantPublicWaterSupply _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<PlanningPlantPublicWaterSupply>().Create();
            _viewModel = _viewModelFactory.Build<CreatePlanningPlantPublicWaterSupply, PlanningPlantPublicWaterSupply>(_entity);
            _vmTester = new ViewModelTester<CreatePlanningPlantPublicWaterSupply, PlanningPlantPublicWaterSupply>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.PlanningPlant, GetEntityFactory<PlanningPlant>().Create());
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PlanningPlant);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PublicWaterSupply);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.PlanningPlant, GetEntityFactory<PlanningPlant>().Create());
        }

        #endregion
    }
}
