using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    // This is the base ViewModel test - we only should be testing the inheriting classes
    //[TestClass]
    public class ChemicalStorageLocationTest<TViewModel> : ViewModelTestBase<ChemicalStorageLocation, TViewModel> where TViewModel : ChemicalStorageLocationViewModel
    {
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.PlanningPlant);
            _vmTester.CanMapBothWays(x => x.ChemicalWarehouseNumber);
            _vmTester.CanMapBothWays(x => x.StorageLocationNumber);
            _vmTester.CanMapBothWays(x => x.StorageLocationDescription);
            _vmTester.CanMapBothWays(x => x.IsActive);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.State);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.IsActive);
            ValidationAssert.PropertyIsRequired(x => x.StorageLocationNumber);
            ValidationAssert.PropertyIsRequired(x => x.StorageLocationDescription);

            ValidationAssert.PropertyIsNotRequired(x => x.PlanningPlant);
            ValidationAssert.PropertyIsNotRequired(x => x.ChemicalWarehouseNumber);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.StorageLocationNumber, ChemicalStorageLocation.StringLengths.STORAGE_LOCATION_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.StorageLocationDescription, ChemicalStorageLocation.StringLengths.STORAGE_LOCATION_DESCRIPTION);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.State, GetEntityFactory<State>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetFactory<UniqueOperatingCenterFactory>().Create());
            ValidationAssert.EntityMustExist(x => x.PlanningPlant, GetEntityFactory<PlanningPlant>().Create());
            ValidationAssert.EntityMustExist(x => x.ChemicalWarehouseNumber, GetEntityFactory<ChemicalWarehouseNumber>().Create());
        }
    }
}
