using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Linq;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class HydrantViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Hydrant>
    {
        #region Fields

        private ViewModelTester<TestHydrantViewModel, Hydrant> _vmTester;
        private TestHydrantViewModel _viewModel;
        private Hydrant _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IHydrantRepository>().Use<HydrantRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IRepository<AssetStatus>>().Use(ctx => ctx.GetInstance<IAssetStatusRepository>());
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFunctionalLocationRepository>().Use<FunctionalLocationRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IFireDistrictRepository>().Use<FireDistrictRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            _user.IsAdmin = true;

            _entity = GetEntityFactory<Hydrant>().Create();
            _viewModel = _viewModelFactory.Build<TestHydrantViewModel, Hydrant>(_entity);
            _vmTester = new ViewModelTester<TestHydrantViewModel, Hydrant>(_viewModel, _entity, _container.GetInstance<ITestDataFactoryService>());

            GetFactory<AssetStatusFactory>().CreateAll();
            GetFactory<HydrantBillingFactory>().CreateAll();

            Session.Flush();
            Session.Clear();
        }

        #endregion

        #region Mapping: Sending notifications

        [TestMethod]
        public void TestMapToEntityDoesNotSetSendNotificationOnSaveToTrueWhenANewNonPublicActiveOrRetiredHydrantIsCreated()
        {
            _viewModel.EnableSendNotificationCheck = true;
            _viewModel.HydrantBilling = GetFactory<PrivateHydrantBillingFactory>().Create().Id;
            _viewModel.Id = 0; // To make it new
            _viewModel.Status = AssetStatus.Indices.ACTIVE;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendNotificationOnSave);

            _viewModel.Status = AssetStatus.Indices.RETIRED;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotSetSendNotificationOnSaveToTrueWhenANewPublicHydrantIsCreatedForStatusesThatArentRetiredOrActive()
        {
            _viewModel.EnableSendNotificationCheck = true;
            _viewModel.HydrantBilling = HydrantBilling.Indices.PUBLIC;
            _viewModel.Id = 0; // To make it new
            _vmTester = new ViewModelTester<TestHydrantViewModel, Hydrant>(_viewModel, _entity);
            
            foreach (var invalidStatus in new[] {AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REMOVED})
            {
                _viewModel.Status = invalidStatus;
                _vmTester.MapToEntity();
                Assert.IsFalse(_viewModel.SendNotificationOnSave);
            }
        }

        [TestMethod]
        public void TestMapToEntityDoesNotSetSendNotificationOnSaveToTrueWhenPublicHydrantStatusIsSetToActiveFromActive()
        {
            _viewModel = _viewModelFactory.Build<TestHydrantViewModel, Hydrant>(_entity);
            _viewModel.HydrantBilling = HydrantBilling.Indices.PUBLIC;
            _viewModel.Status = AssetStatus.Indices.ACTIVE;
            _viewModel.EnableSendNotificationCheck = true;
            _vmTester = new ViewModelTester<TestHydrantViewModel, Hydrant>(_viewModel, _entity);

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotSetSendNotificationOnSaveToTrueWhenPublicHydrantStatusIsChangingFromSomethingNotActiveToSomethingNotActive()
        {
            var invalidStatuses = _container.GetInstance<IAssetStatusRepository>().Where(x => !new[] {
                AssetStatus.Indices.ACTIVE,
                AssetStatus.Indices.RETIRED,
                AssetStatus.Indices.REQUEST_CANCELLATION,
                AssetStatus.Indices.REQUEST_RETIREMENT
            }.Contains(x.Id) && !x.IsUserAdminOnly);

            foreach (var invalidStatus in invalidStatuses)
            {
                var entity = GetEntityFactory<Hydrant>().Create(new {Status = invalidStatus });
                _viewModel = _viewModelFactory.Build<TestHydrantViewModel, Hydrant>(entity);
                _viewModel.HydrantBilling = HydrantBilling.Indices.PUBLIC;
                _viewModel.EnableSendNotificationCheck = true;
                _vmTester = new ViewModelTester<TestHydrantViewModel, Hydrant>(_viewModel, entity);

                foreach (var otherInvalidStatus in invalidStatuses)
                {
                    _viewModel.Status = otherInvalidStatus.Id;

                    _vmTester.MapToEntity();

                    Assert.IsFalse(_viewModel.SendNotificationOnSave, $"This should be false for {otherInvalidStatus.Description}");
                }
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsSendNotificationOnSaveToTrueWhenANewPublicActiveHydrantIsCreated()
        {
            _viewModel.EnableSendNotificationCheck = true;
            _viewModel.HydrantBilling = HydrantBilling.Indices.PUBLIC;
            _viewModel.Id = 0; // To make it new
            _viewModel.Status = AssetStatus.Indices.ACTIVE;
            _vmTester = new ViewModelTester<TestHydrantViewModel, Hydrant>(_viewModel, _entity);

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendNotificationOnSaveToTrueWhenANewPublicRetiredHydrantIsCreated()
        {
            _viewModel.EnableSendNotificationCheck = true;
            _viewModel.HydrantBilling = HydrantBilling.Indices.PUBLIC;
            _viewModel.Id = 0; // To make it new
            _viewModel.Status = AssetStatus.Indices.RETIRED;
            _vmTester = new ViewModelTester<TestHydrantViewModel, Hydrant>(_viewModel, _entity);

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendNotificationOnSaveToTrueWhenPublicHydrantStatusIsActiveIsChangedFromActive()
        {
            _viewModel.EnableSendNotificationCheck = true;
            _viewModel.HydrantBilling = HydrantBilling.Indices.PUBLIC;

            _viewModel.Status = AssetStatus.Indices.CANCELLED;
            _vmTester = new ViewModelTester<TestHydrantViewModel, Hydrant>(_viewModel, _entity);

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendNotificationOnSaveToTrueWhenPublicHydrantStatusIsSetToActiveAndWasNotActive()
        {
            var invalidStatuses = _container.GetInstance<IAssetStatusRepository>().GetAll().Where(x => x.Id != AssetStatus.Indices.ACTIVE);

            foreach (var invalidStatus in invalidStatuses)
            {
                var entity = GetEntityFactory<Hydrant>().Create(new {Status = invalidStatus });
                _viewModel = _viewModelFactory.Build<TestHydrantViewModel, Hydrant>(entity);
                _viewModel.EnableSendNotificationCheck = true;
                _vmTester = new ViewModelTester<TestHydrantViewModel, Hydrant>(_viewModel, entity);
                _viewModel.HydrantBilling = HydrantBilling.Indices.PUBLIC;
                _viewModel.Status = AssetStatus.Indices.ACTIVE;

                _vmTester.MapToEntity();

                Assert.IsTrue(_viewModel.SendNotificationOnSave);
            }
        }
        
        [TestMethod]
        public void TestMapToEntitySetsSendNotificationOnSaveToTrueTheStatusIsChangedToAnyStatusThatIsNotLimitedToUserAdmins()
        {
            _viewModel.EnableSendNotificationCheck = true;
            _viewModel.Id = _entity.Id;

            var adminStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var nonAdminStatus = GetFactory<RequestRetirementAssetStatusFactory>().Create();

            _entity.Status = adminStatus;
            _viewModel.Status = nonAdminStatus.Id;

            _vmTester.MapToEntity();
        }

        #endregion

        #region Mapping: Both Ways

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.IsNonBPUKPI);
            _vmTester.CanMapBothWays(x => x.Critical);
            _vmTester.CanMapBothWays(x => x.CriticalNotes);
            _vmTester.CanMapBothWays(x => x.DateInstalled);
            _vmTester.CanMapBothWays(x => x.IsDeadEndMain);
            _vmTester.CanMapBothWays(x => x.Elevation);
            _vmTester.CanMapBothWays(x => x.InspectionFrequency);
            _vmTester.CanMapBothWays(x => x.InspectionFrequencyUnit);
            _vmTester.CanMapBothWays(x => x.PaintingFrequency);
            _vmTester.CanMapBothWays(x => x.PaintingFrequencyUnit);
            _vmTester.CanMapBothWays(x => x.PaintingZone);
            _vmTester.CanMapBothWays(x => x.Location);
            _vmTester.CanMapBothWays(x => x.MapPage);
            _vmTester.CanMapBothWays(x => x.Route);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.ValveLocation);
            _vmTester.CanMapBothWays(x => x.YearManufactured);
            _vmTester.CanMapBothWays(x => x.ClowTagged);
            _vmTester.CanMapBothWays(x => x.BillingDate);
            _vmTester.CanMapBothWays(x => x.BranchLengthFeet);
            _vmTester.CanMapBothWays(x => x.BranchLengthInches);
            _vmTester.CanMapBothWays(x => x.DepthBuryFeet);
            _vmTester.CanMapBothWays(x => x.DepthBuryInches);
            _vmTester.CanMapBothWays(x => x.GISUID);
            _vmTester.CanMapBothWays(x => x.HydrantOutletConfiguration);
            _vmTester.CanMapBothWays(x => x.HydrantType);
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
        public void TestLateralValveCanMapBothWays()
        {
            var valve = GetEntityFactory<Valve>().Create(new {Description = "Foo"});
            _entity.LateralValve = valve;

            _vmTester.MapToViewModel();

            Assert.AreEqual(valve.Id, _viewModel.LateralValve);

            _entity.LateralValve = null;
            _vmTester.MapToEntity();

            Assert.IsNotNull(_entity.LateralValve);
            Assert.AreSame(valve, _entity.LateralValve);
        }

        [TestMethod]
        public void TestFireDistrictCanMapBothWays()
        {
            var fireDistrict = GetEntityFactory<FireDistrict>().Create(new {DistrictName = "Foo"});
            _entity.FireDistrict = fireDistrict;

            _vmTester.MapToViewModel();

            Assert.AreEqual(fireDistrict.Id, _viewModel.FireDistrict);

            _entity.FireDistrict = null;
            _vmTester.MapToEntity();

            Assert.AreSame(fireDistrict, _entity.FireDistrict);
        }

        [TestMethod]
        public void TestFunctionalLocationCanMapBothWays()
        {
            var funcLoc = GetEntityFactory<FunctionalLocation>().Create();
            _entity.FunctionalLocation = funcLoc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(funcLoc.Id, _viewModel.FunctionalLocation);

            _entity.FunctionalLocation = null;
            _vmTester.MapToEntity();

            Assert.AreSame(funcLoc, _entity.FunctionalLocation);
        }

        [TestMethod]
        public void TestHydrantTagStatusCanMapBothWays()
        {
            var hydrantTagStatus = GetEntityFactory<HydrantTagStatus>().Create(new {Description = "Foo"});
            _entity.HydrantTagStatus = hydrantTagStatus;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantTagStatus.Id, _viewModel.HydrantTagStatus);

            _entity.HydrantTagStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantTagStatus, _entity.HydrantTagStatus);
        }

        [TestMethod]
        public void TestHydrantManufacturerCanMapBothWays()
        {
            var hydrantManufacture = GetEntityFactory<HydrantManufacturer>().Create(new {Description = "Foo"});
            _entity.HydrantManufacturer = hydrantManufacture;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantManufacture.Id, _viewModel.HydrantManufacturer);

            _entity.HydrantManufacturer = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantManufacture, _entity.HydrantManufacturer);
        }

        [TestMethod]
        public void TestHydrantModelCanMapBothWays()
        {
            var hydrantManufacturer = GetEntityFactory<HydrantManufacturer>().Create(new { Description = "Bar" });
            var hydrantModel = GetEntityFactory<HydrantModel>().Create(new {Description = "Foo", HydrantManufacturer = hydrantManufacturer});
            _entity.HydrantModel = hydrantModel;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantModel.Id, _viewModel.HydrantModel);

            _entity.HydrantModel = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantModel, _entity.HydrantModel);
        }

        [TestMethod]
        public void TestHydrantStatusCanMapBothWays()
        {
            var hydrantStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _entity.Status = hydrantStatus;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantStatus.Id, _viewModel.Status);

            _entity.Status = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantStatus, _entity.Status);
        }

        [TestMethod]
        public void TestLateralSizeCanMapBothWays()
        {
            var lateralSize = GetEntityFactory<LateralSize>().Create(new {Description = "Foo"});
            _entity.LateralSize = lateralSize;

            _vmTester.MapToViewModel();

            Assert.AreEqual(lateralSize.Id, _viewModel.LateralSize);

            _entity.LateralSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(lateralSize, _entity.LateralSize);
        }

        [TestMethod]
        public void TestOpenDirectionCanMapBothWays()
        {
            var opensDirection = GetEntityFactory<HydrantDirection>().Create(new {Description = "Foo"});
            _entity.OpenDirection = opensDirection;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opensDirection.Id, _viewModel.OpenDirection);

            _entity.OpenDirection = null;
            _vmTester.MapToEntity();

            Assert.AreSame(opensDirection, _entity.OpenDirection);
        }

        [TestMethod]
        public void TestGradientCanMapBothWays()
        {
            var gradient = GetEntityFactory<Gradient>().Create(new {Description = "Foo"});
            _entity.Gradient = gradient;

            _vmTester.MapToViewModel();

            Assert.AreEqual(gradient.Id, _viewModel.Gradient);

            _entity.Gradient = null;
            _vmTester.MapToEntity();

            Assert.AreSame(gradient, _entity.Gradient);
        }

        [TestMethod]
        public void TestHydrantSizeCanMapBothWays()
        {
            var hydrantSize = GetEntityFactory<HydrantSize>().Create(new {Description = "Foo"});
            _entity.HydrantSize = hydrantSize;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantSize.Id, _viewModel.HydrantSize);

            _entity.HydrantSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantSize, _entity.HydrantSize);
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
        public void TestPaintingFrequencyUnitCanMapBothWays()
        {
            var inspFreqUnit = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _entity.PaintingFrequencyUnit = inspFreqUnit;

            _vmTester.MapToViewModel();

            Assert.AreEqual(inspFreqUnit.Id, _viewModel.PaintingFrequencyUnit);

            _entity.PaintingFrequencyUnit = null;
            _vmTester.MapToEntity();

            Assert.AreSame(inspFreqUnit, _entity.PaintingFrequencyUnit);
        }

        [TestMethod]
        public void TestHydrantMainSizeCanMapBothWays()
        {
            var hydrantMainSize = GetEntityFactory<HydrantMainSize>().Create(new {Description = "Foo"});
            _entity.HydrantMainSize = hydrantMainSize;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantMainSize.Id, _viewModel.HydrantMainSize);

            _entity.HydrantMainSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantMainSize, _entity.HydrantMainSize);
        }

        [TestMethod]
        public void TestHydrantThreadTypeCanMapBothWays()
        {
            var hydrantThreadType = GetEntityFactory<HydrantThreadType>().Create(new {Description = "Foo"});
            _entity.HydrantThreadType = hydrantThreadType;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantThreadType.Id, _viewModel.HydrantThreadType);

            _entity.HydrantThreadType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantThreadType, _entity.HydrantThreadType);
        }

        [TestMethod]
        public void TestTownSectionCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create();
            var townSection = GetEntityFactory<TownSection>().Create(new {Name = "Foo", Town = town});
            _entity.TownSection = townSection;

            _vmTester.MapToViewModel();

            Assert.AreEqual(townSection.Id, _viewModel.TownSection);

            _entity.TownSection = null;
            _vmTester.MapToEntity();

            Assert.AreSame(townSection, _entity.TownSection);
        }

        [TestMethod]
        public void TestHydrantBillingCanMapBothWays()
        {
            var hydrantBilling = GetFactory<PrivateHydrantBillingFactory>().Create();
            _entity.HydrantBilling = hydrantBilling;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantBilling.Id, _viewModel.HydrantBilling);

            _entity.HydrantBilling = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantBilling, _entity.HydrantBilling);
        }

        [TestMethod]
        public void TestWaterSystemCanMapBothWays()
        {
            var ws = GetEntityFactory<WaterSystem>().Create(new{ Description = "Why don't I have a factory?" });
            _entity.WaterSystem = ws;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ws.Id, _viewModel.WaterSystem);

            _entity.WaterSystem = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ws, _entity.WaterSystem);
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
            var statuses = new[] { AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REMOVED, AssetStatus.Indices.REMOVED };

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = _container.GetInstance<IRepository<AssetStatus>>().Find(status);
                _viewModel.Status = status;

                _vmTester.MapToEntity();

                Assert.IsFalse(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPToTrueIfTheExistingStatusWasCancelledAndTheStatusIsChanged()
        {
            // This same test should work for Cancelled, Retired, and Removed.
            var statuses = new[] { AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REMOVED, AssetStatus.Indices.REMOVED };

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = _container.GetInstance<IRepository<AssetStatus>>().Find(status);
                _viewModel.Status = AssetStatus.Indices.ACTIVE;

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

        [TestMethod]
        public void MapToEntitySetsBackInServiceFieldsWhenHydrantIsOutOfServiceAndRetiredStatus()
        {
            var expectedDate = DateTime.Now;
            var expectedUser = GetFactory<UserFactory>().Create();
            var oos = GetFactory<HydrantOutOfServiceFactory>().Create(new { Hydrant = _entity });

            _authServ.Setup(x => x.CurrentUser).Returns(expectedUser);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _entity.SetPropertyValueByName("OutOfService", true);
            foreach (var assetStatusId in AssetStatus.RETIRED_STATUS_IDS)
            {
                _viewModel.Status = assetStatusId;
                _vmTester.MapToEntity();

                Assert.AreEqual(expectedDate, oos.BackInServiceDate);
                Assert.AreSame(expectedUser, oos.BackInServiceByUser);
            }
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestSimpleRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HydrantBilling);
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
                    "DateRetired is required for retired / removed hydrants.");
            }
        }

        
        [TestMethod]
        public void TestCriticalNotesIsRequiredIfCriticalIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.CriticalNotes, "Stuff", x => x.Critical, true, false);
        }

        [TestMethod]
        public void TestCriticalNotesMustBeNullIfCriticalIsFalse()
        {
            _viewModel.Critical = false;
            _viewModel.CriticalNotes = "blah";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.CriticalNotes, "Critical checkbox must be checked when setting critical notes.");

            _viewModel.CriticalNotes = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.CriticalNotes);
        }

        [TestMethod]
        public void TestCoordinateIsRequiredForActiveHydrants()
        {
            var cord = GetEntityFactory<Coordinate>().Create();

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.Coordinate, cord.Id, x => x.Status, AssetStatus.Indices.ACTIVE, AssetStatus.Indices.RETIRED, "Coordinate is required for active hydrants.");
        }

        [TestMethod]
        public void TestBillingDateIsRequiredForACTIVE_AND_PUBLIC_hydrants()
        {
            _viewModel.HydrantBilling = HydrantBilling.Indices.PUBLIC;
            _viewModel.Status = AssetStatus.Indices.ACTIVE;
            _viewModel.BillingDate = null;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.BillingDate, "BillingDate is required for public and active hydrants.");

            _viewModel.HydrantBilling = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.BillingDate);

            _viewModel.HydrantBilling = HydrantBilling.Indices.PUBLIC;
            _viewModel.Status = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.BillingDate);
        }

        [TestMethod]
        public void TestMainSizeIsRequiredForActiveAndInstalledHydrants()
        {
            _viewModel.Status = null;
            _viewModel.HydrantMainSize = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HydrantMainSize);

            _viewModel.Status = AssetStatus.Indices.ACTIVE;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HydrantMainSize, "Main Size is required for active / installed hydrants.");

            var hydrantMainSize = GetEntityFactory<HydrantMainSize>().Create(new { Description = "Foo" });
            _viewModel.HydrantMainSize = hydrantMainSize.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HydrantMainSize);

            _viewModel.Status = AssetStatus.Indices.INSTALLED;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HydrantMainSize);
        }

        [TestMethod]
        public void TestDateInstalledIsRequiredIfHydrantIsActive()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.DateInstalled, DateTime.Now, x => x.Status, AssetStatus.Indices.ACTIVE, AssetStatus.Indices.RETIRED, "DateInstalled is required for active hydrants.");
        }

        [TestMethod]
        public void TestStringLengths()
        {
            _viewModel.Critical = true; // Needs to be true for testing CriticalNotes.
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CriticalNotes, Hydrant.StringLengths.CRITICAL_NOTES);
            //ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Elevation, Hydrant.StringLengths.ELEVATION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Location, Hydrant.StringLengths.LOCATION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MapPage, Hydrant.StringLengths.MAP_PAGE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.StreetNumber, Hydrant.StringLengths.STREET_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ValveLocation, Hydrant.StringLengths.VALVE_LOCATION);
        }
        
        #endregion

        #region Test classes

        private class TestHydrantViewModel : HydrantViewModel
        {
            public TestHydrantViewModel(IContainer container) : base(container)
            {
                EnableSendNotificationCheck = false;
            }

            /// <summary>
            /// This property is misleading. If you forget to set it when you want it
            /// then your test can pass without you knowing why. 
            /// </summary>
            public bool EnableSendNotificationCheck { get; set; }

            protected override void SetSendNotificationsOnSave(Hydrant entity)
            {
                if (EnableSendNotificationCheck)
                {
                    base.SetSendNotificationsOnSave(entity);
                }
            }
        }

        #endregion
    }
}
