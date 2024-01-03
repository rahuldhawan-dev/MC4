using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    public abstract class BelowGroundHazardViewModelTest<TViewModel> : ViewModelTestBase<BelowGroundHazard, TViewModel> where TViewModel : BelowGroundHazardViewModel
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel.StreetNumber = 123456;
            _viewModel.HazardDescription =
                "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345679801234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678";
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.HazardDescription);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
            _vmTester.CanMapBothWays(x => x.ProximityToAmWaterAsset);
            _vmTester.CanMapBothWays(x => x.HazardApproachRecommendedType);
            _vmTester.CanMapBothWays(x => x.Town, GetEntityFactory<Town>().Create());
            _vmTester.CanMapBothWays(x => x.TownSection, GetEntityFactory<TownSection>().Create());
        }
        
        [TestMethod]
        public void TestRequiredRangeValidation()
        {
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.HazardArea, BelowGroundHazard.Ranges.AREA_LOWER, BelowGroundHazard.Ranges.AREA_UPPER);
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.DepthOfHazard, BelowGroundHazard.Ranges.DEPTH_LOWER, BelowGroundHazard.Ranges.DEPTH_UPPER);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HazardType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HazardDescription);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.StreetNumber);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.WorkOrder);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.TownSection);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.DepthOfHazard);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.ProximityToAmWaterAsset);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.HazardApproachRecommendedType);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.AssetStatus, GetEntityFactory<AssetStatus>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Town, GetEntityFactory<Town>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.HazardType, GetEntityFactory<HazardType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.HazardApproachRecommendedType, GetEntityFactory<HazardApproachRecommendedType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.TownSection, GetEntityFactory<TownSection>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            //
        }

        #endregion
    }

    [TestClass]
    public class CreateBelowGroundHazardTest : BelowGroundHazardViewModelTest<CreateBelowGroundHazard>
    {

        [TestMethod]
        public void TestCreateBelowGroundHazardSetDefaultsSetsWorkOrderValues()
        {
            var expected = "12345";
            var townSection = GetFactory<TownSectionFactory>().Create();
            var workorder = GetFactory<WorkOrderFactory>().Create(new { StreetNumber = expected, TownSection = townSection });

            var model = _viewModelFactory.Build<CreateBelowGroundHazard>();
            model.WorkOrder = workorder.Id;
            model.SetDefaults();

            var coordBelowGroundHazard = _container.GetInstance<IRepository<Coordinate>>().Find(model.Coordinate ?? 0);

            Assert.AreEqual(workorder.Coordinate.Latitude, coordBelowGroundHazard.Latitude);
            Assert.AreEqual(workorder.Coordinate.Longitude, coordBelowGroundHazard.Longitude);
            Assert.AreEqual(workorder.OperatingCenter.Id, model.OperatingCenter);
            Assert.AreEqual(workorder.NearestCrossStreet.Id, model.CrossStreet);
            Assert.AreEqual(workorder.Street.Id, model.Street);
            Assert.AreEqual(expected, model.StreetNumber.ToString());
            Assert.AreEqual(workorder.Town.Id, model.Town);
            Assert.AreEqual(workorder.TownSection.Id, model.TownSection);
            Assert.AreEqual(workorder.Id, model.WorkOrder);
        }
    }

    [TestClass]
    public class EditBelowGroundHazardTest : BelowGroundHazardViewModelTest<EditBelowGroundHazard> { }
}
