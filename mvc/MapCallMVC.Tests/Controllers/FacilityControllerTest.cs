using System;
using System.Collections.Generic;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Helpers;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class FacilityControllerTest : MapCallMvcControllerTestBase<FacilityController, Facility>
    {
        #region Private Members

        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFunctionalLocationRepository>().Use<FunctionalLocationRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.DestroyRedirectsToRouteOnSuccessArgs = (id) => new { controller = "Facility", action = "Index" };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateFacility)vm;
                model.PublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create().Id;
                model.PublicWaterSupplyPressureZone = GetEntityFactory<PublicWaterSupplyPressureZone>().Create().Id;
            };
        }

        protected override User CreateUser()
        {
            var user = base.CreateUser();
            user.IsAdmin = true;
            return user;
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.ProductionFacilities;
                var facilityAreaManagementModule = RoleModules.ProductionFacilityAreaManagement;
                a.RequiresRole("~/Facility/Search", module, RoleActions.Read);
                a.RequiresRole("~/Facility/Index", module, RoleActions.Read);
                a.RequiresRole("~/Facility/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/Facility/Update", module, RoleActions.Edit);
                a.RequiresRole("~/Facility/New", module, RoleActions.Add);
                a.RequiresRole("~/Facility/Create", module, RoleActions.Add);
                a.RequiresRole("~/Facility/Destroy", module, RoleActions.Delete);
                a.RequiresRole("~/Facility/Readings", module, RoleActions.Read);
                a.RequiresRole("~/Facility/AddFacilityProcess", module, RoleActions.Edit);
                a.RequiresRole("~/Facility/RemoveFacilityProcess", module, RoleActions.Edit);
                a.RequiresRole("~/Facility/AddSystemDeliveryEntryType", RoleModules.ProductionSystemDeliveryConfiguration, RoleActions.Add);
                a.RequiresRole("~/Facility/RemoveSystemDeliveryEntryType", RoleModules.ProductionSystemDeliveryConfiguration, RoleActions.Delete);
                a.RequiresRole("~/Facility/AddFacilityFacilityArea/", facilityAreaManagementModule, RoleActions.Edit);
                a.RequiresRole("~/Facility/RemoveFacilityFacilityArea/", facilityAreaManagementModule, RoleActions.Delete);
                a.RequiresRole("~/Facility/Show", module);
                a.RequiresRole("~/Facility/GetSystemDeliveryHistoryForFacility/", module);

                a.RequiresLoggedInUserOnly("~/Facility/ByEquipment/");
                a.RequiresLoggedInUserOnly("~/Facility/ByTownId");
                a.RequiresLoggedInUserOnly("~/Facility/ByPlanningPlant");
                a.RequiresLoggedInUserOnly("~/Facility/ByPlanningPlants");
                a.RequiresLoggedInUserOnly("~/Facility/ByFunctionalLocation/");
                a.RequiresLoggedInUserOnly("~/Facility/ByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/Facility/ByOperatingCenterIds/");
                a.RequiresLoggedInUserOnly("~/Facility/GetByPublicWaterSupplyId/");
                a.RequiresLoggedInUserOnly("~/Facility/ActiveByOperatingCenterOrPlanningPlant/");
                a.RequiresLoggedInUserOnly("~/Facility/ActiveAndPendingRetirementByOperatingCenterOrPlanningPlant/");
                a.RequiresLoggedInUserOnly("~/Facility/ActiveByOperatingCentersOrPlanningPlant/");
                a.RequiresLoggedInUserOnly("~/Facility/GetByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/Facility/GetActiveByOperatingCenterId");
                a.RequiresLoggedInUserOnly("~/Facility/GetActiveByOperatingCentersWithPointOfEntryAndSystemDeliveryType");
                a.RequiresLoggedInUserOnly("~/Facility/GetActiveByOperatingCenterWithPointOfEntry");
                a.RequiresLoggedInUserOnly("~/Facility/GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId");
                a.RequiresLoggedInUserOnly("~/Facility/ByOperatingCenterOrPlanningPlant/");
                a.RequiresLoggedInUserOnly("~/Facility/UnarchivedByOperatingCenterOrPlanningPlant/");
                a.RequiresLoggedInUserOnly("~/Facility/ByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId/");
                a.RequiresLoggedInUserOnly("~/Facility/UnarchivedByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId/");
                a.RequiresLoggedInUserOnly("~/Facility/GetByOperatingCenterIdAndCommunityRightToKnowIsTrue/");
                a.RequiresLoggedInUserOnly("~/Facility/GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply/");
            });
        }

        #endregion

        #region Show(int)

        [TestMethod]
        public void TestShowReturnsJsonAddressAndCoordinates()
        {
            _currentUser.IsAdmin = true;
            var state = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var town = GetEntityFactory<Town>().Create(new { State = state });
            var townSection = GetEntityFactory<TownSection>().Create();
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var street = GetEntityFactory<Street>().Create(new {
                Prefix = GetEntityFactory<StreetPrefix>().Create(new {Description = "N"}),
                Name = "Main",
                Suffix = GetEntityFactory<StreetSuffix>().Create(new {Description = "St"}), 
                Town = town
            });
            var facility = GetEntityFactory<Facility>().Create(new {
                StreetNumber = "666",
                Street = street,
                TownSection = townSection,
                Town = town,
                ZipCode = "07711", 
                Coordinate = coordinate
            });
            InitializeControllerAndRequest("~/Facility/Show/" + facility.Id + ".json");

            var target = (JsonResult)_target.Show(facility.Id);
            dynamic data = target.Data;

            Assert.IsNotNull(target);
            Assert.AreEqual(facility.Address, data.Address);
            Assert.AreEqual(coordinate.Id, data.CoordinateId);
            Assert.AreEqual(coordinate.Latitude, data.Latitude);
            Assert.AreEqual(coordinate.Longitude, data.Longitude);
            Assert.AreEqual(1, data.DepartmentId);
            Assert.AreEqual(town.Id, data.Town);
            Assert.AreEqual(town.County.Id, data.County);
            Assert.AreEqual(street.Id, data.Street);
            Assert.AreEqual(facility.StreetNumber, data.StreetNumber);
            Assert.AreEqual(facility.ZipCode, data.ZipCode);
            Assert.AreEqual(1, data.DepartmentId);
            Assert.AreEqual(townSection.Id, data.TownSection);
        }

        [TestMethod]
        public void TestShowDisplaysNotificationIfArcFlashStudyIsRequiredAndDoesNotExist()
        {
            _currentUser.IsAdmin = true;
            var facility = GetEntityFactory<Facility>().Create(new { ArcFlashStudyRequired = true });

            var result = _target.Show(facility.Id) as ViewResult;

            Assert.AreEqual(FacilityController.ARC_FLASH_STUDY_REQUIRED,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestShowDoesNotDisplaysNotificationIfArcFlashStudyIsRequiredAndOneExist()
        {
            //var facility = GetEntityFactory<Facility>().Create(new { ArcFlashStudyRequired = true });
            //var study = GetEntityFactory<ArcFlashStudy>().Create(new { DateLabelsApplied = DateTime.Now, Facility = facility });
            //facility.ArcFlashStudies.Add(study);
            //facility.MostRecentArcFlashStudy = new MostRecentArcFlashStudy { DateLabelsApplied = study.DateLabelsApplied, Facility = facility, Id = study.Id, ExpiringWithAYear = false };

            _currentUser.IsAdmin = true;
            var study = GetEntityFactory<ArcFlashStudy>().Create(new { DateLabelsApplied = DateTime.Now.AddYears(5) });
            var facility = study.Facility;
            facility.ArcFlashStudyRequired = true;

            Session.Save(facility);
            Session.Save(study);
            Session.Flush();
            Session.Evict(facility);
            Session.Evict(study);
            
            var result = _target.Show(facility.Id) as ViewResult;

            Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]);
        }

        [TestMethod]
        public void TestShowDoesNotDisplaysNotificationIfArcFlashStudyIsNotRequired()
        {
            _currentUser.IsAdmin = true;
            var facility = GetEntityFactory<Facility>().Create(new { ArcFlashStudyRequired = false });

            var result = _target.Show(facility.Id) as ViewResult;

            Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]);
        }

        [TestMethod]
        public void TestShowDisplaysNotificationIfArcFlashStudyIsRequiredAndExpiring()
        {
            _currentUser.IsAdmin = true;
            var facility = GetFactory<FacilityFactory>().Create(new { ArcFlashStudyRequired = true });
            facility.MaintenanceRiskOfFailure = GetFactory<FacilityLowMaintenanceRiskOfFailureFactory>().Create();
            var study = GetEntityFactory<ArcFlashStudy>().Create(new { Facility = facility, DateLabelsApplied = DateTime.Now.AddYears(-5) });
            Session.Save(facility);
            Session.Save(study);
            Session.Flush();
            Session.Evict(facility);
            Session.Evict(study);
            
            var result = _target.Show(facility.Id) as ViewResult;

            Assert.AreEqual(FacilityController.ARC_FLASH_STUDY_EXPIRING,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestShowDisplaysNotificationIfArcFlashStudyIsRequiredAndExpired()
        {
            _currentUser.IsAdmin = true;
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var facility = GetFactory<FacilityFactory>().Create(new { ArcFlashStudyRequired = true });
            var study = GetEntityFactory<ArcFlashStudy>().Create(new { Facility = facility, DateLabelsApplied = DateTime.Now.AddYears(-6) });
            Session.Save(facility);
            Session.Save(study);
            Session.Flush();
            Session.Evict(facility);
            Session.Evict(study);
            
            var result = _target.Show(facility.Id) as ViewResult;

            Assert.AreEqual(FacilityController.ARC_FLASH_STUDIES_EXPIRED,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateRedirectsToShowWhenAnArcFlashStudyIsNotRequiredToBeEntered() 
        {
            _currentUser.IsAdmin = true;
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var department = GetEntityFactory<Department>().Create();
            var facility = _viewModelFactory.BuildWithOverrides<CreateFacility, Facility>(GetFactory<FacilityFactory>().Build(), new {
                OperatingCenter = operatingCenter.Id,
                Department = department.Id,
                ArcFlashStudyRequired = false
            });

            var result = _target.Create(facility) as RedirectToRouteResult;
            
            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual("Facility", result.RouteValues["controller"]);
            Assert.AreEqual(facility.Id, result.RouteValues["id"]);
        }

        [TestMethod]
        public void TestCreateRedirectsToNewArcFlashStudyWithValuesWhenAnArcFlashStudyIsRequiredToBeEntered()
        {
            _currentUser.IsAdmin = true;
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var department = GetEntityFactory<Department>().Create();
            var facility = _viewModelFactory.BuildWithOverrides<CreateFacility, Facility>(GetFactory<FacilityFactory>().Build(), new {
                OperatingCenter = operatingCenter.Id,
                Department = department.Id,
                ArcFlashStudyRequired = true
            });

            var result = _target.Create(facility) as RedirectToRouteResult;
            var entity = Repository.Load(facility.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("New", result.RouteValues["action"]);
            Assert.AreEqual("ArcFlashStudy", result.RouteValues["controller"]);
            Assert.AreEqual("Engineering", result.RouteValues["area"]);
            Assert.AreEqual(entity.Id, result.RouteValues["Facility"]);
            Assert.AreEqual(entity.OperatingCenter.State.Id, result.RouteValues["State"]);
            Assert.AreEqual(entity.OperatingCenter.Id, result.RouteValues["OperatingCenter"]);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            _currentUser.IsAdmin = true;
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var coordinate1 = GetFactory<CoordinateFactory>().Create(new { Latitude = 43m, Longitude = -74m });
            var coordinate2 = GetFactory<CoordinateFactory>().Create(new { Latitude = 1m, Longitude = 2m });
            var facility1 = GetFactory<FacilityFactory>().Create(new {
                Coordinate = coordinate1, 
                FacilityTotalCapacityMGD = 10m, 
                FacilityReliableCapacityMGD = 5.2m, 
                FacilityOperatingCapacityMGD = 3.14m, 
                FacilityRatedCapacityMGD = 6m, 
                FacilityAuxPowerCapacityMGD = 9.8m,
                UsedInProductionCapacityCalculation = true,
                CreatedBy = _currentUser,
                InsuranceId = "123-456",
                InsuranceScore = 1.2m,
                InsuranceScoreQuartile = new InsuranceScoreQuartile { Id = 1, Description = "1" },
                InsuranceVisitDate = new DateTime(2020, 4, 1)
            });
            var facility2 = GetFactory<FacilityFactory>().Create(new { Coordinate = coordinate2 });
            var search = new SearchFacility();

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(facility1.Id, "Id");
                helper.AreEqual(coordinate1.Latitude, "Latitude");
                helper.AreEqual(coordinate1.Longitude, "Longitude");
                helper.AreEqual(facility2.Id, "Id", 1);
                helper.AreEqual(facility1.FacilityTotalCapacityMGD, "Facility Total Capacity (MGD)");
                helper.AreEqual(facility1.FacilityReliableCapacityMGD, "Facility Reliable Capacity (MGD)");
                helper.AreEqual(facility1.FacilityOperatingCapacityMGD, "Facility Operating Capacity (MGD)");
                helper.AreEqual(facility1.FacilityRatedCapacityMGD, "Facility Rated Capacity (MGD)");
                helper.AreEqual(facility1.FacilityAuxPowerCapacityMGD, "Facility Aux Power Capacity (MGD)");
                helper.AreEqual(facility1.UsedInProductionCapacityCalculation, "Facility Used in Production (Treatment) Capacity Calculation");
                helper.AreEqual(coordinate2.Latitude, "Latitude", 1);
                helper.AreEqual(coordinate2.Longitude, "Longitude", 1);
                helper.AreEqual(facility1.BasicGroundWaterSupply, "Ground Water Supply - GWUDI");
                helper.AreEqual(facility1.RawWaterPumpStation, "RawWaterPumpStation");
                helper.AreEqual(facility1.InsuranceId, "Insurance ID");
                helper.AreEqual(facility1.InsuranceScore, "Insurance Score");
                helper.AreEqual(facility1.InsuranceScoreQuartile, "Insurance Score Quartile");
                helper.AreEqual(facility1.InsuranceVisitDate, "Insurance Last Visit Date");
                helper.AreEqual(facility1.CreatedBy, "CreatedBy");
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            _currentUser.IsAdmin = true;
            InitializeControllerForRequest("~/Facility/Index.map");
            var good = GetFactory<FacilityFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var bad = GetFactory<FacilityFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var model = new SearchFacility {
                OperatingCenter = good.OperatingCenter.Id
            };
            var result = (MapResult)_target.Index(model);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithoutModelsIfModelStateIsNotValid()
        {
            _currentUser.IsAdmin = true;
            InitializeControllerForRequest("~/Facility/Index.map");
            var good = GetFactory<FacilityFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });

            var model = new SearchFacility();
            var validResult = (MapResult)_target.Index(model);

            Assert.AreEqual(1, validResult.CoordinateSets.Single().Coordinates.Count());
            Assert.IsTrue(validResult.CoordinateSets.Single().Coordinates.Contains(good));

            _target.ModelState.AddModelError("error", "error");
            var badResult = (MapResult)_target.Index(model);

            Assert.IsFalse(badResult.CoordinateSets.Any());
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            _currentUser.IsAdmin = true;
            var facility = GetFactory<FacilityFactory>().Create();
            var expected = "NJSB-21";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditFacility, Facility>(facility, new {
                FacilityName = expected
            }));

            Assert.AreEqual(expected, Session.Get<Facility>(facility.Id).FacilityName);
        }

        #endregion

        #region System Delivery

        [TestMethod]
        public void TestAddSystemDeliveryEntryTypeAddsEntryType()
        {
            var entity = GetEntityFactory<Facility>().Create();
            var facility = _viewModelFactory.BuildWithOverrides<AddFacilitySystemDeliveryEntryType, Facility>(entity, new {IsEnabled = true, SystemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create().Id});

            Assert.AreEqual(0, entity.FacilitySystemDeliveryEntryTypes.Count()); // Make sure this is zero

            var result = (RedirectToRouteResult)_target.AddSystemDeliveryEntryType(facility);
            entity = Repository.Load(facility.Id);

            Assert.AreEqual(entity.Id, facility.Id);
            Assert.AreEqual(1, entity.FacilitySystemDeliveryEntryTypes.Count());
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual("Facility", result.RouteValues["controller"]);
            Assert.AreEqual(facility.Id, result.RouteValues["id"]);
        }

        [TestMethod]
        public void TestAddSystemDeliveryEntryTypeDoesNotAddDuplicateActiveEntryType()
        {
            var entity = GetEntityFactory<Facility>().Create();
            var systemDelivery = GetEntityFactory<SystemDeliveryEntryType>().Create();
            var facility = _viewModelFactory.BuildWithOverrides<AddFacilitySystemDeliveryEntryType, Facility>(entity, new {IsEnabled = true, SystemDeliveryEntryType = systemDelivery.Id});

            Assert.AreEqual(0, entity.FacilitySystemDeliveryEntryTypes.Count()); // Make sure this is zero

            var result = (RedirectToRouteResult)_target.AddSystemDeliveryEntryType(facility);
            entity = Repository.Load(facility.Id);

            Assert.AreEqual(1, entity.FacilitySystemDeliveryEntryTypes.Count());
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual("Facility", result.RouteValues["controller"]);
            Assert.AreEqual(facility.Id, result.RouteValues["id"]);

            result = (RedirectToRouteResult)_target.AddSystemDeliveryEntryType(facility);
            entity = Repository.Load(facility.Id);

            Assert.AreEqual(1, entity.FacilitySystemDeliveryEntryTypes.Count());
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual("Facility", result.RouteValues["controller"]);
            Assert.AreEqual(facility.Id, result.RouteValues["id"]);

            facility = _viewModelFactory.BuildWithOverrides<AddFacilitySystemDeliveryEntryType, Facility>(entity, new {IsEnabled = false, SystemDeliveryEntryType = systemDelivery.Id});
            result = (RedirectToRouteResult)_target.AddSystemDeliveryEntryType(facility);
            entity = Repository.Load(facility.Id);

            Assert.AreEqual(2, entity.FacilitySystemDeliveryEntryTypes.Count());
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual("Facility", result.RouteValues["controller"]);
            Assert.AreEqual(facility.Id, result.RouteValues["id"]);
        }

        [TestMethod]
        public void TestRemoveSystemDeliveryEntryTypeRemovesEntryType()
        {
            var entryType = GetEntityFactory<SystemDeliveryEntryType>().Create();
            var entity = GetEntityFactory<Facility>().Create();
            var sysdeliveryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {SystemDeliveryEntryType = entryType, Facility = entity});
            var facility = _viewModelFactory.BuildWithOverrides<RemoveFacilitySystemDeliveryEntryType, Facility>(entity, new {FacilitySystemDeliveryEntryTypeId = entryType.Id});

            entity.FacilitySystemDeliveryEntryTypes.Add(sysdeliveryType);

            var result = (RedirectToRouteResult)_target.RemoveSystemDeliveryEntryType(facility);

            Assert.AreEqual(0, entity.FacilitySystemDeliveryEntryTypes.Count());
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual("Facility", result.RouteValues["controller"]);
            Assert.AreEqual(entity.Id, result.RouteValues["id"]);
        }

        #endregion

        #region ByOperatingCenterId

        [TestMethod]
        public void TestByOperatingCenterIdReturnsCascadingResultForMatchingOperatingCenters()
        {
            _currentUser.IsAdmin = true;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opc });
            var badFacility = GetFactory<FacilityFactory>().Create();

            var result = (CascadingActionResult)_target.ByOperatingCenterId(opc.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodFacility.Id.ToString(), actual[1].Value);
        }

        #endregion

        #region ByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId

        [TestMethod]
        public void TestByOperatingCenterIdOrPlanningPlantReturnsCascadingResultForMatchingOperatingCenters()
        {
            var planc = GetEntityFactory<PlanningPlant>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opc });
            var badFacility = GetFactory<FacilityFactory>().Create(new {PlanningPlant = planc});

            var result = (CascadingActionResult)_target.ByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId(opc.Id, null);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodFacility.Id.ToString(), actual[1].Value);
        }

        [TestMethod]
        public void TestByOperatingCenterIdOrPlanningPlantReturnsCascadingResultForMatchingOperatingCenterAndPlanningPlant()
        { 
            var planc = GetEntityFactory<PlanningPlant>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opc, PlanningPlant = planc });
            var badFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opc});

            var result = (CascadingActionResult)_target.ByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId(opc.Id, planc.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodFacility.Id.ToString(), actual[1].Value);
        }

        #endregion

        #region GetActiveByOperatingCenterId

        [TestMethod]
        public void TestGetActiveByOperatingCenterIdReturnsWhenFacilityIsActive()
        {
            //Arrange
            _currentUser.IsAdmin = true;
            var opcenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new
                {OperatingCenter = opcenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            var inactiveFacility = GetFactory<FacilityFactory>().Create(new
                {OperatingCenter = opcenter1, FacilityStatus = typeof(InactiveFacilityStatusFactory)});
            var otherActiveFacility = GetFactory<FacilityFactory>().Create(new
                {OperatingCenter = opcenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            
            //Act
            var resultActive = (CascadingActionResult)_target.GetActiveByOperatingCenterId(opcenter1.Id);
            var actual = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(1, actual.Count() - 1, "There should only be two items: the empty select and the active facility");
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility.Id.ToString()), "Only the active facility must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == inactiveFacility.Id.ToString()), "Only the active facility must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == otherActiveFacility.Id.ToString()), "Only the active facility must be in the results");
        }

        #endregion
        
        #region GetActiveByOperatingCenterIdWithPointOfEntryAndSystemDeliveryType

        [TestMethod]
        public void TestGetActiveByOperatingCenterIdReturnsWhenFacilityIsActiveithPointOfEntryAndSystemDeliveryType()
        {
            //Arrange
            _currentUser.IsAdmin = true;
            var systemdeliverytype = GetEntityFactory<SystemDeliveryType>().Create();
            var systemDeliveryEntryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var opcenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcenter3 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemdeliverytype
            });
            var inactiveFacility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter1, FacilityStatus = typeof(InactiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemdeliverytype
            });
            var otherActiveFacility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemdeliverytype
            });
            var transferredFromFacility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemdeliverytype
            });
            var activeFacilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                IsEnabled = true, Facility = activeFacility
            });
            var otherActiveFacilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                IsEnabled = true, Facility = otherActiveFacility
            });
            var transferredFromFacilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                IsEnabled = true, Facility = otherActiveFacility, SystemDeliveryEntryType = systemDeliveryEntryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM)
            });

            activeFacility.FacilitySystemDeliveryEntryTypes.Add(activeFacilityEntryType);
            otherActiveFacility.FacilitySystemDeliveryEntryTypes.Add(otherActiveFacilityEntryType);
            transferredFromFacility.FacilitySystemDeliveryEntryTypes.Add(transferredFromFacilityEntryType);

            Session.SaveOrUpdate(transferredFromFacility);
            Session.SaveOrUpdate(activeFacility);
            Session.SaveOrUpdate(otherActiveFacility);
            //Act
            var resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithPointOfEntryAndSystemDeliveryType(new int[]{opcenter1.Id, opcenter2.Id}, systemdeliverytype.Id);
            var actual = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(2, actual.Count() - 1, "There should only be two items: the empty select and the active facility");
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility.Id.ToString()), "Only the active facility must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == inactiveFacility.Id.ToString()), "Only the active facility must be in the results");
            Assert.IsTrue(actual.Any(x => x.Value == otherActiveFacility.Id.ToString()), "Only the active facility must be in the results");
        }

        #endregion

        #region GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply

        [TestMethod]
        public void TestGetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply()
        {
            //Arrange
            _currentUser.IsAdmin = true;
            var systemDeliveryType1 = GetFactory<SystemDeliveryTypeWaterFactory>().Create();
            var systemDeliveryType2 = GetFactory<SystemDeliveryTypeWasteWaterFactory>().Create();
            var publicWaterSupply1 = new[] { GetFactory<PublicWaterSupplyFactory>().Create() };
            var publicWaterSupply2 = new[] { GetFactory<PublicWaterSupplyFactory>().Create() };
            var opcenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var active1Facility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemDeliveryType1, PublicWaterSupply = publicWaterSupply1[0]
            });
            var active2Facility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemDeliveryType1, PublicWaterSupply = publicWaterSupply2[0]
            });
            var active3Facility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemDeliveryType2, PublicWaterSupply = publicWaterSupply1[0]
            });

            var activeFacility1EntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new { IsEnabled = true, Facility = active1Facility });
            var activeFacility2EntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new { IsEnabled = true, Facility = active2Facility });
            var activeFacility3EntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new { IsEnabled = true, Facility = active3Facility });

            active1Facility.FacilitySystemDeliveryEntryTypes.Add(activeFacility1EntryType);
            active2Facility.FacilitySystemDeliveryEntryTypes.Add(activeFacility2EntryType);
            active3Facility.FacilitySystemDeliveryEntryTypes.Add(activeFacility3EntryType);

            Session.SaveOrUpdate(active1Facility);
            Session.SaveOrUpdate(active2Facility);
            Session.SaveOrUpdate(active3Facility);

            //Act
            var resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply(new[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType1.Id, new[] { publicWaterSupply1[0].Id });
            var actual1 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply(new[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType1.Id, new[] { publicWaterSupply2[0].Id });
            var actual2 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply(new[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType2.Id, new[] { publicWaterSupply1[0].Id });
            var actual3 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply(new[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType2.Id, new[] { publicWaterSupply2[0].Id });
            var actual4 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply(new[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType1.Id, null);
            var actual5 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply(new[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType2.Id, null);
            var actual6 = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(1, actual1.Count() - 1, "There should be 1 item.");
            Assert.AreEqual(1, actual2.Count() - 1, "There should be 1 item.");
            Assert.AreEqual(1, actual3.Count() - 1, "There should be 1 item.");
            Assert.AreEqual(-1, actual4.Count() - 1, "There should be 0 (-1) items.");
            Assert.AreEqual(2, actual5.Count() - 1, "There should be 2 items.");
            Assert.AreEqual(1, actual6.Count() - 1, "There should be 1 item.");
        }

        #endregion

        #region TestGetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId

        [TestMethod]
        public void TestGetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId()
        {
            //Arrange
            _currentUser.IsAdmin = true;
            var systemDeliveryType1 = GetFactory<SystemDeliveryTypeWaterFactory>().Create();
            var systemDeliveryType2 = GetFactory<SystemDeliveryTypeWasteWaterFactory>().Create();
            var publicWaterSupply1 = new[] { GetFactory<PublicWaterSupplyFactory>().Create() };
            var publicWaterSupply2 = new[] { GetFactory<PublicWaterSupplyFactory>().Create() };
            var wasteWaterSystem1 = new[] { GetFactory<WasteWaterSystemFactory>().Create() };
            var wasteWaterSystem2 = new[] { GetFactory<WasteWaterSystemFactory>().Create() };
            var opcenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var active1Facility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemDeliveryType1, WasteWaterSystem = wasteWaterSystem1[0], PublicWaterSupply = publicWaterSupply1[0]
            });
            var active2Facility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemDeliveryType1, WasteWaterSystem = wasteWaterSystem2[0], PublicWaterSupply = publicWaterSupply2[0]
            });
            var active3Facility = GetFactory<FacilityFactory>().Create(new {
                OperatingCenter = opcenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true, SystemDeliveryType = systemDeliveryType2, WasteWaterSystem = wasteWaterSystem1[0], PublicWaterSupply = publicWaterSupply1[0]
            });

            var activeFacility1EntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new { IsEnabled = true, Facility = active1Facility });
            var activeFacility2EntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new { IsEnabled = true, Facility = active2Facility });
            var activeFacility3EntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new { IsEnabled = true, Facility = active3Facility });

            active1Facility.FacilitySystemDeliveryEntryTypes.Add(activeFacility1EntryType);
            active2Facility.FacilitySystemDeliveryEntryTypes.Add(activeFacility2EntryType);
            active3Facility.FacilitySystemDeliveryEntryTypes.Add(activeFacility3EntryType);

            Session.SaveOrUpdate(active1Facility);
            Session.SaveOrUpdate(active2Facility);
            Session.SaveOrUpdate(active3Facility);

            //Act
            var resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId(new int[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType1.Id, new[] { publicWaterSupply1[0].Id}, new[] { wasteWaterSystem1[0].Id});
            var actual1 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId(new int[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType1.Id, new[] { publicWaterSupply2[0].Id}, new[] { wasteWaterSystem2[0].Id});
            var actual2 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId(new int[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType2.Id, new[] { publicWaterSupply1[0].Id}, new[] { wasteWaterSystem1[0].Id});
            var actual3 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId(new int[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType2.Id, new[] {publicWaterSupply2[0].Id}, new[] { wasteWaterSystem2[0].Id});
            var actual4 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId(new int[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType1.Id, null, null);
            var actual5 = resultActive.GetSelectListItems().ToArray();
            resultActive = (CascadingActionResult)_target.GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId(new int[] { opcenter1.Id, opcenter2.Id }, systemDeliveryType2.Id, null, null);
            var actual6 = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(1, actual1.Count() - 1, "There should be 1 item.");
            Assert.AreEqual(1, actual2.Count() - 1, "There should be 1 item.");
            Assert.AreEqual(1, actual3.Count() - 1, "There should be 1 item.");
            Assert.AreEqual(-1, actual4.Count() - 1, "There should be 0 (-1) items.");
            Assert.AreEqual(2, actual5.Count() - 1, "There should be 2 items.");
            Assert.AreEqual(1, actual6.Count() - 1, "There should be 1 item.");
        }

        #endregion

        #region GetActiveByOperatingCenterWithPointOfEntry

        [TestMethod]
        public void TestGetActiveByOperatingCenterWithPointOfEntry()
        {
            // Arrange 
            _currentUser.IsAdmin = true;
            var opCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeFacility1 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opCenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true});
            var activeFacility2 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opCenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            var activeFacility3 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opCenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true});
            var activeFacility4 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opCenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true});
            var activeFacility5 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opCenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            var activeFacility6 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opCenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory), PointOfEntry = true});

            // Act
            var result = (CascadingActionResult)_target.GetActiveByOperatingCenterWithPointOfEntry(opCenter1.Id);
            var actual = result.GetSelectListItems().ToArray();

            // Assert 
            Assert.AreEqual(2, actual.Count() - 1);
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility1.Id.ToString()));
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility3.Id.ToString()));
            Assert.IsFalse(actual.Any(x => x.Value == activeFacility2.Id.ToString()));
            Assert.IsFalse(actual.Any(x => x.Value == activeFacility4.Id.ToString()));
            Assert.IsFalse(actual.Any(x => x.Value == activeFacility6.Id.ToString()));

            // Act: The sequel
            result = (CascadingActionResult)_target.GetActiveByOperatingCenterWithPointOfEntry(opCenter2.Id);
            actual = result.GetSelectListItems().ToArray();

            // Assert: The sequel
            Assert.AreEqual(2, actual.Count() - 1);
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility4.Id.ToString()));
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility6.Id.ToString()));
            Assert.IsFalse(actual.Any(x => x.Value == activeFacility5.Id.ToString()));
            Assert.IsFalse(actual.Any(x => x.Value == activeFacility1.Id.ToString()));
            Assert.IsFalse(actual.Any(x => x.Value == activeFacility3.Id.ToString()));
        }

        #endregion

        #region ActiveAndPendingRetirementByOperatingCenterAppearsInDropdown

        [TestMethod]
        public void TestActiveAndPendingRetirementByOperatingCenterAppearsInDropdownWhenFacilityIsActive()
        {
            //Arrange
            _currentUser.IsAdmin = true;
            var opcenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            var inactiveFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter1, FacilityStatus = typeof(InactiveFacilityStatusFactory) });
            var otherActiveFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            var pendingRetFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter1, FacilityStatus = typeof(PendingRetirementFacilityStatusFactory) });
            var otherPendingRetFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter2, FacilityStatus = typeof(PendingRetirementFacilityStatusFactory) });

            //Act
            var resultActive = (CascadingActionResult)_target.ActiveAndPendingRetirementByOperatingCenterOrPlanningPlant(opcenter1.Id, null);
            var actualActive = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(2, actualActive.Count() - 1, "There should only be three items: the empty select, the active facility, and the pending retirement facility");
            Assert.IsTrue(actualActive.Any(x => x.Value == activeFacility.Id.ToString()), "Only the active facility and the pending retirement facility must be in the results");
            Assert.IsFalse(actualActive.Any(x => x.Value == inactiveFacility.Id.ToString()), "Only the active facility and the pending retirement facility must be in the results");
            Assert.IsFalse(actualActive.Any(x => x.Value == otherActiveFacility.Id.ToString()), "Only the active facility and the pending retirement facility must be in the results");
            Assert.IsTrue(actualActive.Any(x => x.Value == pendingRetFacility.Id.ToString()), "Only the active facility and the pending retirement facility must be in the results");
            Assert.IsFalse(actualActive.Any(x => x.Value == otherPendingRetFacility.Id.ToString()), "Only the active facility and the pending retirement facility must be in the results");
        }

        [TestMethod]
        public void TestActiveAndPendingRetirementByOperatingCenterAndPlanningPlantAppearsInDropdownWhenFacilityIsActive()
        {
            //Arrange - with the otherActiveFacility having a planning plant, the output's will be different 
            _currentUser.IsAdmin = true;
            var opcenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var planplant1 = GetFactory<PlanningPlantFactory>().Create();
            var planplant2 = GetFactory<PlanningPlantFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant1, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            var inactiveFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant1, FacilityStatus = typeof(InactiveFacilityStatusFactory) });
            var otherActiveFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant2, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            var pendingRetFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant1, FacilityStatus = typeof(PendingRetirementFacilityStatusFactory) });
            var otherPendingRetFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant2, FacilityStatus = typeof(PendingRetirementFacilityStatusFactory) });
            
            //Act
            var resultActive = (CascadingActionResult)_target.ActiveAndPendingRetirementByOperatingCenterOrPlanningPlant(opcenter.Id, planplant1.Id);
            var actual = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(2, actual.Count() - 1, "There should only be three items: the empty select and the active facility and the pending retirement facility");
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility.Id.ToString()), "Only the active and pending retirement facilities must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == inactiveFacility.Id.ToString()), "Only the active and pending retirement facilities must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == otherActiveFacility.Id.ToString()), "Only the active and pending retirement facilities must be in the results");
            Assert.IsTrue(actual.Any(x => x.Value == pendingRetFacility.Id.ToString()), "Only the active and pending retirement facilities must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == otherPendingRetFacility.Id.ToString()), "Only the active and pending retirement facilities must be in the results");
        }

        #endregion

        #region ActiveByOperatingCenterAppearsInDropdownWhenFacilityIsActive

        [TestMethod]
        public void TestActiveByOperatingCenterAppearsInDropdownWhenFacilityIsActive()
        {
            //Arrange
            _currentUser.IsAdmin = true;
            var opcenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opcenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            var inactiveFacility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opcenter1, FacilityStatus = typeof(InactiveFacilityStatusFactory) });
            var otherActiveFacility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opcenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            
            //Act
            var resultActive = (CascadingActionResult)_target.ActiveByOperatingCenterOrPlanningPlant(opcenter1.Id, null);
            var actualActive = resultActive.GetSelectListItems().ToArray();
            
            //Assert
            Assert.AreEqual(1, actualActive.Count() - 1, "There should only be two items: the empty select and the active facility");
            Assert.IsTrue(actualActive.Any(x => x.Value == activeFacility.Id.ToString()), "Only the active facility must be in the results");
            Assert.IsFalse(actualActive.Any(x => x.Value == inactiveFacility.Id.ToString()), "Only the active facility must be in the results");
            Assert.IsFalse(actualActive.Any(x => x.Value == otherActiveFacility.Id.ToString()), "Only the active facility must be in the results");
        }

        [TestMethod]
        public void TestActiveByOperatingCentersOrPlanningPlantWhenFaciliyIsActive()
        {
            _currentUser.IsAdmin = true;
            var opcenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opcenter1, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            var inactiveFacility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opcenter1, FacilityStatus = typeof(InactiveFacilityStatusFactory) });
            var otherActiveFacility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opcenter2, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            
            //Act
            var resultActive = (CascadingActionResult)_target.ActiveByOperatingCentersOrPlanningPlant(new[] {opcenter1.Id, opcenter2.Id}, null);
            var actualActive = (IEnumerable<FacilityDisplayItem>)resultActive.Data;
            
            //Assert
            Assert.AreEqual(2, actualActive.Count(), "There should only be two items: the empty select and the active facility");
            Assert.IsTrue(actualActive.Any(x => x.Id == activeFacility.Id), "Only the active facility must be in the results");
            Assert.IsFalse(actualActive.Any(x => x.Id == inactiveFacility.Id), "Only the active facility must be in the results");
            Assert.IsTrue(actualActive.Any(x => x.Id == otherActiveFacility.Id), "Only the active facility must be in the results");
        }

        #endregion
        
        #region ActiveByOperatingCenterAndPlanningPlantAppearsInDropdownWhenFacilityIsActive

        [TestMethod]
        public void TestActiveByOperatingCenterAndPlanningPlantAppearsInDropdownWhenFacilityIsActive()
        {
            //Arrange - with the otherActiveFacility having a planning plant, the output's will be different 
            _currentUser.IsAdmin = true;
            var opcenter = GetFactory<UniqueOperatingCenterFactory>().Create(); 
            var planplant1 = GetFactory<PlanningPlantFactory>().Create();
            var planplant2 = GetFactory<PlanningPlantFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opcenter, PlanningPlant = planplant1, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            var inactiveFacility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opcenter, PlanningPlant = planplant1, FacilityStatus = typeof(InactiveFacilityStatusFactory)});
            var otherActiveFacility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opcenter, PlanningPlant = planplant2, FacilityStatus = typeof(ActiveFacilityStatusFactory)});
            
            //Act
            var resultActive = (CascadingActionResult)_target.ActiveByOperatingCenterOrPlanningPlant(opcenter.Id, planplant1.Id);
            var actual = resultActive.GetSelectListItems().ToArray();
            
            //Assert
            Assert.AreEqual(1, actual.Count() - 1, "There should only be three items: the empty select and the active facility for opcenter1 and the active facility for opcenter2");
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility.Id.ToString()), "Only the active facilities must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == inactiveFacility.Id.ToString()), "No active facilities should be listed");
            Assert.IsFalse(actual.Any(x => x.Value == otherActiveFacility.Id.ToString()), "Only the active facility must be in the results");
        }

        #endregion

        #region UnarchivedByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId

        [TestMethod]
        public void TestUnarchivedByOperatingCenterAndSometimesPlanningPlantAppearsInDropdownWhenFacilityIsUnarchived()
        {
            //Arrange - with the otherActiveFacility having a planning plant, the output's will be different 
            var opcenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var planplant1 = GetFactory<PlanningPlantFactory>().Create();
            var planplant2 = GetFactory<PlanningPlantFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant1, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            var inactiveFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant1, FacilityStatus = typeof(InactiveFacilityStatusFactory) });
            var otherActiveFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant2, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            var archivedActiveFacility = GetFactory<FacilityFactory>().Create(new { FacilityName = "Archive bunker", OperatingCenter = opcenter, PlanningPlant = planplant2, FacilityStatus = typeof(ActiveFacilityStatusFactory) });

            //Act
            var resultActive = (CascadingActionResult)_target.UnarchivedByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId(opcenter.Id, planplant1.Id);
            var actual = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(1, actual.Count() - 1, "There should only be three items: the empty select and the active facility for opcenter1 and the active facility for opcenter2");
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility.Id.ToString()), "Only the unarchived facilities must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == inactiveFacility.Id.ToString()), "No unarchived facilities should be listed");
            Assert.IsFalse(actual.Any(x => x.Value == otherActiveFacility.Id.ToString()), "Only the unarchived facility must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == archivedActiveFacility.Id.ToString()), "Only the unarchived facility must be in the results");
        }

        [TestMethod]
        public void TestUnarchivedByOperatingCenterAndSometimesPlanningPlantAppearsInDropdownWhenFacilityIsUnarchivedAndPlanPlantNull()
        {
            //Arrange - with the otherActiveFacility having a planning plant, the output's will be different 
            var opcenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var planplant1 = GetFactory<PlanningPlantFactory>().Create();
            var planplant2 = GetFactory<PlanningPlantFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant1, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            var inactiveFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant1, FacilityStatus = typeof(InactiveFacilityStatusFactory) });
            var otherActiveFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opcenter, PlanningPlant = planplant2, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            var thirdActiveFacility = GetFactory<FacilityFactory>().Create(new { FacilityName = "Archey bunker", OperatingCenter = opcenter, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            var archivedActiveFacility = GetFactory<FacilityFactory>().Create(new { FacilityName = "Archive bunker", OperatingCenter = opcenter, PlanningPlant = planplant2, FacilityStatus = typeof(ActiveFacilityStatusFactory) });
            //Act
            var resultActive = (CascadingActionResult)_target.UnarchivedByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId(opcenter.Id,null);
            var actual = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(3, actual.Count() - 1, "There should only be three items: the empty select and the active facility for opcenter1 and the active facility for opcenter2");
            Assert.IsTrue(actual.Any(x => x.Value == activeFacility.Id.ToString()), "Only the unarchived facilities must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == inactiveFacility.Id.ToString()), "No unarchived facilities should be listed");
            Assert.IsTrue(actual.Any(x => x.Value == otherActiveFacility.Id.ToString()), "Only the unarchived facilities must be in the results");
            Assert.IsTrue(actual.Any(x => x.Value == thirdActiveFacility.Id.ToString()), "Only the unarchived facilities must be in the results");
            Assert.IsFalse(actual.Any(x => x.Value == archivedActiveFacility.Id.ToString()), "No unarchived facilities should be listed");
        }

        #endregion

        #region ByFunctionalLocation

        [TestMethod]
        public void TestByFunctionalLocationReturnsCascadingResultForMatchingFunctionalLocations()
        {
            _currentUser.IsAdmin = true;
            var functionalLocation = "AABB-CCDD-EEFF";
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opc, FunctionalLocation = "AABB-CCDD", Description = "good facility" });
            var badFacility1 = GetFactory<FacilityFactory>().Create(new { FunctionalLocation = "AABB-CCDE", Description = "bad facility 1" });
            var badFacility2 = GetFactory<FacilityFactory>().Create(new { FunctionalLocation = "AABB", Description = "bad facility 2" });

            var result = (CascadingActionResult)_target.ByFunctionalLocation(functionalLocation);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodFacility.Id.ToString(), actual[1].Value);
        }

        #endregion

        #region ByTownId

        [TestMethod]
        public void TestByTownIdReturnsCascadingResultForMatchingTown()
        {
            _currentUser.IsAdmin = true;
            var town = GetFactory<TownFactory>().Create();
            var goodFacility = GetFactory<FacilityFactory>().Create(new { Town = town });
            var badFacility = GetFactory<FacilityFactory>().Create();

            var result = (CascadingActionResult)_target.ByTownId(town.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodFacility.Id.ToString(), actual[1].Value);
        }

        #endregion

        #region ByPublicWaterSupplyId

        [TestMethod]
        public void TestByPublicWaterSupplyIdReturnsFacility()
        {
            _currentUser.IsAdmin = true;
            var publicWaterSupply = GetFactory<PublicWaterSupplyFactory>().Create();
            var goodFacility = GetFactory<FacilityFactory>().Create(new { PublicWaterSupply = publicWaterSupply });
            var badFacility = GetFactory<FacilityFactory>().Create();

            var result = (CascadingActionResult)_target.GetByPublicWaterSupplyId(publicWaterSupply.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodFacility.Id.ToString(), actual[1].Value);
        }

        #endregion

        #region Readings

        [TestMethod]
        public void TestReadingsDoesNotCreateChartIfModelStateIsNotValid()
        {
            _target.ModelState.AddModelError("Oops", "oops");
            _target.Readings(null);

            Assert.IsFalse(_target.ViewData.ContainsKey("Chart"));
        }

        [TestMethod]
        public void TestReadingsCreatesChartWithExpectedReadingsForDailyReadings()
        {
            _currentUser.IsAdmin = true;
            var kw = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();
            var equipment = GetFactory<EquipmentFactory>().Create();
            var sensor = GetFactory<SensorFactory>().Create(new { Name = "Sensor One", MeasurementType = kw });
            var facility = equipment.Facility;
            // Make some readings
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 0, 0, 0), ScaledData = 1 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 2, 0, 15, 0), ScaledData = 2 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 3, 0, 30, 0), ScaledData = 3 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 4, 0, 45, 0), ScaledData = 4 });
            // Make sure sensor is associated with equipment.
            GetFactory<EquipmentSensorFactory>().Create(new { Equipment = equipment, Sensor = sensor });

            var search = new SearchFacilityReadings();
            search.StartDate = new DateTime(2014, 1, 1, 0, 0, 0);
            search.EndDate = new DateTime(2014, 1, 4, 0, 0, 0);
            search.Id = facility.Id;
            search.Interval = ReadingGroupType.Daily;

            _target.Readings(search);

            var chart = (ChartBuilder<DateTime, double>)_target.ViewData["Chart"];

            var seriesOne = chart.Series.Single();
            Assert.AreEqual(0.25d, seriesOne[new DateTime(2014, 1, 1, 0, 0, 0)]);
            Assert.AreEqual(0.5d, seriesOne[new DateTime(2014, 1, 2, 0, 0, 0)]);
            Assert.AreEqual(0.75d, seriesOne[new DateTime(2014, 1, 3, 0, 0, 0)]);
            Assert.AreEqual(1d, seriesOne[new DateTime(2014, 1, 4, 0, 0, 0)]);
        }

        [TestMethod]
        public void TestReadingsCreatesChartWithExpectedReadingsForHourlyReadings()
        {
            _currentUser.IsAdmin = true;
            var kw = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();
            var equipment = GetFactory<EquipmentFactory>().Create();
            var sensor = GetFactory<SensorFactory>().Create(new { Name = "Sensor One", MeasurementType = kw });
            var facility = equipment.Facility;
            // Make some readings
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 0, 0, 0), ScaledData = 1 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 1, 15, 0), ScaledData = 2 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 2, 30, 0), ScaledData = 3 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 3, 45, 0), ScaledData = 4 });
            // Make sure sensor is associated with equipment.
            GetFactory<EquipmentSensorFactory>().Create(new { Equipment = equipment, Sensor = sensor });

            var search = new SearchFacilityReadings();
            search.StartDate = new DateTime(2014, 1, 1, 0, 0, 0);
            search.EndDate = new DateTime(2014, 1, 1, 0, 0, 0);
            search.Id = facility.Id;
            search.Interval = ReadingGroupType.Hourly;

            _target.Readings(search);

            var chart = (ChartBuilder<DateTime, double>)_target.ViewData["Chart"];

            var seriesOne = chart.Series.Single();
            Assert.AreEqual(0.25d, seriesOne[new DateTime(2014, 1, 1, 0, 0, 0)]);
            Assert.AreEqual(0.5d, seriesOne[new DateTime(2014, 1, 1, 1, 0, 0)]);
            Assert.AreEqual(0.75d, seriesOne[new DateTime(2014, 1, 1, 2, 0, 0)]);
            Assert.AreEqual(1d, seriesOne[new DateTime(2014, 1, 1, 3, 0, 0)]);
        }

        [TestMethod]
        public void TestReadingsCreatesChartWithExpectedReadingsForQuarterHourlyReadings()
        {
            _currentUser.IsAdmin = true;
            var kw = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();
            var equipment = GetFactory<EquipmentFactory>().Create();
            var sensor = GetFactory<SensorFactory>().Create(new { Name = "Sensor One", MeasurementType = kw });
            var facility = equipment.Facility;
            // Make some readings
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 0, 0, 0), ScaledData = 1 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 0, 15, 0), ScaledData = 2 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 0, 30, 0), ScaledData = 3 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 0, 45, 0), ScaledData = 4 });
            // Make sure sensor is associated with equipment.
            GetFactory<EquipmentSensorFactory>().Create(new { Equipment = equipment, Sensor = sensor });

            var search = new SearchFacilityReadings();
            search.StartDate = new DateTime(2014, 1, 1, 0, 0, 0);
            search.EndDate = new DateTime(2014, 1, 1, 0, 0, 0);
            search.Id = facility.Id;
            search.Interval = ReadingGroupType.QuarterHour;

            _target.Readings(search);

            var chart = (ChartBuilder<DateTime, double>)_target.ViewData["Chart"];

            var seriesOne = chart.Series.Single();
            Assert.AreEqual(0.25d, seriesOne[new DateTime(2014, 1, 1, 0, 0, 0)]);
            Assert.AreEqual(0.5d, seriesOne[new DateTime(2014, 1, 1, 0, 15, 0)]);
            Assert.AreEqual(0.75d, seriesOne[new DateTime(2014, 1, 1, 0, 30, 0)]);
            Assert.AreEqual(1d, seriesOne[new DateTime(2014, 1, 1, 0, 45, 0)]);
        }

        [TestMethod]
        public void TestReadingsSetsReadingCostsOnViewModelInOrderByDate()
        {
            _currentUser.IsAdmin = true;
            var kw = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();
            var equipment = GetFactory<EquipmentFactory>().Create();
            var sensor = GetFactory<SensorFactory>().Create(new { Name = "Sensor One", MeasurementType = kw });
            var facility = equipment.Facility;
            var facilityKwh1 =
                GetFactory<FacilityKwhCostFactory>()
                   .Create(
                        new {
                            Facility = facility,
                            CostPerKwh = 0.5m,
                            StartDate = new DateTime(2014, 1, 1, 0, 0, 0),
                            EndDate = new DateTime(2014, 1, 2, 0, 0, 0)
                        });
            var facilityKwh2 =
                GetFactory<FacilityKwhCostFactory>()
                   .Create(
                        new {
                            Facility = facility,
                            CostPerKwh = 1.0m,
                            StartDate = new DateTime(2014, 1, 3, 0, 0, 0),
                            EndDate = new DateTime(2014, 1, 4, 0, 0, 0)
                        });

            facility.KwhCosts.Add(facilityKwh1);
            facility.KwhCosts.Add(facilityKwh2);

            // Make some readings that are out of order
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 4, 0, 45, 0), ScaledData = 4 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 1, 0, 0, 0), ScaledData = 1 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 3, 0, 30, 0), ScaledData = 3 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensor, DateTimeStamp = new DateTime(2014, 1, 2, 0, 15, 0), ScaledData = 2 });
            // Make sure sensor is associated with equipment.
            GetFactory<EquipmentSensorFactory>().Create(new { Equipment = equipment, Sensor = sensor });

            var search = new SearchFacilityReadings();
            search.StartDate = new DateTime(2014, 1, 1, 0, 0, 0);
            search.EndDate = new DateTime(2014, 1, 4, 0, 0, 0);
            search.Id = facility.Id;
            search.Interval = ReadingGroupType.Daily;

            _target.Readings(search);

            var readingCosts = search.ReadingCosts.ToArray();

            // Need 1/4th the ScaledData value since they're 15 minute intervals
            Assert.AreEqual(0.25d, readingCosts[0].ReadingValue);
            Assert.AreEqual(0.5d, readingCosts[1].ReadingValue);
            Assert.AreEqual(0.75d, readingCosts[2].ReadingValue);
            Assert.AreEqual(1d, readingCosts[3].ReadingValue);

            Assert.AreEqual(facilityKwh1.CostPerKwh, readingCosts[0].KwhCost);
            Assert.AreEqual(facilityKwh1.CostPerKwh, readingCosts[1].KwhCost);
            Assert.AreEqual(facilityKwh2.CostPerKwh, readingCosts[2].KwhCost);
            Assert.AreEqual(facilityKwh2.CostPerKwh, readingCosts[3].KwhCost);
        }

        #endregion

        #region Lookups

        [TestMethod]
        public void TestFacilityDropDownLookupForNewSetsActiveOC()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _target.SetLookupData(ControllerAction.New);

            var opcDropDownData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            // Need to filter out operating center created during _authentication service setup
            var _authenticationServiceOc = _authenticationService.Object.CurrentUser.DefaultOperatingCenter.Id.ToString();

            var dropDownData = opcDropDownData.Where(x => x.Value != _authenticationServiceOc);

            Assert.AreEqual(1, dropDownData.Count());
            Assert.AreEqual(opc1.Id.ToString(), dropDownData.First().Value);
            Assert.AreNotEqual(opc2.Id.ToString(), dropDownData.First().Value);
        }

        [TestMethod]
        public void TestFacilityDropDownLookupForShowSetsActiveOC()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _target.SetLookupData(ControllerAction.Show);

            var opcDropDownData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            // Need to filter out operating center created during _authentication service setup
            var _authenticationServiceOc = _authenticationService.Object.CurrentUser.DefaultOperatingCenter.Id.ToString();

            var dropDownData = opcDropDownData.Where(x => x.Value != _authenticationServiceOc);

            Assert.AreEqual(1, dropDownData.Count());
            Assert.AreEqual(opc1.Id.ToString(), dropDownData.First().Value);
            Assert.AreNotEqual(opc2.Id.ToString(), dropDownData.First().Value);
        }

        #endregion

        #region FacilityArea

        [TestMethod]
        public void TestAddFacilityAreaAddsToFacility()
        {
            var facility = GetFactory<FacilityFactory>().Create();
            var facilityArea = GetEntityFactory<FacilityArea>().Create(new {Description = "test"});
            var facilitySubArea = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription",
                Area = facilityArea
            });
            var facilityCoordinate = GetEntityFactory<Coordinate>().Create(new { 
                Latitude = (decimal)20.25,
                Longitude = (decimal)-40.50
            });
            _currentUser.IsAdmin = true;
            var model = _viewModelFactory.BuildWithOverrides<AddFacilityFacilityArea>(new {
                FacilityArea = facilityArea.Id,
                FacilitySubArea = facilitySubArea.Id,
                Facility = facility.Id,
                facility.Id,
                Coordinate = facilityCoordinate.Id
            });
            MyAssert.CausesIncrease(
                () => _target.AddFacilityFacilityArea(model),
                () => Session.Get<Facility>(facility.Id).FacilityAreas.Count);
        }
        
        [TestMethod]
        public void TestRemoveFacilityFacilityArea()
        {
            var facility = GetFactory<FacilityFactory>().Create();
            var facilityArea = GetEntityFactory<FacilityArea>().Create(new {Description = "test"});
            var facilitySubArea = GetEntityFactory<FacilitySubArea>().Create(new {
                Description ="SubareaDescription",
                Area = facilityArea
            });
            var facilityFacilityArea = GetEntityFactory<FacilityFacilityArea>().Create(new {
                FacilitySubArea = facilitySubArea,
                FacilityArea = facilityArea,
                Facility = facility
            });

            facility.FacilityAreas.Add(facilityFacilityArea);
            Session.Save(facility);
            var model = _viewModelFactory.BuildWithOverrides<RemoveFacilityFacilityArea>(new {
                facility.Id,
                FacilityFacilityArea = facilityFacilityArea.Id
            });

            MyAssert.CausesDecrease(
                () => _target.RemoveFacilityFacilityArea(model),
                () => Session.Get<FacilityFacilityArea>(facility.Id).Facility.FacilityAreas.Count);
        }

        #endregion

        #region GetSystemDeliveryHistoryForFacility

        [TestMethod]
        public void TestGetSystemDeliveryHistoryForFacility()
        {
            var now = new DateTime(2021, 5, 26);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var facility = GetEntityFactory<Facility>().Create();
            var systemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = _dateTimeProvider.Object.GetCurrentDate().AddMonths(-6).Date, IsValidated = true});
            var systemDeliveryEntryTwo = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = _dateTimeProvider.Object.GetCurrentDate().Date, IsValidated = true});
            var systemDeliveryFacilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {Facility = facility, SystemDeliveryEntry = systemDeliveryEntry});
            var systemDeliveryFacilityEntryTwo = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {Facility = facility, SystemDeliveryEntry = systemDeliveryEntryTwo});

            var model = new SearchFacilitySystemDeliveryHistory {Id = facility.Id};

            var result = _target.GetSystemDeliveryHistoryForFacility(model);

            Assert.AreEqual(2, model.SystemDeliveryHistory.Count);
            Assert.IsTrue(model.SystemDeliveryHistory.Any(x => x.Date == systemDeliveryFacilityEntry.SystemDeliveryEntry.WeekOf));
            Assert.IsTrue(model.SystemDeliveryHistory.Any(x => x.Date == systemDeliveryFacilityEntryTwo.SystemDeliveryEntry.WeekOf));
        }

        #endregion

        #region ByOperatingCenterIdWhereCommunityRightToKnowIsTrue

        [TestMethod]
        public void TestByOperatingCenterIdWhereCommunityRightToKnowIsTrue()
        {
            //Arrange
            _currentUser.IsAdmin = true;
            var opcenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeFacility = GetFactory<FacilityFactory>().Create(new
                { OperatingCenter = opcenter, FacilityStatus = typeof(ActiveFacilityStatusFactory), CommunityRightToKnow = true });
            var activeFacility2 = GetFactory<FacilityFactory>().Create(new
                { OperatingCenter = opcenter, FacilityStatus = typeof(ActiveFacilityStatusFactory), CommunityRightToKnow = false });

            //Act
            var resultActive =
                (CascadingActionResult)_target.GetByOperatingCenterIdAndCommunityRightToKnowIsTrue(opcenter.Id);
            var actualActive = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(1, actualActive.Count() - 1,
                "There should only be one facility returned along with the empty select option");
            Assert.IsTrue(actualActive.Any(x => x.Value == activeFacility.Id.ToString()), "Only the active facility with community right to know must be in the results");
            Assert.IsFalse(actualActive.Any(x => x.Value == activeFacility2.Id.ToString()), "Only the facility with community right to know set to true should be in the results");
        }

        #endregion
    }
}
