using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class ValveViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Valve>
    {
        #region Fields

        private ViewModelTester<ValveViewModel, Valve> _vmTester;
        private ValveViewModel _viewModel;
        private Valve _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private AssetStatus _activeStatus;
        private AssetStatus _retiredStatus;
        private AssetStatus _removedStatus;
        private AssetStatus _cancelledStatus;
        private AssetStatus _installedAssetStatus;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _entity = GetEntityFactory<Valve>().Create();
            _viewModel = _viewModelFactory.Build<ValveViewModel, Valve>(_entity);
            _vmTester = new ViewModelTester<ValveViewModel, Valve>(_viewModel, _entity);

            // An OperatingCenter is needed for the mapping tests
            _viewModel.OperatingCenter = GetFactory<OperatingCenterFactory>().Create().Id;

            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            _installedAssetStatus = GetFactory<InstalledAssetStatusFactory>().Create();
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.BPUKPI);
            _vmTester.CanMapBothWays(x => x.DateTested);
            _vmTester.CanMapBothWays(x => x.Elevation);
            _vmTester.CanMapBothWays(x => x.InspectionFrequency);
            _vmTester.CanMapBothWays(x => x.MapPage);
            _vmTester.CanMapBothWays(x => x.Route);
            _vmTester.CanMapBothWays(x => x.SketchNumber);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.Traffic);
            _vmTester.CanMapBothWays(x => x.ValveLocation);
            _vmTester.CanMapBothWays(x => x.DateInstalled);
            _vmTester.CanMapBothWays(x => x.Turns);
            _vmTester.CanMapBothWays(x => x.GISUID);
        }

        [TestMethod]
        public void TestValveBillingCanMapBothWays()
        {
            var valveBilling = GetFactory<PublicValveBillingFactory>().Create();
            _entity.ValveBilling = valveBilling;

            _vmTester.MapToViewModel();

            Assert.AreEqual(valveBilling.Id, _viewModel.ValveBilling);

            _entity.ValveBilling = null;
            _vmTester.MapToEntity();

            Assert.AreSame(valveBilling, _entity.ValveBilling);
        }

        [TestMethod]
        public void TestInspectionFrequencyUnitCanMapBothWays()
        {
            var inspFreqUnit = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _entity.InspectionFrequencyUnit = inspFreqUnit;

            _vmTester.MapToViewModel();

            Assert.AreEqual(inspFreqUnit.Id, _viewModel.InspectionFrequencyUnit);

            _entity.InspectionFrequencyUnit = null;
            _vmTester.MapToEntity();

            Assert.AreSame(inspFreqUnit, _entity.InspectionFrequencyUnit);
        }

        [TestMethod]
        public void TestNormalPositionCanMapBothWays()
        {
            var normalPosition = GetEntityFactory<ValveNormalPosition>().Create(new {Description = "Foo"});
            _entity.NormalPosition = normalPosition;

            _vmTester.MapToViewModel();

            Assert.AreEqual(normalPosition.Id, _viewModel.NormalPosition);

            _entity.NormalPosition = null;
            _vmTester.MapToEntity();

            Assert.AreSame(normalPosition, _entity.NormalPosition);
        }

        [TestMethod]
        public void TestValveOpenDirectionCanMapBothWays()
        {
            var valveOpenDirection = GetEntityFactory<ValveOpenDirection>().Create(new { Description = "Foo" });
            _entity.OpenDirection = valveOpenDirection;

            _vmTester.MapToViewModel();

            Assert.AreEqual(valveOpenDirection.Id, _viewModel.OpenDirection);

            _entity.OpenDirection = null;
            _vmTester.MapToEntity();

            Assert.AreSame(valveOpenDirection, _entity.OpenDirection);
        }

        [TestMethod]
        public void TestTownSectionCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create();
            var townSection = GetEntityFactory<TownSection>().Create(new { Name = "Foo", Town = town });
            _entity.TownSection = townSection;

            _vmTester.MapToViewModel();

            Assert.AreEqual(townSection.Id, _viewModel.TownSection);

            _entity.TownSection = null;
            _vmTester.MapToEntity();

            Assert.AreSame(townSection, _entity.TownSection);
        }

        [TestMethod]
        public void TestMainTypeCanMapBothWays()
        {
            var mainType = GetEntityFactory<MainType>().Create(new {Description = "Foo"});
            _entity.MainType = mainType;

            _vmTester.MapToViewModel();

            Assert.AreEqual(mainType.Id, _viewModel.MainType);

            _entity.MainType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(mainType, _entity.MainType);
        }

        [TestMethod]
        public void TestValveControlsCanMapBothWays()
        {
            var valveControl = GetFactory<SomeValveControlFactory>().Create();
            _entity.ValveControls = valveControl;

            _vmTester.MapToViewModel();

            Assert.AreEqual(valveControl.Id, _viewModel.ValveControls);

            _entity.ValveControls = null;
            _vmTester.MapToEntity();

            Assert.AreSame(valveControl, _entity.ValveControls);
        }

        [TestMethod]
        public void TestValveMakeCanMapBothWays()
        {
            var valveMake = GetEntityFactory<ValveManufacturer>().Create(new {Description = "Foo"});
            _entity.ValveMake = valveMake;

            _vmTester.MapToViewModel();

            Assert.AreEqual(valveMake.Id, _viewModel.ValveMake);

            _entity.ValveMake = null;
            _vmTester.MapToEntity();

            Assert.AreSame(valveMake, _entity.ValveMake);
        }

        [TestMethod]
        public void TestValveTypeCanMapBothWays()
        {
            var valveType = GetEntityFactory<ValveType>().Create(new {Description = "Foo"});
            _entity.ValveType = valveType;

            _vmTester.MapToViewModel();

            Assert.AreEqual(valveType.Id, _viewModel.ValveType);

            _entity.ValveType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(valveType, _entity.ValveType);
        }

        [TestMethod]
        public void TestValveSizeCanMapBothWays()
        {
            var valveSize = GetEntityFactory<ValveSize>().Create(new {Description = "Foo"});
            _entity.ValveSize = valveSize;

            _vmTester.MapToViewModel();

            Assert.AreEqual(valveSize.Id, _viewModel.ValveSize);

            _entity.ValveSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(valveSize, _entity.ValveSize);
        }
        
        [TestMethod]
        public void TestValveStatusCanMapBothWays()
        {
            var valveStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _entity.Status = valveStatus;

            _vmTester.MapToViewModel();

            Assert.AreEqual(valveStatus.Id, _viewModel.Status);

            _entity.Status = null;
            _vmTester.MapToEntity();

            Assert.AreSame(valveStatus, _entity.Status);
        }
        
        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var c = GetFactory<CoordinateFactory>().Create();
            _entity.Coordinate = c;
            _vmTester.MapToViewModel();
            Assert.AreEqual(c.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();
            Assert.AreSame(c, _entity.Coordinate);
        }

        [TestMethod]
        public void TestValveZoneCanMapBothWays()
        {
            var valveZone = GetEntityFactory<ValveZone>().Create(new {Description = "Foo"});
            _entity.ValveZone = valveZone;

            _vmTester.MapToViewModel();

            Assert.AreEqual(valveZone.Id, _viewModel.ValveZone);

            _entity.ValveZone = null;
            _vmTester.MapToEntity();

            Assert.AreSame(valveZone, _entity.ValveZone);
        }

        [TestMethod]
        public void TestWaterSystemCanMapBothWays()
        {
            var waterSystem = GetEntityFactory<WaterSystem>().Create(new {Description = "Foo"});
            _entity.WaterSystem = waterSystem;

            _vmTester.MapToViewModel();

            Assert.AreEqual(waterSystem.Id, _viewModel.WaterSystem);

            _entity.WaterSystem = null;
            _vmTester.MapToEntity();

            Assert.AreSame(waterSystem, _entity.WaterSystem);
        }
        
        [TestMethod]
        public void TestFunctionalLocationCanMapBothWays()
        {
            var functionalLocation = GetEntityFactory<FunctionalLocation>().Create(new {Description = "Foo"});
            _entity.FunctionalLocation = functionalLocation;

            _vmTester.MapToViewModel();

            Assert.AreEqual(functionalLocation.Id, _viewModel.FunctionalLocation);

            _entity.FunctionalLocation = null;
            _vmTester.MapToEntity();

            Assert.AreSame(functionalLocation, _entity.FunctionalLocation);
        }

        [TestMethod]
        public void TestFacilityCanMapBothWays()
        {
            var facility = GetEntityFactory<Facility>().Create(new {Description = "Foo"});
            _entity.Facility = facility;

            _vmTester.MapToViewModel();

            Assert.AreEqual(facility.Id, _viewModel.Facility);

            _entity.Facility = null;
            _vmTester.MapToEntity();

            Assert.AreSame(facility, _entity.Facility);
        }


        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPTrueWhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledTrueAndContractedOps()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPToFalseIfTheExistingStatusIsCancelledAndTheStatusItNotBeingChanged()
        {
            // This same test should work for Cancelled, Retired, and Removed.
            var statuses = new[] { _cancelledStatus, _retiredStatus, _removedStatus };

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = status;
                _viewModel.Status = status.Id;

                _vmTester.MapToEntity();

                Assert.IsFalse(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPToTrueIfTheExistingStatusWasCancelledAndTheStatusIsChanged()
        {
            // This same test should work for Cancelled, Retired, and Removed.
            var statuses = new[] { _cancelledStatus, _retiredStatus, _removedStatus };

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = status;
                _viewModel.Status = _activeStatus.Id;

                _vmTester.MapToEntity();

                Assert.IsTrue(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestRetiredDateCanMapBothWaysWhenAssetStatusIsNotRetired()
        {
            var expectedDate = DateTime.Now;

            foreach (var assetStatusId in AssetStatus.ALL_STATUS_IDS.Except(AssetStatus.RETIRED_STATUS_IDS))
            {
                _entity.DateRetired = expectedDate;

                _vmTester.MapToViewModel();

                Assert.AreEqual(expectedDate, _viewModel.DateRetired);

                _viewModel.Status = assetStatusId;

                _vmTester.MapToEntity();

                Assert.IsNull(_entity.DateRetired, $"Assertion failed for status id: {assetStatusId}. Expected null, got: {_entity.DateRetired}");
            }
        }

        [TestMethod]
        public void TestRetiredDateCanMapBothWaysWhenAssetStatusIsRetired()
        {
            var expectedDate = DateTime.Now;

            foreach (var assetStatusId in AssetStatus.RETIRED_STATUS_IDS)
            {
                _entity.DateRetired = expectedDate;

                _vmTester.MapToViewModel();

                Assert.AreEqual(expectedDate, _viewModel.DateRetired);

                _viewModel.Status = assetStatusId;

                _vmTester.MapToEntity();

                Assert.AreEqual(expectedDate, _entity.DateRetired, $"Assertion failed for status id: {assetStatusId}. Expected {expectedDate}, got: {_entity.DateRetired}");
            }
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ValveZone);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ValveSize);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Status);
        }

        [TestMethod]
        public void TestDateRetiredIsRequiredWhenAssetStatusIsRetired()
        {
            foreach (var assetStatusId in AssetStatus.RETIRED_STATUS_IDS)
            {
                ValidationAssert.PropertyIsRequiredWhen(
                    _viewModel, 
                    x => x.DateRetired, 
                    DateTime.Now, 
                    x => x.Status, 
                    assetStatusId, 
                    AssetStatus.Indices.ACTIVE, 
                    "DateRetired is required for retired / removed valves.");
            }
        }

        [TestMethod]
        public void TestStringLengths()
        {
            //ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ValveNumber, Valve.StringLengths.VALVE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ValveLocation, Valve.StringLengths.VALVE_LOCATION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MapPage, Valve.StringLengths.MAP_PAGE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SketchNumber, Valve.StringLengths.SKETCH_NUMBER, false, "The field Sketch # must be a string with a maximum length of 15.");
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.StreetNumber, Valve.StringLengths.STREET_NUMBER);
        }

        [TestMethod]
        public void TestCoordinateIsRequiredForActiveValves()
        {
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var cord = GetEntityFactory<Coordinate>().Create();
 
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.Coordinate, cord.Id, x => x.Status, activeStatus.Id, retiredStatus.Id, "Coordinate is required for active valves.");
        }

        [TestMethod]
        public void TestInstalledWithTypesRequiresTurns()
        {
            var types = new List<ValveType> {
                GetFactory<ValveTypeBallFactory>().Create(),
                GetFactory<ValveTypeButterflyFactory>().Create(),
                GetFactory<ValveTypeCheckFactory>().Create(),
                GetFactory<ValveTypeGateFactory>().Create(),
                GetFactory<ValveTypeTappingFactory>().Create()
            };
            _viewModel.Status = AssetStatus.Indices.ACTIVE;

            foreach (var type in types)
            {
                _viewModel.ValveType = type.Id;
                _viewModel.ValveSize = 4;
                if (ValveType.VALVE_TYPES_REQUIRING_TURNS.Contains(type.Id))
                {
                    ValidationAssert.ModelStateHasError(_viewModel, x => x.Turns, ValveViewModel.ErrorMessages.ERROR_TURNS_REQUIRED);
                }
                else
                {
                    ValidationAssert.ModelStateIsValid(_viewModel, x => x.Turns);
                }

            }
        }

        [TestMethod]
        public void TestActiveWithTypesRequiresTurns()
        {
            var types = new List<ValveType> {
                GetFactory<ValveTypeBallFactory>().Create(),
                GetFactory<ValveTypeButterflyFactory>().Create(),
                GetFactory<ValveTypeCheckFactory>().Create(),
                GetFactory<ValveTypeGateFactory>().Create(),
                GetFactory<ValveTypeTappingFactory>().Create()
            };
            _viewModel.Status = AssetStatus.Indices.ACTIVE;
            
            foreach (var type in types)
            {
                _viewModel.ValveType = type.Id;
                _viewModel.ValveSize = 4;
                if (ValveType.VALVE_TYPES_REQUIRING_TURNS.Contains(type.Id))
                {
                    ValidationAssert.ModelStateHasError(_viewModel, x => x.Turns, ValveViewModel.ErrorMessages.ERROR_TURNS_REQUIRED);
                }
                else
                {
                    ValidationAssert.ModelStateIsValid(_viewModel, x => x.Turns);
                }

            }
        }

        [TestMethod]
        public void TestValveTypeIsRequiredForActiveAndInstalledValves()
        {
            _viewModel.Status = null;
            _viewModel.ValveType = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ValveType);
            
            _viewModel.Status = _activeStatus.Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ValveType, "Valve Type is required for active / installed valves.") ;

            var valveType = GetEntityFactory<ValveType>().Create();
            _viewModel.ValveType = valveType.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ValveType);

            _viewModel.Status = _installedAssetStatus.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ValveType);
        }

        [TestMethod]
        public void TestNormalPositionIsRequiredForActiveAndInstalledValves()
        {
            _viewModel.Status = null;
            _viewModel.NormalPosition = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.NormalPosition);
            
            _viewModel.Status = _activeStatus.Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.NormalPosition, "Normal Position is required for active / installed valves.");

            var normalPosition = GetEntityFactory<ValveNormalPosition>().Create();
            _viewModel.NormalPosition = normalPosition.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.NormalPosition);

            _viewModel.Status = _installedAssetStatus.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.NormalPosition);
        }
        #endregion
    }
}
