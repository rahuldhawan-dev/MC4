using MMSINC.Testing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using StructureMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels.Facilities
{
    [TestClass]
    public class EditFacilityViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Facility>
    {
        #region Fields

        private Facility _entity;
        private EditFacility _viewModel;
        private ViewModelTester<EditFacility, Facility> _vmTester;

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
            _entity = GetFactory<FacilityFactory>().Create();
            _viewModel = new EditFacility(_container);
            _vmTester = new ViewModelTester<EditFacility, Facility>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMappings()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 13);
            _vmTester.DoesNotMapToEntity(x => x.Id, 31);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationForParentFacilityWorks()
        {
            var parentFacility = GetEntityFactory<Facility>().Create(new {
                FunctionalLocation = "AABB",
                PublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(),
                PublicWaterSupplyPressureZone = GetEntityFactory<PublicWaterSupplyPressureZone>().Create()
            });

            _viewModel.FunctionalLocation = "AAAA-CCDD";
            _viewModel.ParentFacility = parentFacility.Id;
            _viewModel.OperatingCenter = parentFacility.OperatingCenter.Id;
            _viewModel.Department = parentFacility.Department.Id;
            _viewModel.PublicWaterSupply = parentFacility.PublicWaterSupply.Id;
            _viewModel.PublicWaterSupplyPressureZone = parentFacility.PublicWaterSupplyPressureZone.Id;

            _viewModel.ArcFlashStudyRequired = false;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.ParentFacility, FacilityViewModel.PARENT_FUNCTIONAL_LOCATION_MISMATCH);

            _viewModel.FunctionalLocation = parentFacility.FunctionalLocation + "-ABCD";

            ValidationAssert.ModelStateIsValid(_viewModel);
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
