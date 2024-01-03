using MMSINC.Testing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using StructureMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels.Facilities
{
    [TestClass]
    public class CreateFacilityViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Facility>
    {
        #region Fields

        private CreateFacility _viewModel;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateFacility(_container);
        }

        #endregion

        #region Tests

        #region Validation

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.PublicWaterSupply,
                GetEntityFactory<PublicWaterSupply>().Create().Id,
                x => x.WasteWaterSystem,
                null,
                GetEntityFactory<WasteWaterSystem>().Create().Id,
                "Please select either a public water supply or a waste water system");

            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.WasteWaterSystem,
                GetEntityFactory<WasteWaterSystem>().Create().Id,
                x => x.PublicWaterSupply,
                null,
                GetEntityFactory<PublicWaterSupply>().Create().Id,
                "Please select either a public water supply or a waste water system");

            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.PublicWaterSupplyPressureZone,
                GetEntityFactory<PublicWaterSupplyPressureZone>().Create().Id,
                x => x.PublicWaterSupply,
                GetEntityFactory<PublicWaterSupply>().Create().Id,
                null,
                "Please select a public water supply pressure zone");

            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.WasteWaterSystemBasin,
                GetEntityFactory<WasteWaterSystemBasin>().Create().Id,
                x => x.WasteWaterSystem,
                GetEntityFactory<WasteWaterSystem>().Create().Id,
                null,
                "Please select a waste water system basin");
        }

        [TestMethod]
        public void TestVampUrlMustBeAUrl()
        {
            ValidationAssert.PropertyMustBeUrl(_viewModel, x => x.VampUrl);
        }
        #endregion

        #endregion
    }
}
