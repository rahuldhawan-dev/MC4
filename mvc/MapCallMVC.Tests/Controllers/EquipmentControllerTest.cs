using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Helpers;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using StructureMap;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Conventions;
using Historian.Data.Client.Repositories;
using log4net;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Utilities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Documents;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallMVC.Tests.Controllers
{
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class EquipmentControllerTest : MapCallMvcControllerTestBase<EquipmentController, Equipment, EquipmentRepository>
    {
        #region Setup/Teardown

        private Mock<INotificationService> _notifier;
        private Mock<IRawDataRepository> _rawDataRepo;
        private Mock<ISAPEquipmentRepository> _sapRepository;

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IRawDataRepository>().Use((_rawDataRepo = new Mock<IRawDataRepository>()).Object);
            e.For<ILog>().Mock();
            _notifier = e.For<INotificationService>().Mock();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IEquipmentModelRepository>().Use<EquipmentModelRepository>();
            e.For<IFunctionalLocationRepository>().Use<FunctionalLocationRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IEquipmentCharacteristicFieldRepository>().Use<EquipmentCharacteristicFieldRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<ISensorMeasurementTypeRepository>().Use<SensorMeasurementTypeRepository>();
            e.For<IDocumentService>().Use<InMemoryDocumentService>();
            _sapRepository = e.For<ISAPEquipmentRepository>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var expectedPreReq = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            GetEntityFactory<EquipmentStatus>().Create(new {Description = "In Service"});
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Update action has an extra FormCollection parameter. We should really kill that
            // if it's at all possible.
            options.DoUpdateSingleViewModelParameterCheck = false;
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues[nameof(SearchEquipment.Facility)] = 1 ;
                tester.TestPropertyValues[nameof(SearchEquipment.FacilityFacilityArea)] = 1;
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateEquipment)vm;
                model.RequestedBy = GetEntityFactory<Employee>().Create().Id;
                model.EquipmentType = GetFactory<EquipmentTypeAeratorFactory>().Create().Id;
            };
        }

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.ProductionEquipment;
                a.RequiresRole("~/Equipment/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Equipment/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Equipment/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Equipment/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Equipment/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Equipment/New/", module, RoleActions.Add);
                a.RequiresRole("~/Equipment/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Equipment/Copy/", module, RoleActions.Add);
                a.RequiresRole("~/Equipment/Replace/", module, RoleActions.Add);
                a.RequiresRole("~/Equipment/AddLink/", module, RoleActions.Edit);
                a.RequiresRole("~/Equipment/RemoveLink/", module, RoleActions.Edit);
                a.RequiresRole("~/Equipment/AddSensor/", module, RoleActions.Edit);
                a.RequiresRole("~/Equipment/AddEquipmentMaintenancePlan/", module, RoleActions.Edit);
                a.RequiresRole("~/Equipment/RemoveSensor/", module, RoleActions.Edit);
                a.RequiresRole("~/Equipment/Readings/", module, RoleActions.Read);
                a.RequiresRole("~/Equipment/ScadaReadings/", module, RoleActions.Read);
                a.RequiresLoggedInUserOnly("~/Equipment/GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId");
                a.RequiresLoggedInUserOnly("~/Equipment/GetActiveInServiceSAPEquipmentByFacilityIdsOrEquipmentTypeIds");
                a.RequiresLoggedInUserOnly("~/Equipment/GetActiveInServiceSAPEquipmentWhereNotPresentInPlan");
                a.RequiresLoggedInUserOnly("~/Equipment/ByFacilityIdForEquipmentTypeOfWell");
                a.RequiresLoggedInUserOnly("~/Equipment/GasDetectorsByOperatingCenter/");
                a.RequiresLoggedInUserOnly("~/Equipment/GetCriticalNotes/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByFacilityId/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByFunctionalLocation/");
                a.RequiresLoggedInUserOnly("~/Equipment/EquipmentTypesByFunctionalLocation/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByFacilityFunctionalLocation/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByFacilityIdAndSometimesEquipmentTypeIdAndProductionWorkOrder/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByFacilityIdsForSystemDelivery/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByFacilityIds/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByTownIdForWorkOrders/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByFacilityIdAndIsEligibleForRedTagPermitEquipmentTypes/");
                a.RequiresLoggedInUserOnly("~/Equipment/ByOperatingCenterOnlyPotableWaterTanks/");
            });
        }

        #region ByFacilityIdAndIsEligibleForRedTagPermitEquipmentTypes 

        [TestMethod]
        public void ByFacilityIdAndIsEligibleForRedTagPermitEquipmentPurposes()
        {
            var facility = GetEntityFactory<Facility>().Create();

            var fireAlarm = GetEntityFactory<Equipment>().Create(new {
                Facility = facility, 
                EquipmentType = GetFactory<EquipmentTypeFireSuppressionFactory>().Create()
            });

            var generator = GetEntityFactory<Equipment>().Create(new {
                Facility = facility, 
                EquipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create()
            });

            var actionResult = (CascadingActionResult)_target.ByFacilityIdAndIsEligibleForRedTagPermitEquipmentTypes(facility.Id);
            var equipmentResults = actionResult.GetSelectListItems().ToArray();

            Assert.AreEqual(1, equipmentResults.Count() - 1, "There should only be two items: the empty select and the matching equipment");
            Assert.IsTrue(equipmentResults.Any(x => x.Value == fireAlarm.Id.ToString()), "The matching equipment must exist in the results");
            Assert.IsTrue(equipmentResults.All(x => x.Value != generator.Id.ToString()), "The non-matching equipment should not exist in the results");
        }

        #endregion

        #region GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentPurposeIdReturnsEquipmentWhenFacilityHasValue

        [TestMethod]
        public void TestGetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentPurposeIdReturnsEquipmentWhenFacilityHasValue() 
        {
            //arrange
            var facility = GetEntityFactory<Facility>().Create();
            var equipment1 = GetEntityFactory<Equipment>().Create(new { Facility = facility, SAPEquipmentId = 123 });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { Facility = facility, SAPEquipmentId = 456 });

            //act
            var result = (CascadingActionResult)_target.GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId(facility.Id, null, null);
            var actual = result.GetSelectListItems().ToArray();

            //assert
            Assert.AreEqual(2, actual.Count() - 1); //-1 because it adds -- Select -- list item
            Assert.IsTrue(actual.Any(x => x.Value == equipment1.Id.ToString()));
            Assert.IsTrue(actual.Any(x => x.Value == equipment2.Id.ToString()));
        }

        [TestMethod]
        public void TestGetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentPurposeIdReturnsEquipmentWhenFacilityAndFacilityAreaHasValue()
        {
            //arrange
            var facility = GetEntityFactory<Facility>().Create();
            var facilityArea1 = GetEntityFactory<FacilityFacilityArea>().Create();
            var facilityArea2 = GetEntityFactory<FacilityFacilityArea>().Create();
            var equipment1 = GetEntityFactory<Equipment>().Create(new { Facility = facility, FacilityFacilityArea = facilityArea1, SAPEquipmentId = 123 });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { Facility = facility, FacilityFacilityArea = facilityArea2, SAPEquipmentId = 456 });

            //act
            var result = (CascadingActionResult)_target.GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId(facility.Id, facilityArea1.Id, null);

            //assert
            Assert.IsTrue(result.GetSelectListItems().Any(x => x.Value == equipment1.Id.ToString()));
            Assert.IsFalse(result.GetSelectListItems().Any(x => x.Value == equipment2.Id.ToString()));

            //act
            result = (CascadingActionResult)_target.GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId(facility.Id, facilityArea2.Id, null);

            //assert
            Assert.IsFalse(result.GetSelectListItems().Any(x => x.Value == equipment1.Id.ToString()));
            Assert.IsTrue(result.GetSelectListItems().Any(x => x.Value == equipment2.Id.ToString()));
        }

        [TestMethod]
        public void TestGetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentPurposeIdReturnsEquipmentWhenFacilityAndEquipmentPurposeHasValue()
        {
            //arrange
            var facility = GetEntityFactory<Facility>().Create();
            var equipmentType = GetFactory<EquipmentTypeFactory>().CreateAll();
            var equipment1 = GetEntityFactory<Equipment>().Create(new { Facility = facility, EquipmentType = equipmentType[0], SAPEquipmentId = 123 });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { Facility = facility, EquipmentType = equipmentType[1], SAPEquipmentId = 456 });

            //act
            var result = (CascadingActionResult)_target.GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId(facility.Id, null, equipmentType[0].Id);

            //assert
            Assert.IsTrue(result.GetSelectListItems().Any(x => x.Value == equipment1.Id.ToString()));
            Assert.IsFalse(result.GetSelectListItems().Any(x => x.Value == equipment2.Id.ToString()));

            //act
            result = (CascadingActionResult)_target.GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId(facility.Id, null, equipmentType[1].Id);

            //assert
            Assert.IsFalse(result.GetSelectListItems().Any(x => x.Value == equipment1.Id.ToString()));
            Assert.IsTrue(result.GetSelectListItems().Any(x => x.Value == equipment2.Id.ToString()));
        }

        [TestMethod]
        public void TestGetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentPurposeIdReturnsEquipmentWhenFacilityAndFacilityAreaAndEquipmentPurposeHasValue()
        {
            //arrange
            var facility = GetEntityFactory<Facility>().Create();
            var facilityArea1 = GetEntityFactory<FacilityFacilityArea>().Create();
            var facilityArea2 = GetEntityFactory<FacilityFacilityArea>().Create();
            var equipmentType = GetFactory<EquipmentTypeFactory>().CreateAll();
            var equipment1 = GetEntityFactory<Equipment>().Create(new { Facility = facility, FacilityFacilityArea = facilityArea1, EquipmentType = equipmentType[0], SAPEquipmentId = 123 });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { Facility = facility, FacilityFacilityArea = facilityArea2, EquipmentType = equipmentType[1], SAPEquipmentId = 456 });

            //act
            var result = (CascadingActionResult)_target.GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId(facility.Id, facilityArea1.Id, equipmentType[0].Id);

            //assert
            Assert.IsTrue(result.GetSelectListItems().Any(x => x.Value == equipment1.Id.ToString()));
            Assert.IsFalse(result.GetSelectListItems().Any(x => x.Value == equipment2.Id.ToString()));

            //act
            result = (CascadingActionResult)_target.GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId(facility.Id, facilityArea2.Id, equipmentType[1].Id);

            //assert
            Assert.IsFalse(result.GetSelectListItems().Any(x => x.Value == equipment1.Id.ToString()));
            Assert.IsTrue(result.GetSelectListItems().Any(x => x.Value == equipment2.Id.ToString()));
        }

        #endregion

        #region ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId

        [TestMethod]
        public void TestByFacilityIdAndOrEquipmentPurposeIdReturnsEquipmentForFacilityWhenOnlyFacilityIdHasValue()
        {
            //Arrange
            var goodFacility = GetEntityFactory<Facility>().Create();
            var badFacility = GetEntityFactory<Facility>().Create();
            var goodEquipment = GetEntityFactory<Equipment>().Create(new { Facility = goodFacility });
            var badEquipment = GetEntityFactory<Equipment>().Create(new { Facility = badFacility });
            //Act
            var resultActive = (CascadingActionResult)_target.ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId(goodFacility.Id, null, null);
            var actual = resultActive.GetSelectListItems().ToArray();
            //Assert
            Assert.AreEqual(1, actual.Count() - 1, "There should only be two items: the empty select and the matching equipment");
            Assert.IsTrue(actual.Any(x => x.Value == goodEquipment.Id.ToString()), "The matching equipment must exist in the results");
            Assert.IsFalse(actual.Any(x => x.Value == badEquipment.Id.ToString()), "The non-matching equipment should not exist in the results");
        }

        [TestMethod]
        public void TestGetActiveInServiceSAPEquipmentReturnsEquipmentForFacilityWhenOnlyFacilityIdHasValue()
        {
            //Arrange
            var goodFacility = GetEntityFactory<Facility>().Create();
            var goodFacility2 = GetEntityFactory<Facility>().Create();
            var badFacility = GetEntityFactory<Facility>().Create();
            var equipmentType = GetFactory<EquipmentTypeFactory>().CreateAll();
            var goodEquipment = GetEntityFactory<Equipment>().Create(new { Facility = goodFacility, EquipmentType = equipmentType[0], SAPEquipmentId = 123 });
            var goodEquipment2 = GetEntityFactory<Equipment>().Create(new { Facility = goodFacility2, EquipmentType = equipmentType[1], SAPEquipmentId = 456 });
            var badEquipment = GetEntityFactory<Equipment>().Create(new { Facility = badFacility, EquipmentType = equipmentType[3], SAPEquipmentId = 789 });
            //Act 
            var resultActive = (CascadingActionResult)_target.GetActiveInServiceSAPEquipmentByFacilityIdsOrEquipmentTypeIds(new int[] { goodFacility.Id, goodFacility2.Id }, new int[] { goodEquipment2.EquipmentType.Id, goodEquipment.EquipmentType.Id });
            var actual = (IEnumerable<dynamic>)resultActive.Data;
            //Assert
            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(actual.Any(x => x.Id == goodEquipment.Id));
            Assert.IsTrue(actual.Any(x => x.Id == goodEquipment2.Id));
        }

        [TestMethod]
        public void TestGetActiveInServiceSAPEquipmentWhereNotPresentInPlanOnlyReturnsItemsNotPresentInPlan()
        {
            //Arrange
            var goodFacility = GetEntityFactory<Facility>().Create();
            var equipmentType = GetFactory<EquipmentTypeFactory>().CreateAll();
            var goodMaintenancePlan = GetEntityFactory<MaintenancePlan>().Create(new {
                Facility = goodFacility,
            });
            var goodEquipment = GetEntityFactory<Equipment>().Create(new {
                Facility = goodFacility, 
                EquipmentType = equipmentType[0], 
                SAPEquipmentId = 123
            });
            var goodEquipment2 = GetEntityFactory<Equipment>().Create(new {
                Facility = goodFacility,
                EquipmentType = equipmentType[1], 
                SAPEquipmentId = 456
            });
            var equipmentMaintenancePlan = GetEntityFactory<EquipmentMaintenancePlan>().Create(new {
                MaintenancePlan = goodMaintenancePlan,
                Equipment = goodEquipment
            });

            goodEquipment.MaintenancePlans = new List<EquipmentMaintenancePlan> { equipmentMaintenancePlan };

            //Act
            var resultActive = (CascadingActionResult)_target.GetActiveInServiceSAPEquipmentWhereNotPresentInPlan(new int[] { goodFacility.Id }, new int[] { goodEquipment2.EquipmentType.Id, goodEquipment.EquipmentType.Id }, goodMaintenancePlan.Id);
            var actual = (IEnumerable<dynamic>)resultActive.Data;
            
            //Assert
            Assert.AreEqual(1, actual.Count());
            Assert.IsTrue(actual.Single().Id == goodEquipment2.Id);
        }

        [TestMethod]
        public void TestByFacilityIdAndOrSAPEquipmentPurposeIdReturnsEquipmentWhenOnlyEquipmentIdHasValue()
        {
            // This test needs to return Equipment that have the same EquipmentType that matches 
            // the equipmentTypeId parameter passed into ByFacilityIdAndOrEquipmentTypeId.
            //Arrange
            // Create two EquipmentTypes 
            var type1 = GetEntityFactory<EquipmentType>().Create();
            var type2 = GetEntityFactory<EquipmentType>().Create();
            var badFacility = GetEntityFactory<Facility>().Create();
            // Create two Equipment that have different EquipmentTypes so that we can guarantee
            // that filtering by EquipmentType is done correctly.
            var goodEquipment = GetEntityFactory<Equipment>().Create(new { EquipmentType = type1 });
            var badEquipment = GetEntityFactory<Equipment>().Create(new { EquipmentType = type2 });
            var badEquipment2 = GetEntityFactory<Equipment>().Create(new { Facility = badFacility });
            //Act
            // Call the method by only passing along the EquipmentType Id value that we want to filter by.
            var resultActive = (CascadingActionResult)_target.ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId(null, null, type1.Id);
            var actualActive = resultActive.GetSelectListItems().ToArray();
            //Assert
            Assert.AreEqual(2, actualActive.Count() - 1, "There should only be two items: the empty select and the matching equipment");
            Assert.IsTrue(actualActive.Any(x => x.Value == goodEquipment.Id.ToString()), "Only the associated equipment must be in the results");
            Assert.IsFalse(actualActive.Any(x => x.Value == badEquipment2.Id.ToString()), "Only the associated equipment must be in the results");
        }

        [TestMethod]
        public void TestByFacilityIdAndOrEquipmentPurposeIdReturnsEquipmentForFacilityWhenOnlyEquipmentIdHasValue()
        {
            //Arrange
            var goodFacility = GetEntityFactory<Facility>().Create();
            var badFacility = GetEntityFactory<Facility>().Create();
            var type1 = GetEntityFactory<EquipmentType>().Create();
            var type2 = GetEntityFactory<EquipmentType>().Create();
            var goodEquipment = GetEntityFactory<Equipment>().Create(new { Facility = goodFacility, EquipmentType = type1 });
            var badEquipment = GetEntityFactory<Equipment>().Create(new { Facility = badFacility });

            //Act
            var resultActive = (CascadingActionResult)_target.ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId(goodFacility.Id, null, type1.Id);
            var actual = resultActive.GetSelectListItems().ToArray();
            //Assert
            Assert.AreEqual(1, actual.Count() - 1, "There should only be two items: the empty select and the matching equipment");
            Assert.IsTrue(actual.Any(x => x.Value == goodEquipment.Id.ToString()), "The matching equipment must exist in the results");
            Assert.IsFalse(actual.Any(x => x.Value == badEquipment.Id.ToString()), "The non-matching equipment should not exist in the results");
        }

        [TestMethod]
        public void TestByFacilityIdAndOrEquipmentPurposeIdReturnsNothingWhenNothingSelected()
        {
            //Arrange
            var badFacility = GetEntityFactory<Facility>().Create();
            var type1 = GetEntityFactory<EquipmentType>().Create();
            var badEquipment = GetEntityFactory<Equipment>().Create(new { });

            //Act
            var resultActive = (CascadingActionResult)_target.ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId(null, null, null);
            var actual = resultActive.GetSelectListItems().ToArray();
            //Assert
            Assert.IsFalse(actual.Any(), "no results are expected.");
            Assert.IsFalse(actual.Any(x => x.Value == badEquipment.Id.ToString()), "The non-matching equipment should not exist in the results");
        }

        [TestMethod]
        public void TestByFacilityIdAndOrEquipmentPurposeIdReturnsEquipmentForFacilityWhenFacilityFacilityAreaIdHasValue()
        {
            //Arrange
            var goodFacility = GetEntityFactory<Facility>().Create();
            var badFacility = GetEntityFactory<Facility>().Create();
            var facilityArea = GetEntityFactory<FacilityFacilityArea>().Create();
            var goodEquipment = GetEntityFactory<Equipment>().Create(new { Facility = goodFacility, FacilityFacilityArea = facilityArea });
            var badEquipment = GetEntityFactory<Equipment>().Create(new { Facility = badFacility });

            //Act
            var resultActive = (CascadingActionResult)_target.ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId(goodFacility.Id, facilityArea.Id, null);
            var actual = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(1, actual.Count() - 1, "There should only be two items: the empty select and the matching equipment");
            Assert.IsTrue(actual.Any(x => x.Value == goodEquipment.Id.ToString()), "The matching equipment must exist in the results");
            Assert.IsFalse(actual.Any(x => x.Value == badEquipment.Id.ToString()), "The non-matching equipment should not exist in the results");
        }

        [TestMethod]
        public void TestByFacilityIdAndOrEquipmentPurposeIdReturnsEquipmentForFacilityWhenAllParamsHaveValue()
        {
            //Arrange
            var facility1 = GetEntityFactory<Facility>().Create();
            var facilityArea1 = GetEntityFactory<FacilityFacilityArea>().Create();
            var type1 = GetEntityFactory<EquipmentType>().Create();
            var goodEquipment = GetEntityFactory<Equipment>().Create(new { Facility = facility1, FacilityFacilityArea = facilityArea1, EquipmentType = type1 });

            var facility2 = GetEntityFactory<Facility>().Create();
            var facilityArea2 = GetEntityFactory<FacilityFacilityArea>().Create();
            var type2 = GetEntityFactory<EquipmentType>().Create();
            var badEquipment = GetEntityFactory<Equipment>().Create(new { Facility = facility2, FacilityFacilityArea = facilityArea2, EquipmentType = type2 });

            //Act
            var resultActive = (CascadingActionResult)_target.ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId(facility1.Id, facilityArea1.Id, type1.Id);
            var actual = resultActive.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(1, actual.Count() - 1, "There should only be two items: the empty select and the matching equipment");
            Assert.IsTrue(actual.Any(x => x.Value == goodEquipment.Id.ToString()), "The matching equipment must exist in the results");
            Assert.IsFalse(actual.Any(x => x.Value == badEquipment.Id.ToString()), "The non-matching equipment should not exist in the results");
        }

        #endregion

        #region GetByFacilityIdAndOrEquipment

        [TestMethod]
        public void TestByFacilityIdsForSystemDeliveryReturnsEquipmentWithFloMeterEquipmentPurpose()
        {
            // Arrange 
            var facility = GetEntityFactory<Facility>().Create();
            var facility2 = GetEntityFactory<Facility>().Create();
            var equipmentType = GetFactory<EquipmentTypeFlowMeterFactory>().Create();
            var equipmentSubCategory = GetFactory<PurchasedWaterEquipmentSubCategoryFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {EquipmentSubCategory = equipmentSubCategory});
            var badEquipmentType = GetEntityFactory<EquipmentType>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new {EquipmentType = equipmentType, Facility = facility, EquipmentPurpose = equipmentPurpose});
            var equipment2 = GetEntityFactory<Equipment>().Create(new {EquipmentType = equipmentType, Facility = facility2, EquipmentPurpose = equipmentPurpose});
            var badEquipment = GetEntityFactory<Equipment>().Create(new {EquipmentType = badEquipmentType, Facility = facility2});

            // Act
            var result = (CascadingActionResult)_target.ByFacilityIdsForSystemDelivery(new[]{facility.Id, facility2.Id});
            var actual = result.GetSelectListItems().ToArray();

            // Assert
            Assert.IsTrue(actual.Any());
            Assert.AreEqual(actual.Count() - 1, 2);
            Assert.IsTrue(actual.Any(x => x.Value == equipment.Id.ToString()));
            Assert.IsTrue(actual.Any(x => x.Value == equipment2.Id.ToString()));
        }

        [TestMethod]
        public void TestDuplicateEquipmentsWithSameIdDifferentLinks()
        {
            // This should return only One Equipment with two Links
            var state = GetEntityFactory<State>().Create(new {Id = 1});
            var operating = GetEntityFactory<OperatingCenter>().Create(new {State = state});
            var equipment = GetEntityFactory<Equipment>().Create(overrides: new {OperatingCenter = operating});
            var linkType = GetEntityFactory<LinkType>().Create();
            var equipmentUrl = GetEntityFactory<EquipmentLink>().Create(new {Equipment = equipment, Url = "foo", LinkType = linkType});
            equipment.Links.Add(equipmentUrl);
            var equipmentUrl1 = GetEntityFactory<EquipmentLink>().Create(new {Equipment = equipment, Url = "foo1", LinkType = linkType});
            equipment.Links.Add(equipmentUrl1);

            var search = new SearchEquipment {
                State = 1, //NJ
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Count().Equals(1));
            Assert.IsTrue(search.Results.First().Links.Count().Equals(2));
            Assert.IsFalse(search.Results.Count().Equals(2));
        }

        #endregion

        #region ByFacilityIdForEquipmentTypeOfWell

        [TestMethod]
        public void ByFacilityIdForEquipmentTypeOfWell()
        {
            var equipmentTypeWell = GetFactory<EquipmentTypeWellFactory>().Create();
            var equipmentTypeGenerator = GetFactory<EquipmentTypeGeneratorFactory>().Create();

            var facility = GetEntityFactory<Facility>().Create();

            var equipmentWithWell = GetEntityFactory<Equipment>().Create(new { Facility = facility, EquipmentType = equipmentTypeWell });
            var equipmentWithGenerator = GetEntityFactory<Equipment>().Create(new { Facility = facility, EquipmentType = equipmentTypeGenerator });

            var resultActive = (CascadingActionResult)_target.ByFacilityIdForEquipmentTypeOfWell(facility.Id);
            var actual = resultActive.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1, "There should only be two items: the empty select and the matching equipment");
            Assert.IsTrue(actual.Any(x => x.Value == equipmentWithWell.Id.ToString()), "The matching equipment must exist in the results");
            Assert.IsTrue(actual.All(x => x.Value != equipmentWithGenerator.Id.ToString()), "The non-matching equipment should not exist in the results");
        }

        #endregion

        #region GetActiveByOperatingCenterId

        [TestMethod]
        public void TestShow404sIfEquipmentDoesNotExist()
        {
            Assert.IsNotNull(_target.Show(666) as HttpNotFoundResult);
        }

        [TestMethod]
        public void TestShowSetsSensorsViewData()
        {
            var equipment = GetFactory<EquipmentFactory>().Create();
            equipment.Characteristics.Add(new EquipmentCharacteristic());

            Assert.IsNull(_target.ViewData["Sensors"]);
            _target.Show(equipment.Id);
            Assert.IsNotNull(_target.ViewData["Sensors"]);
        }

        [TestMethod]
        public void TestShowShowsFacilityArcFlashMessage()
        {
            var arcFlashStatuses = GetEntityFactory<ArcFlashStatus>().CreateList(7);
            foreach (var arcFlashStatus in arcFlashStatuses)
            {
                var facility = GetFactory<FacilityFactory>().Create();
                var study = GetEntityFactory<ArcFlashStudy>().Create(new { Facility = facility, DateLabelsApplied = DateTime.Now.AddYears(-5), ArcFlashStatus = arcFlashStatus });
                var equipment = GetFactory<EquipmentFactory>().Create(new {Facility = facility});
                Session.Save(facility);
                Session.Save(equipment);
                Session.Save(study);
                Session.Flush();
                Session.Evict(facility);
                Session.Evict(equipment);
                Session.Evict(study);

                var result = _target.Show(equipment.Id) as ViewResult;
                if (arcFlashStatus.Id == ArcFlashStatus.Indices.N_A)
                    Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]);
                else
                {
                    Assert.AreEqual(string.Format(EquipmentController.ARC_FLASH_STATUS_MESSAGE, arcFlashStatus.Description), 
                        ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).First());
                }
                _target.TempData = null;
            }
        }

        [TestMethod]
        public void TestShowJsonReturnsWithProperJson()
        {
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var entity = GetEntityFactory<Equipment>().Create(new { Coordinate = coordinate});

            InitializeControllerAndRequest("~/Equipment/Show" + entity.Id + ".json");

            var result = _target.Show(entity.Id) as JsonResult;
            var resultData = (dynamic)result.Data;

            Assert.IsNotNull(result);
            Assert.AreEqual(coordinate.Latitude, resultData.Latitude);
            Assert.AreEqual(coordinate.Longitude, resultData.Longitude);
        }

        [TestMethod]
        public void TestByOperatingCenterOnlyPotableWaterTanksReturnsEquipmentWithPotableWaterTanks()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var equipmentTypes = GetFactory<EquipmentTypeFactory>().CreateAll();
            var equipmentTank = GetFactory<EquipmentFactory>().Create(new {
                OperatingCenter = operatingCenter,
                EquipmentType = equipmentTypes.SingleOrDefault(x => x.Id == EquipmentType.Indices.TNK_WPOT)
            });
            var equipmentChemTank = GetFactory<EquipmentFactory>().Create(new {
                OperatingCenter = operatingCenter,
                EquipmentType = equipmentTypes.SingleOrDefault(x => x.Id == EquipmentType.Indices.TNK_CHEM)
            });
            var equipmentNotTank = GetFactory<EquipmentFactory>().Create(new {
                OperatingCenter = operatingCenter,
                EquipmentType = equipmentTypes.SingleOrDefault(x => x.Id == EquipmentType.Indices.RTU_PLC)
            });

            var result = (CascadingActionResult)_target.ByOperatingCenterOnlyPotableWaterTanks(operatingCenter.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(2, actual.Count(), "There should only be two items: the empty select and the equipment with Potable Water Tank type");
            Assert.IsTrue(actual.Any(x => x.Value == equipmentTank.Id.ToString()), "The Potable Water Tank equipment must exist in the results");
            Assert.IsTrue(actual.All(x => x.Value != equipmentChemTank.Id.ToString()), "Other Tank equipment should not exist in the results");
            Assert.IsTrue(actual.All(x => x.Value != equipmentNotTank.Id.ToString()), "Non-Tank equipment should not exist in the results");
        }

        [TestMethod]
        public void TestShowSetsViewModelDataOnModelFound()
        {
            var entity = GetEntityFactory<Equipment>().Create();

            var result = _target.Show(entity.Id);
            var search = (bool)_target.ViewData["HasLinkedEquipment"];

            Assert.IsFalse(search);
        }
        
        #endregion

        #region New/Create

        [TestMethod]
        public void TestNewClearsModelState()
        {
            _target.ModelState.AddModelError("Oops", "my bad");
            _target.New(new CreateEquipment(_container));
            Assert.IsFalse(_target.ModelState.Any());
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // override because this redirects to the Edit action for some reason.
            var equipment = _viewModelFactory.Build<CreateEquipment, Equipment>(GetFactory<EquipmentFactory>().Build(new
            {
                Facility = GetFactory<FacilityFactory>().Create(),
                EquipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create()
            }));
            RedirectToRouteResult result = null;

            MyAssert.CausesIncrease(
                () => result = _target.Create(equipment) as RedirectToRouteResult,
                () => Repository.GetAll().Count());

            Assert.AreEqual(result.RouteValues["Action"], "Edit");
        }

        [TestMethod]
        public void TestCreateSendsNotificationEmail()
        {
            var facility = GetEntityFactory<Facility>().Create();
            var functionalLocation = "functional location";
            var model = _viewModelFactory.Build<CreateEquipment, Equipment>( GetEntityFactory<Equipment>().Build(new {
                EquipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create(),
                Facility = facility,
                EquipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create(),
                RequestedBy = GetFactory<ActiveEmployeeFactory>().Create(),
                EquipmentManufacturer = GetFactory<EquipmentManufacturerFactory>().Create(),
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(),
                EquipmentStatus = GetEntityFactory<EquipmentStatus>().Create(),
                FunctionalLocation = functionalLocation
            }));

            // If this fails, the rest of the test will fail. Sanity check.
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.IsNotNull(resultArgs, "Notifier may not have been called.");
            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(entity.Facility.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(EquipmentController.ROLE, resultArgs.Module);
            Assert.AreEqual(EquipmentController.CREATED_NOTIFICATION, resultArgs.Purpose);
            Assert.AreEqual("http://localhost/Equipment/Show/" + entity.Id, entity.RecordUrl);
            Assert.IsNull(resultArgs.Subject);
        }

        [TestMethod]
        public void TestCreateAddsPrereqAutomaticallyWhenCertainEquipmentPurposesAreSelected()
        {
            //ARRANGE
            var triggeringEquipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });

           var model = _viewModelFactory.Build<CreateEquipment, Equipment>(GetEntityFactory<Equipment>()
               .BuildWithConcreteDependencies(new
                {
                    OperatingCenter = opCntr,
                    EquipmentType = triggeringEquipmentType
                }));
            Assert.IsTrue(model.Prerequisites.IsEmpty());

            //ACT
            _target.Create(model);
            var equipment = Repository.Find(model.Id);

            //ASSERT
            Assert.AreEqual(equipment.ProductionPrerequisites[0].Id, 1);
            Assert.AreEqual(equipment.ProductionPrerequisites[0].Description, "Has Lockout Requirement");
        }
        
        #region SAP Syncronization

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsErrorCodeUponFailure()
        {
            var equipmentType = GetFactory<EquipmentTypeRTUFactory>().Create();
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var productionDepartment = GetEntityFactory<Department>().CreateList(3)[Department.Indices.PRODUCTION - 1];
            var facility = GetEntityFactory<Facility>().Create(new { Town = town, OperatingCenter = opCntr, Department = productionDepartment });
            var model = _viewModelFactory.Build<CreateEquipment, Equipment>( GetEntityFactory<Equipment>().BuildWithConcreteDependencies(new {
                OperatingCenter = opCntr, Facility = facility, EquipmentType = equipmentType, FunctionalLocation = "2"
            }));
            Session.Flush();

            _target.Create(model);

            // FAILS BECAUSE WE DID NOT INJECT AN SAPRepository
            var equipment = Repository.Find(model.Id);
            Assert.IsTrue(equipment.SAPErrorCode.StartsWith(EquipmentController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestCreateDoesntFailValidationForFunctionalLocationWhenSAPEnabledIsFalseAndFunctionalLocationIsNull()
        {
            var equipmentType = GetFactory<EquipmentTypeRTUFactory>().Create();
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var productionDepartment = GetEntityFactory<Department>().CreateList(3)[Department.Indices.PRODUCTION - 1];
            var facility = GetEntityFactory<Facility>().Create(new { Town = town, OperatingCenter = opCntr, Department = productionDepartment });
            var model = _viewModelFactory.Build<CreateEquipment, Equipment>(GetEntityFactory<Equipment>().BuildWithConcreteDependencies(new {
                OperatingCenter = opCntr,
                Facility = facility,
                EquipmentType = equipmentType
            }));
            Session.Flush();

            _target.Create(model);

            var equipment = Repository.Find(model.Id);
            Assert.IsNotNull(equipment);
        }

        [TestMethod]
        public void TestCreateWillFailValidationForFunctionalLocationWhenSAPEnabledIsTrueAndFunctionalLocationIsNull()
        {
            var equipmentType = GetFactory<EquipmentTypeRTUFactory>().Create();
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var productionDepartment = GetEntityFactory<Department>().CreateList(3)[Department.Indices.PRODUCTION - 1];
            var facility = GetEntityFactory<Facility>().Create(new { Town = town, OperatingCenter = opCntr, Department = productionDepartment });
            var model = _viewModelFactory.Build<CreateEquipment, Equipment>(GetEntityFactory<Equipment>().BuildWithConcreteDependencies(new {
                OperatingCenter = opCntr,
                Facility = facility,
                EquipmentType = equipmentType
            }));
            Session.Flush();

            _target.RunModelValidation(model);
            _target.Create(model);

            var equipment = Repository.Find(model.Id);
            Assert.IsNull(equipment);
        }

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsSAPEquipmentIdAndNoErrorMessage()
        {
            //ARRANGE
            var productionAssetType = GetEntityFactory<ProductionAssetType>().Create(new {
                Description = "Hydrant",
                Id = 1
            });
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create(new {
                Id = EquipmentType.Indices.HYD,
                ProductionAssetType = productionAssetType,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "theRefNumber"
            });
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var sapEquipment = new SAPEquipment { SAPEquipmentNumber = "123456789", SAPErrorCode = string.Empty };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);
            var productionDepartment = GetEntityFactory<Department>().CreateList(3)[Department.Indices.PRODUCTION - 1];
            var facility = GetEntityFactory<Facility>().Create(new { Town = town, OperatingCenter = opCntr, Department = productionDepartment });

            var model = _viewModelFactory.Build<CreateEquipment, Equipment>( GetEntityFactory<Equipment>().BuildWithConcreteDependencies(new {
                OperatingCenter = opCntr, Facility = facility, EquipmentType = equipmentType, FunctionalLocation = "2"
            }));

            //ACT
            _target.Create(model);
            var equipment = Repository.Find(model.Id);

            //ASSERT
            Assert.AreEqual(string.Empty, equipment.SAPErrorCode);
            Assert.AreEqual(int.Parse(sapEquipment.SAPEquipmentNumber), equipment.SAPEquipmentId);
        }

        #endregion

        #endregion

        #region Copy

        [TestMethod]
        public void TestCopyCreatesCopyAndRedirectsToEdit()
        {
            var equipmentStatuses = GetFactory<EquipmentStatusFactory>().CreateList(7);

            foreach (var status in equipmentStatuses)
            {
                var equipment = GetFactory<EquipmentFactory>().Create(new {
                    EquipmentStatus = status
                });

                var result = _target.Copy(equipment.Id);

                if (EquipmentStatus.CanBeCopiedStatuses.Contains(equipment.EquipmentStatus.Id))
                {
                    MvcAssert.RedirectsToRoute(result, "Edit", new {id = equipment.Id + 1});
                }
                else
                {
                    Assert.IsNotNull(result as HttpNotFoundResult);
                }
            }
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var eq0 = GetFactory<EquipmentFactory>().Create();
            var eq1 = GetFactory<EquipmentFactory>().Create();
            var search = new SearchEquipment();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(eq0.Id, "Id");
                helper.AreEqual(eq1.Id, "Id", 1);
                helper.AreEqual(eq0.HasAtLeastOneWellTest, "Has Well Tests");
                helper.AreEqual(eq1.HasAtLeastOneWellTest, "Has Well Tests");
            }
        }

        [TestMethod]
        public void TestIndexXLSExportsExcelForCompliances()
        {
            var eq0 = GetEntityFactory<Equipment>().Create(new {
                HasProcessSafetyManagement = true,
                HasCompanyRequirement = true,
                HasRegulatoryRequirement = true,
                HasOshaRequirement = true,
                OtherCompliance = true,
                OtherComplianceReason = "test"
            });
            var eq1 = GetEntityFactory<Equipment>().Create();

            // this test likes to fluke in TC, comparing the entity DateInstalled values to the ones on the
            // spreadsheet end up off by less than a minute.  on the chance that this is due to something
            // being rounded on one side but not the other, let's trim the seconds and milliseconds from the
            // value being rendered and compared.  UNDO THIS IF THIS TEST FAILS IN TC!!!
            eq0.DateInstalled = eq0.DateInstalled.Value.BeginningOfMinute();
            eq1.DateInstalled = eq1.DateInstalled.Value.BeginningOfMinute();

            Session.Save(eq0);
            Session.Save(eq1);
            Session.Flush();

            var search = new SearchEquipment();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(true, "Process Safety Management");
                helper.AreEqual(true, "Company Requirement");
                helper.AreEqual(true, "Environmental / Water Quality Regulatory Requirement");
                helper.AreEqual(true, "OSHA Requirement");
                helper.AreEqual(true, "Other");
                helper.AreEqual("test", "Other Compliance Reason");
                helper.AreEqual(eq0.DateInstalled, "DateInstalled");
                helper.AreEqual(eq1.DateInstalled, "DateInstalled", 1);
            }
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForHasOpenLockoutFormsSearchType()
        {
            // This should return only Equipment with incomplete lockouts
            //SELECT 1 FROM LockOutForms lock WHERE lock.EquipmentID = EquipmentID AND lock.ReturnedToServiceDateTime is null)
            var eq0Open = GetFactory<EquipmentFactory>().Create();
            var eq1Open = GetFactory<EquipmentFactory>().Create();
            var eq1Open1Closed = GetFactory<EquipmentFactory>().Create();
            var lockoutThatIsOpen = GetEntityFactory<LockoutForm>().Create(new { Equipment = eq1Open1Closed });
            var lockoutThatIsOpenJr = GetEntityFactory<LockoutForm>().Create(new { Equipment = eq1Open });
            var lockoutThatIsClosed = GetEntityFactory<LockoutForm>().Create(new { Equipment = eq1Open1Closed , ReturnedToServiceDateTime = DateTime.Now });
            var lockoutThatIsClosedJr = GetEntityFactory<LockoutForm>().Create(new { Equipment = eq0Open, ReturnedToServiceDateTime = DateTime.Now });

            var search = new SearchEquipment
            {
                HasOpenLockoutForms = true
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(eq1Open));
            Assert.IsTrue(search.Results.Contains(eq1Open1Closed));
            Assert.IsFalse(search.Results.Contains(eq0Open));
        }

        [TestMethod]
        public void TestIndexXLSExportsExcelWhenTheresIsASapEquipmentPurposeChosen()
        {
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create();
            var eq0 = GetFactory<EquipmentFactory>().Create(new { EquipmentType = equipmentType });
            var eq1 = GetFactory<EquipmentFactory>().Create(new { EquipmentType = equipmentType });
            var charFieldTheCat0 = GetEntityFactory<EquipmentCharacteristicField>().Create(new { EquipmentType = equipmentType, Description = "Field 0" });
            var charFieldTheCat1 = GetEntityFactory<EquipmentCharacteristicField>().Create(new { EquipmentType = equipmentType, Description = "Field 1" });
            var char0 = GetEntityFactory<EquipmentCharacteristic>().Create(new { Equipment = eq0, Field = charFieldTheCat0, Value = "Value 0" });
            var char1 = GetEntityFactory<EquipmentCharacteristic>().Create(new { Equipment = eq1, Field = charFieldTheCat1, Value = "Value 1" });

            // The factories aren't correctly adding references on parents when they create these objects.
            // ex: The EquipmentCharacteristicFields aren't being added to EquipmentType.Characteristicfields.
            // ex: EquipmentCharacteristics aren't being added to EquipmentCharacteristicField.EquipmentCharacteristics or Equipment.Characteristics.
            // These all need to be Session.Refresh()ed individually or we can clear the session entirely so everything is requeried.
            Session.Clear();

            var search = new SearchEquipment();
            search.EquipmentType = new[] { equipmentType.Id };
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(eq0.Id, "Id");
                helper.AreEqual(eq1.Id, "Id", 1);
                helper.AreEqual("Value 0", "Field 0");
                helper.AreEqual("Value 1", "Field 1", 1);

                // These headers should still have the same formatting as whatever's in their View attribute.
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerForRequest("~/Equipment/Index.map");
            var equipmentPurposes = GetFactory<EquipmentPurposeFactory>().CreateList(2);
            var good = GetFactory<EquipmentFactory>().Create(new { EquipmentPurpose = equipmentPurposes[0] });
            var bad = GetFactory<EquipmentFactory>().Create(new { EquipmentPurpose = equipmentPurposes[1] });
            var model = new SearchEquipment
            {
                EquipmentPurpose = new[] {equipmentPurposes[0].Id }
            };
            var result = (MapResult)_target.Index(model);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
        }

        [TestMethod]
        public void TestIndexReturnsFrag()
        {
            var eq1 = GetEntityFactory<Equipment>().Create();
            // Same Equipment Purpose and IsReplacement set to true
            var eq2 = GetEntityFactory<Equipment>().Create(new {
                IsReplacement = true,
                eq1.EquipmentPurpose
            });
            // Same Equipment Purpose and Replacement Id
            var eq3 = GetEntityFactory<Equipment>().Create(new {
                eq1.EquipmentPurpose
            });
            eq1.ReplacedEquipment = eq3;
            // Different Equipment Purpose and IsReplacement set to true
            var eq4 = GetEntityFactory<Equipment>().Create(new { IsReplacement = true });

            eq1.Facility.Equipment.Add(eq2);
            eq1.Facility.Equipment.Add(eq3);
            eq1.Facility.Equipment.Add(eq4);

            Repository.Save(eq1);

            var search = new SearchEquipment {
                Facility = eq1.Facility.Id,
                EquipmentPurpose = new[] { eq1.EquipmentPurpose.Id },
                NotEqualEntityId = eq1.Id, 
                OriginalEquipmentId = eq1.ReplacedEquipment.Id
            };
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.FRAGMENT;

            var result = _target.Index(search);

            MvcAssert.IsPartialView(result);
            MvcAssert.IsViewNamed(result, "_Equipments");
            Assert.IsTrue(search.Results.Contains(eq2));
            Assert.IsTrue(search.Results.Contains(eq3));
            Assert.AreEqual(2, search.Results.Count());
        }

        [TestMethod]
        public void TestIndexReturnsNoResultsFrag()
        {
            var search = new SearchEquipment();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.FRAGMENT;

            var result = _target.Index(search);

            MvcAssert.IsPartialView(result);
            MvcAssert.IsViewNamed(result, "_NoResults");
        }

        #endregion

        #region EquipmentTypesByFunctionalLocation

        [TestMethod]
        public void TestEquipmentTypesByFunctionalLocationReturnsByFunctionalLocation()
        {
            var funcLocation = "functionalLocation";
            var funcLocationTwo = "funcLocation";
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create(2);
            var equipment1 = GetFactory<EquipmentFactory>().CreateList(1, new { FunctionalLocation = funcLocation, EquipmentType = equipmentType});
            var equipment2 = GetFactory<EquipmentFactory>().CreateList(1, new { FunctionalLocation = funcLocationTwo, EquipmentType = equipmentType });

            var result = (CascadingActionResult)_target.EquipmentTypesByFunctionalLocation(funcLocation);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1); // -1 accounts for the select here
            Assert.AreEqual(equipment1.Count(), actual.Count() - 1);
        }

        [TestMethod]
        public void TestEquipmentTypesByFacilityFunctionalLocationReturnsByFacilityFunctionalLocation()
        {
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create(2);
            var functionalLocation = "Test-Location";
            var facility1 = GetFactory<FacilityFactory>().Create();
            var facility2 = GetFactory<FacilityFactory>().Create(new { FunctionalLocation = functionalLocation });
            var equipment1 = GetFactory<EquipmentFactory>().CreateList(1, new { Facility = facility1, EquipmentType = equipmentType });
            var equipment2 = GetFactory<EquipmentFactory>().CreateList(1, new { Facility = facility2, EquipmentType = equipmentType, FunctionalLocation = functionalLocation });

            var result = (CascadingActionResult)_target.ByFacilityFunctionalLocation(facility2.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1); // -1 accounts for the select here
            Assert.AreEqual(equipment2.Count(), actual.Count() - 1);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            // override needed due to second parameter in Update action
            var eq = GetFactory<EquipmentFactory>().Create();
            var expected = "NJSB-1-EQID-2";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(eq, new
            {
                Description = expected
            }), new FormCollection());

            Assert.AreEqual(expected, Session.Get<Equipment>(eq.Id).Description);
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            // override needed due to second parameter in Update action
            var eq = GetFactory<EquipmentFactory>().Create();
            var expected = "NJSB-1-EQID-2";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(eq, new
            {
                Description = expected
            }), new FormCollection()) as RedirectToRouteResult;

            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            Assert.Inconclusive("Test me if I'm not tested already.");
        }

        [TestMethod]
        public override void TestUpdateReturnsNotFoundIfRecordBeingUpdatedDoesNotExist()
        {
            Assert.IsNotNull(_target.Update(new EditEquipment(_container) { Id = 666 }, new FormCollection()) as HttpNotFoundResult);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenEquipmentIsMovedIntoService()
        {
            var requestedBy = GetFactory<ActiveEmployeeFactory>().Create();
            var functionalLocation = "functional location";
            var eq = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create(),
                EquipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create(),
                RequestedBy = requestedBy,
                SAPEquipmentId = 1234,
                EquipmentManufacturer = GetFactory<EquipmentManufacturerFactory>().Create(),
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(),
                EquipmentStatus = GetFactory<PendingEquipmentStatusFactory>().Create(),
                FunctionalLocation = functionalLocation
            });
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(eq, new {
                EquipmentStatus = EquipmentStatus.Indices.IN_SERVICE
            });

            // If this fails, the rest of the test will fail. Sanity check.
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model, new FormCollection());

            Assert.IsNotNull(resultArgs, "Notifier may not have been called.");
            Assert.AreSame(eq, resultArgs.Data);
            Assert.AreEqual(eq.Facility.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(requestedBy.EmailAddress, resultArgs.Address);
            Assert.AreEqual(EquipmentController.ROLE, resultArgs.Module);
            Assert.AreEqual(EquipmentController.IN_SERVICE_NOTIFICATION, resultArgs.Purpose);
            Assert.AreEqual("http://localhost/Equipment/Show/" + eq.Id, eq.RecordUrl);
            Assert.IsNull(resultArgs.Subject);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenEquipmentIsMovedIntoRetirement()
        {
            var retiredEquipmentStatus = GetFactory<RetiredEquipmentStatusFactory>().Create();
            var requestedBy = GetFactory<ActiveEmployeeFactory>().Create();
            var functionalLocation = "functional location";
            var eq = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create(),
                EquipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create(),
                RequestedBy = requestedBy,
                SAPEquipmentId = 1234,
                EquipmentManufacturer = GetFactory<EquipmentManufacturerFactory>().Create(),
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(), 
                DateRetired = DateTime.Now,
                FunctionalLocation = functionalLocation
            });
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(eq, new {
                EquipmentStatus = EquipmentStatus.Indices.RETIRED
            });

            // If this fails, the rest of the test will fail. Sanity check.
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Update(model, new FormCollection());

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.AtLeast(2));
            Assert.IsNotNull(resultArgs, "Notifier may not have been called.");
            Assert.AreSame(eq, resultArgs.Data);
            Assert.AreEqual(eq.Facility.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(requestedBy.EmailAddress, resultArgs.Address);
            Assert.AreEqual(EquipmentController.ROLE, resultArgs.Module);
            Assert.AreEqual(EquipmentController.RETIRED_NOTIFICATION, resultArgs.Purpose);
            Assert.AreEqual("http://localhost/Equipment/Show/" + eq.Id, eq.RecordUrl);
            Assert.IsNull(resultArgs.Subject);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenEquipmentIsMovedIntoPendingRetirement()
        {
            var pendingRetirementStatusFactory = GetFactory<PendingRetirementEquipmentStatusFactory>().Create();
            var requestedBy = GetFactory<ActiveEmployeeFactory>().Create();
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var functionalLocation = "functional location";

            var eq = GetEntityFactory<Equipment>().Create(new {
                EquipmentStatus = GetFactory<InServiceEquipmentStatusFactory>().Create(),
                EquipmentType = equipmentType,
                EquipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create(),
                RequestedBy = requestedBy,
                SAPEquipmentId = 1234,
                EquipmentManufacturer = GetFactory<EquipmentManufacturerFactory>().Create(),
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(),
                FunctionalLocation = functionalLocation
            });
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(eq, new {
                EquipmentStatus = EquipmentStatus.Indices.PENDING_RETIREMENT
            });

            // If this fails, the rest of the test will fail. Sanity check.
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Update(model, new FormCollection());

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Exactly(1));
            Assert.IsNotNull(resultArgs, "Notifier may not have been called.");
            Assert.AreSame(eq, resultArgs.Data);
            Assert.AreEqual(eq.Facility.OperatingCenter.Id, resultArgs.OperatingCenterId);
            //Assert.AreEqual(requestedBy.EmailAddress, resultArgs.Address);
            Assert.AreEqual(EquipmentController.ROLE, resultArgs.Module);
            Assert.AreEqual(EquipmentController.PENDING_RETIREMENT_NOTIFICATION, resultArgs.Purpose);
            Assert.AreEqual("http://localhost/Equipment/Show/" + eq.Id, eq.RecordUrl);
            Assert.IsNull(resultArgs.Subject);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenEquipmentStatusChangedToFieldInstalled()
        {
            var requestedBy = GetFactory<ActiveEmployeeFactory>().Create();
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var fi = GetFactory<FieldInstalledEquipmentStatusFactory>().Create();
            var functionalLocation = "functional location";
            var eq = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = equipmentType,
                EquipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create(),
                RequestedBy = requestedBy,
                SAPEquipmentId = 1234,
                EquipmentManufacturer = GetFactory<EquipmentManufacturerFactory>().Create(),
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(), 
                EquipmentStatus = GetFactory<PendingEquipmentStatusFactory>().Create(),
                FunctionalLocation = functionalLocation
            });
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(eq, new {
                EquipmentStatus = fi.Id
            });

            // If this fails, the rest of the test will fail. Sanity check.
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Update(model, new FormCollection());

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Exactly(1));
            Assert.IsNotNull(resultArgs, "Notifier may not have been called.");
            Assert.AreSame(eq, resultArgs.Data);
            Assert.AreEqual(eq.Facility.OperatingCenter.Id, resultArgs.OperatingCenterId);
            //Assert.AreEqual(requestedBy.EmailAddress, resultArgs.Address);
            Assert.AreEqual(EquipmentController.ROLE, resultArgs.Module);
            Assert.AreEqual(EquipmentController.FIELD_INSTALLED_NOTIFICATION, resultArgs.Purpose);
            Assert.AreEqual("http://localhost/Equipment/Show/" + eq.Id, eq.RecordUrl);
            Assert.IsNull(resultArgs.Subject);
        }

        #region SAP

        [TestMethod]
        public void TestUpdateCallsSAPRepositorySaveAndRecordsErrorCodeUponFailure()
        {
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var productionDepartment = GetEntityFactory<Department>().CreateList(3)[Department.Indices.PRODUCTION-1];
            var facility = GetEntityFactory<Facility>().Create(new {Town = town, OperatingCenter = opCntr, Department = productionDepartment});
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            
            var entity = GetEntityFactory<Equipment>().Create(new { OperatingCenter = opCntr, Facility = facility, EquipmentType = equipmentType, FunctionalLocation = "2" });
            var model = _viewModelFactory.Build<EditEquipment, Equipment>( entity);

            _target.Update(model, new FormCollection());

            // FAILS BECAUSE WE DID NOT INJECT AN SAPRepository
            var equipment = Repository.Find(model.Id);
            Assert.IsNotNull(equipment.SAPErrorCode);
            Assert.IsTrue(equipment.SAPErrorCode.StartsWith(EquipmentController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestUpdateCallsSAPRepositorySaveAndDoesNotModifySapEquipmentIdOrError()
        {
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var productionDepartment = GetEntityFactory<Department>().CreateList(3)[Department.Indices.PRODUCTION - 1];
            var facility = GetEntityFactory<Facility>().Create(new { Town = town, OperatingCenter = opCntr, Department = productionDepartment });
            var productionAssetType = GetEntityFactory<ProductionAssetType>().Create(new {
                Description = "Hydrant",
                Id = 1
            });
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create(new {
                Id = EquipmentType.Indices.HYD,
                ProductionAssetType = productionAssetType,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "theRefNumber"
            });
            var sapEquipmentId = 420311;
            var entity = GetEntityFactory<Equipment>().Create(new { OperatingCenter = opCntr, Facility = facility, EquipmentType = equipmentType, SAPEquipmentId = sapEquipmentId });
            var model = _viewModelFactory.Build<EditEquipment, Equipment>( entity);
            var sapEquipment = new SAPEquipment { SAPErrorCode = string.Empty };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);

            _target.Update(model, new FormCollection());
            var equipment = Repository.Find(model.Id);

            Assert.IsTrue(String.IsNullOrWhiteSpace(equipment.SAPErrorCode), equipment.SAPErrorCode);
            Assert.AreEqual(sapEquipmentId, equipment.SAPEquipmentId);
        }

        [TestMethod]
        public void TestUpdateCallsSendsNotificationWithCorrectAreaInUrl()
        {
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var productionDepartment = GetEntityFactory<Department>().CreateList(3)[Department.Indices.PRODUCTION - 1];
            var facility = GetEntityFactory<Facility>().Create(new { Town = town, OperatingCenter = opCntr, Department = productionDepartment });
            var productionAssetType = GetEntityFactory<ProductionAssetType>().Create(new {
                Description = "Hydrant",
                Id = 1
            });
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create(new {
                Id = EquipmentType.Indices.HYD,
                ProductionAssetType = productionAssetType,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "theRefNumber"
            });
            var sapEquipmentId = 420311;
            var entity = GetEntityFactory<Equipment>().Create(new { OperatingCenter = opCntr, Facility = facility, EquipmentType = equipmentType, SAPEquipmentId = sapEquipmentId, FunctionalLocation = "2" });
            var model = _viewModelFactory.Build<EditEquipment, Equipment>( entity);
            var sapEquipment = new SAPEquipment { SAPErrorCode = "This failed so send an email." };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);

            _target.Update(model, new FormCollection());
            var equipment = Repository.Find(model.Id);

            _notifier.Verify(x => x.Notify(It.Is<NotifierArgs>(args => args.Data.GetType().GetProperty("RecordUrl").GetValue(args.Data,null).ToString() == "http://localhost/Equipment/Show/1")));
        }

        [TestMethod]
        public void TestUpdateEffectivelyCreatesInSAPWhenSAPEquipmentIdEqualsZero()
        {
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var productionDepartment = GetEntityFactory<Department>().CreateList(3)[Department.Indices.PRODUCTION - 1];
            var facility = GetEntityFactory<Facility>().Create(new { Town = town, OperatingCenter = opCntr, Department = productionDepartment });
            var productionAssetType = GetEntityFactory<ProductionAssetType>().Create(new {
                Description = "Hydrant",
                Id = 1
            });
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create(new {
                Id = EquipmentType.Indices.HYD,
                ProductionAssetType = productionAssetType,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "theRefNumber"
            });
            var sapEquipmentId = "420311";
            var sapEquipment = new SAPEquipment { SAPErrorCode = string.Empty, SAPEquipmentNumber = sapEquipmentId};
            var entity = GetEntityFactory<Equipment>().Create(new { OperatingCenter = opCntr, Facility = facility, EquipmentType = equipmentType, SAPEquipmentId = 0, FunctionalLocation = "2" });
            var model = _viewModelFactory.Build<EditEquipment, Equipment>( entity);
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);

            _target.Update(model, new FormCollection());
            var equipment = Repository.Find(model.Id);

            Assert.IsTrue(String.IsNullOrWhiteSpace(equipment.SAPErrorCode), equipment.SAPErrorCode);
            Assert.AreEqual(int.Parse(sapEquipmentId), equipment.SAPEquipmentId);
        }

        #endregion

        #endregion

        #region Links

        [TestMethod]
        public void TestAddLinkAddsLinkToEquipment()
        {
            var linkType = GetEntityFactory<LinkType>().Create();
            var equipment = GetEntityFactory<Equipment>().Create();
            var model = _viewModelFactory.BuildWithOverrides<AddEquipmentLink>(new {
                LinkType = linkType.Id, Url = "not a link", equipment.Id
            });

            MyAssert.CausesIncrease(
                () => _target.AddLink(model),
                () => Session.Get<Equipment>(equipment.Id).Links.Count());
        }

        [TestMethod]
        public void TestRemoveLinkRemovesLinkFromEquipment()
        {
            var linkType = GetEntityFactory<LinkType>().Create();
            var equipment = GetEntityFactory<Equipment>().Create();
            var equipmentLink = GetEntityFactory<EquipmentLink>().Create(new {
                Equipment = equipment,
                LinkType = linkType,
                Url = "not a link"
            });
            equipment.Links.Add(equipmentLink);
            Session.Save(equipmentLink);
            var model = _viewModelFactory.BuildWithOverrides<RemoveEquipmentLink>(new {
                equipment.Id,
                EquipmentLink = equipmentLink.Id
            });

            MyAssert.CausesDecrease(
                () => _target.RemoveLink(model),
                () => Session.Get<Equipment>(equipment.Id).Links.Count());
        }

        #endregion

        #region RemoveSensor

        [TestMethod]
        public void TestRemoveSensorRedirectsToShowPageIfModelStateIsInvalid()
        {
            var equip = GetFactory<EquipmentFactory>().Create();
            var model = new RemoveEquipmentSensor(_container) {
                Id = equip.Id,
                Sensor = 0
            };
            _target.ModelState.AddModelError("Oops", "Oops");
            var result = _target.RemoveSensor(model);
            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "Equipment", id = equip.Id });
        }

        [TestMethod]
        public void TestRemoveSensorDoesNotRemoveSensorIfModelStateIsInvalid()
        {
            var equip = GetFactory<EquipmentFactory>().Create();
            var sensor = GetEntityFactory<Sensor>().Create();

            var equipmentSensor = GetEntityFactory<EquipmentSensor>().Create(new
            {
                Equipment = equip,
                Sensor = sensor
            });

            var model = new RemoveEquipmentSensor
           (_container) {
                Id = equip.Id,
                Sensor = sensor.Id
            };
            _target.ModelState.AddModelError("Oops", "Oops");
            var result = _target.RemoveSensor(model);
            Assert.IsTrue(equip.Sensors.Any());

            _target.ModelState.Clear();

            result = _target.RemoveSensor(model);
            Assert.IsFalse(equip.Sensors.Any());
        }

        [TestMethod]
        public void TestRemoveSensorRedirectsToEquipmentShowPageWhenSuccessful()
        {
            var equip = GetFactory<EquipmentFactory>().Create();
            var sensor = GetEntityFactory<Sensor>().Create();
            var equipmentSensor = GetEntityFactory<EquipmentSensor>().Create(new
            {
                Equipment = equip,
                Sensor = sensor
            });

            var model = new RemoveEquipmentSensor
           (_container) {
                Id = equip.Id,
                Sensor = sensor.Id
            };

            var result = _target.RemoveSensor(model);
            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "Equipment", id = equip.Id });
        }

        #endregion

        #region AddSensor

        [TestMethod]
        public void TestAddSensorRedirectsToShowPageIfModelStateIsInvalid()
        {
            var equip = GetFactory<EquipmentFactory>().Create();
            var model = new AddEquipmentSensor(_container)
            {
                Id = equip.Id,
                Sensor = 0
            };
            _target.ModelState.AddModelError("Oops", "Oops");
            var result = _target.AddSensor(model);
            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "Equipment", id = equip.Id });
        }

        [TestMethod]
        public void TestAddSensorDoesNotAddSensorIfModelStateIsInvalid()
        {
            var equip = GetFactory<EquipmentFactory>().Create();
            var model = new AddEquipmentSensor(_container)
            {
                Id = equip.Id,
                Sensor = 0
            };
            _target.ModelState.AddModelError("Oops", "Oops");
            var result = _target.AddSensor(model);
            Assert.IsFalse(equip.Sensors.Any());

            _target.ModelState.Clear();
            var sensor = GetEntityFactory<Sensor>().Create();
            model.Sensor = sensor.Id;

            result = _target.AddSensor(model);
            Assert.IsTrue(equip.Sensors.Any());
        }

        [TestMethod]
        public void TestAddSensorRedirectsToEquipmentShowPageWhenSuccessful()
        {
            var equip = GetFactory<EquipmentFactory>().Create();
            var sensor = GetEntityFactory<Sensor>().Create();

            var model = new AddEquipmentSensor(_container)
            {
                Id = equip.Id,
                Sensor = sensor.Id
            };

            var result = _target.AddSensor(model);
            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "Equipment", id = equip.Id });
        }

        #endregion

        #region Maintenance Plan

        [TestMethod]
        public void TestAddEquipmentMaintenancePlanRedirectsToEquipmentShowPageWhenSuccessful()
        {
            var eq = GetFactory<EquipmentFactory>().Create();
            var plan = GetEntityFactory<MaintenancePlan>().Create();
            var model = _viewModelFactory.BuildWithOverrides<AddSingleEquipmentMaintenancePlan, Equipment>(eq,
                new {MaintenancePlan = plan.Id});

            var result = _target.AddEquipmentMaintenancePlan(model);

            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "Equipment", id = eq.Id });
        }

        #endregion

        #region Readings

        [TestMethod]
        public void TestReadingsSetsSensorsViewDataRegardlessOfEquipmentBeingInvalid()
        {
            Assert.IsNull(_target.ViewData["Sensors"]);
            _target.ModelState.AddModelError("Oops", "Oops");
            _target.Readings(_container.GetInstance<SearchEquipmentReadings>());
            Assert.IsNotNull(_target.ViewData["Sensors"]);
        }

        [TestMethod]
        public void TestReadingsActionSetsModelsReadingsPropertyToExpectedSensorReadingsReadingsReadingsReadingsReadings()
        {
            var searchDate = DateTime.Now.BeginningOfDay();
            var kw = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();

            var equipment = GetFactory<EquipmentFactory>().Create();
            var sensorOne = GetFactory<SensorFactory>().Create(new { MeasurementType = kw });
            // Make some readings
            var startOfDayReading = GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = searchDate.Date });
            var endOfDayReading = GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = searchDate.Date.AddDays(1).AddSeconds(-1) });
            var nowReading = GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = DateTime.Now });
            var badDateReading = GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = searchDate.Date.AddDays(-1) });
            var tomorrowBadReading = GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = searchDate.Date.AddDays(1) });
            // Make sure sensor is associated with equipment.
            GetFactory<EquipmentSensorFactory>().Create(new { Equipment = equipment, Sensor = sensorOne });

            var model = _container.GetInstance<SearchEquipmentReadings>();
            model.Id = equipment.Id;
            model.StartDate = searchDate;
            model.EndDate = searchDate.AddDays(1);
            model.Interval = ReadingGroupType.Hourly;
            model.Sensors = new[] { sensorOne.Id };

            _target.Readings(model);

            var result = model.Readings.ToArray();

            Assert.IsFalse(result.Contains(badDateReading));
            Assert.IsFalse(result.Contains(tomorrowBadReading));
            Assert.AreSame(endOfDayReading, result[0]);
            Assert.AreSame(nowReading, result[1]);
            Assert.AreSame(startOfDayReading, result[2]);
        }

        [TestMethod]
        public void TestReadingsActionSetsUpExpectedChartValues()
        {
            var kw = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();
            var searchDate = DateTime.Now;

            var equipment = GetFactory<EquipmentFactory>().Create();
            var sensorOne = GetFactory<SensorFactory>().Create(new { MeasurementType = kw });
            // Make some readings
            var startOfDayReading = GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = searchDate.Date });
            var endOfDayReading = GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = searchDate.Date.AddDays(1).AddSeconds(-1) });
            var nowReading = GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = searchDate });
            // Make sure sensor is associated with equipment.
            GetFactory<EquipmentSensorFactory>().Create(new { Equipment = equipment, Sensor = sensorOne });

            var model = _container.GetInstance<SearchEquipmentReadings>();
            model.Id = equipment.Id;
            model.StartDate = searchDate;
            model.EndDate = searchDate.AddDays(1);
            model.Interval = ReadingGroupType.Hourly;
            model.Sensors = new[] { sensorOne.Id };

            _target.Readings(model);

            Assert.IsNotNull(_target.ViewData["Chart"]);
            var chart = (ChartBuilder<DateTime, double>)_target.ViewData["Chart"];
            Assert.AreEqual(ChartIntervalType.Hourly, chart.Interval);
            Assert.AreEqual(ChartSortType.LowToHigh, chart.SortType);
            Assert.AreEqual((double)0, chart.YMinValue);
            Assert.AreEqual("kWh", chart.YAxisLabel);
            Assert.AreEqual("Hourly Summary of Sensor Readings", chart.Title);
        }

        [TestMethod]
        public void TestReadingsActionSumsUpKilowattUsageIntoKilowattHoursForChartForIndividualSensors()
        {
            var kw = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();
            var searchDate = new DateTime(2014, 1, 1, 0, 0, 0);

            var equipment = GetFactory<EquipmentFactory>().Create();
            var sensorOne = GetFactory<SensorFactory>().Create(new { Name = "Sensor One", MeasurementType = kw });
            var sensorTwo = GetFactory<SensorFactory>().Create(new { Name = "Sensor Two", MeasurementType = kw });
            // Make some readings
            GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = new DateTime(2014, 1, 1, 0, 0, 0), ScaledData = 1 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = new DateTime(2014, 1, 1, 0, 15, 0), ScaledData = 2 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = new DateTime(2014, 1, 1, 0, 30, 0), ScaledData = 3 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensorOne, DateTimeStamp = new DateTime(2014, 1, 1, 0, 45, 0), ScaledData = 4 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensorTwo, DateTimeStamp = new DateTime(2014, 1, 1, 0, 0, 0), ScaledData = 5 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensorTwo, DateTimeStamp = new DateTime(2014, 1, 1, 0, 15, 0), ScaledData = 6 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensorTwo, DateTimeStamp = new DateTime(2014, 1, 1, 0, 30, 0), ScaledData = 7 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensorTwo, DateTimeStamp = new DateTime(2014, 1, 1, 0, 45, 0), ScaledData = 8 });
            GetFactory<ReadingFactory>().Create(new { Sensor = sensorTwo, DateTimeStamp = new DateTime(2014, 1, 1, 1, 00, 0), ScaledData = 1234 });
            // Make sure sensor is associated with equipment.
            GetFactory<EquipmentSensorFactory>().Create(new { Equipment = equipment, Sensor = sensorOne });
            GetFactory<EquipmentSensorFactory>().Create(new { Equipment = equipment, Sensor = sensorTwo });

            var model = _container.GetInstance<SearchEquipmentReadings>();
            model.Id = equipment.Id;
            model.StartDate = searchDate;
            model.EndDate = searchDate.AddDays(1);
            model.Sensors = new[] { sensorOne.Id, sensorTwo.Id };

            _target.Readings(model);

            Assert.IsNotNull(_target.ViewData["Chart"]);
            var chart = (ChartBuilder<DateTime, double>)_target.ViewData["Chart"];

            var seriesOne = chart.Series.Single(x => x.Name == "Sensor One");
            Assert.AreEqual(0.25d, seriesOne[searchDate]);

            var seriesTwo = chart.Series.Single(x => x.Name == "Sensor Two");
            Assert.AreEqual(1.25d, seriesTwo[searchDate]);
            Assert.AreEqual(308.5d, seriesTwo[searchDate.AddHours(1)]);
        }

        #endregion

        #region Lookup
        [TestMethod]
        public void TestEquipmentDropDownLookupForNewSetsActiveOC()
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
        #endregion

        #region GasDetectorsByOperatingCenter

        [TestMethod]
        public void TestGasDetectorsByOperatingCenterOnlyReturnsEquipmentThatAreGasDetectors()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = opc });
            var gasEquipment = GetFactory<GasMonitorEquipmentFactory>().Create(new { Facility = facility });
            var badEquipment = GetFactory<EquipmentFactory>().Create(new { Facility = facility });

            var results = (CascadingActionResult)_target.GasDetectorsByOperatingCenter(opc.Id);
            var resultData = (IEnumerable<EquipmentDisplayItem>)results.Data;
            Assert.AreEqual(gasEquipment.Id, resultData.Single().Id);
        }

        #endregion
    }
}
