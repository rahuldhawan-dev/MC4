using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    public abstract class ChemicalStorageTestBase<TViewModel> : ViewModelTestBase<ChemicalStorage, TViewModel> where TViewModel : ChemicalStorageViewModel
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.MaxStorageQuantityGallons);
            _vmTester.CanMapBothWays(x => x.MinStorageQuantityGallons);
            _vmTester.CanMapBothWays(x => x.MaxStorageQuantityPounds);
            _vmTester.CanMapBothWays(x => x.MinStorageQuantityPounds);
            _vmTester.CanMapBothWays(x => x.ReorderLevelNonPeakProductionGallons);
            _vmTester.CanMapBothWays(x => x.ReorderLevelPeakProductionGallons);
            _vmTester.CanMapBothWays(x => x.ReorderLevelNonPeakProductionPounds);
            _vmTester.CanMapBothWays(x => x.ReorderLevelPeakProductionPounds);
            _vmTester.CanMapBothWays(x => x.TypicalOrderQuantityGallons);
            _vmTester.CanMapBothWays(x => x.TypicalOrderQuantityPounds);
            _vmTester.CanMapBothWays(x => x.Crtk);
            _vmTester.CanMapBothWays(x => x.DeliveryInstructions);
            _vmTester.CanMapBothWays(x => x.Location);
            _vmTester.CanMapBothWays(x => x.ContainerType);
            _vmTester.CanMapBothWays(x => x.MaximumDailyInventory);
            _vmTester.CanMapBothWays(x => x.AverageDailyInventory);
            _vmTester.CanMapBothWays(x => x.DaysOnSite);
            _vmTester.CanMapBothWays(x => x.StoragePressure);
            _vmTester.CanMapBothWays(x => x.StorageTemperature);
            _vmTester.CanMapBothWays(x => x.IsActive);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Chemical);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Location, ChemicalStorage.StringLengths.LOCATION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ContainerType, ChemicalStorage.StringLengths.CONTAINER_TYPE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MaximumDailyInventory, ChemicalStorage.StringLengths.MAXIMUM_DAILY_INVENTORY);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.AverageDailyInventory, ChemicalStorage.StringLengths.AVERAGE_DAILY_INVENTORY);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.StoragePressure, ChemicalStorage.StringLengths.STORAGE_PRESSURE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.StorageTemperature, ChemicalStorage.StringLengths.STORAGE_TEMPERATURE);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Chemical, GetEntityFactory<Chemical>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Facility, GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.WarehouseNumber, GetEntityFactory<ChemicalWarehouseNumber>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetFactory<UniqueOperatingCenterFactory>().Create());
        }

        [TestMethod]
        public void TestMapToEntitySetsFacilitiesChemicalFeedToTrueWhenChemicalStored()
        {
            var facility = GetEntityFactory<Facility>().Create();
            facility.ChemicalFeed = false;
            var chemical = GetEntityFactory<Chemical>().Create();
            GetEntityFactory<ChemicalStorage>().Create(new { Chemical = chemical, Facility = facility });
            _vmTester.MapToEntity();
            Assert.IsTrue(_entity.Facility.ChemicalFeed);
        }

        [TestMethod]
        public void TestMapSetsOperatingCenterFromWarehouse()
        {
            _entity = GetEntityFactory<ChemicalStorage>().Create(new
            {
                WarehouseNumber = GetEntityFactory<ChemicalWarehouseNumber>().Create(new
                {
                    OperatingCenter = GetEntityFactory<OperatingCenter>().Create()
                })
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(_entity.WarehouseNumber.OperatingCenter.Id, _viewModel.OperatingCenter);
        }

        [TestMethod]
        public void TestMapSetsStateFromWarehouse()
        {
            _entity = GetEntityFactory<ChemicalStorage>().Create(new
            {
                WarehouseNumber = GetEntityFactory<ChemicalWarehouseNumber>().Create(new
                {
                    OperatingCenter = GetEntityFactory<OperatingCenter>().Create(new
                    {
                        State = GetEntityFactory<State>().Create()
                    })
                })
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(_entity.WarehouseNumber.OperatingCenter.State.Id, _viewModel.State);
        }

        [TestMethod]
        public void TestMapSetsOperatingCenterFromWarehouseNumberOperatingCenter()
        {
            _entity = GetEntityFactory<ChemicalStorage>().Create(new {
                WarehouseNumber = GetEntityFactory<ChemicalWarehouseNumber>().Create(new {
                    OperatingCenter = GetEntityFactory<OperatingCenter>().Create()
                })
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(_entity.WarehouseNumber.OperatingCenter.Id, _viewModel.OperatingCenter);
        }

        [TestMethod]
        public void TestMapSetsStateFromWarehouseNumberOperatingtCenterState()
        {
            _entity = GetEntityFactory<ChemicalStorage>().Create(new {
                WarehouseNumber = GetEntityFactory<ChemicalWarehouseNumber>().Create(new {
                    OperatingCenter = GetEntityFactory<OperatingCenter>().Create(new {
                        State = GetEntityFactory<State>().Create()
                    })
                })
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(_entity.WarehouseNumber.OperatingCenter.State.Id, _viewModel.State);
        }

        #endregion
    }

    [TestClass]
    public class CreateChemicalStorageTest : ChemicalStorageTestBase<CreateChemicalStorage> {}

    [TestClass]
    public class EditChemicalStorageTest : ChemicalStorageTestBase<EditChemicalStorage> {}
}
