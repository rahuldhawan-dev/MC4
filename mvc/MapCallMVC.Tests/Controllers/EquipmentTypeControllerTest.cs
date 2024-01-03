using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EquipmentTypeControllerTest : MapCallMvcControllerTestBase<EquipmentTypeController, EquipmentType>
    {
        #region ByFacilityId

        [TestMethod]
        public void TestByFacilityIdReturnsACascadingActionResultWithFacilityEquipmentType()
        {
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create();
            var facility = GetFactory<FacilityFactory>().Create();
            var equipment = GetFactory<EquipmentFactory>().Create(new{ Facility = facility, EquipmentType = equipmentType });
            facility.Equipment.Add(equipment);

            var result = (CascadingActionResult)_target.ByFacilityId(facility.Id);
            var data = (IEnumerable<EquipmentType>)result.Data;
            Assert.AreSame(equipmentType, data.Single());
        }

        [TestMethod]
        public void TestByFacilityIdReturnsACascadingActionResultWithEmptyDataWhenTheFacilityIsNull()
        {
            var result = (CascadingActionResult)_target.ByFacilityId(0);

            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void TestByFacilityIdReturnsACascadingActionResultWithEmptyDataWhenTheFacilityDoesNotHaveEquipment()
        {
            var facility = GetFactory<FacilityFactory>().Create();

            var result = (CascadingActionResult)_target.ByFacilityId(0);

            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void TestByFacilityIdReturnsACascadingActionResultWithEmptyEquipmentDataWhenNoProductionWorkOrder()
        {
            var facility = GetFactory<FacilityFactory>().Create();

            var result = (CascadingActionResult)_target.ByFacilityIdAndSometimesProductionWorkOrder(facility.Id, null);

            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void TestByFacilityIdsReturnsACascadingActionResultWithFacilityEquipmentType()
        {
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create();
            var facilities = GetFactory<FacilityFactory>().CreateArray();
            var facilityIds = new List<int>();
            foreach (var facility in facilities)
            {
                facilityIds.Add(facility.Id);
                var equipment = GetFactory<EquipmentFactory>().Create(new{ Facility = facility, EquipmentType = equipmentType });
                facility.Equipment.Add(equipment);
            }
            var result = (CascadingActionResult)_target.ByFacilityIds(facilityIds.ToArray());
            var data = (IEnumerable<EquipmentType>)result.Data;

            Assert.AreEqual(equipmentType.Id, data.Single().Id);
            Assert.AreEqual(equipmentType.Description, data.Single().Description);
        }

        [TestMethod]
        public void TestByFacilityIdWithProductionWorkOrderReturnsACascadingActionResultWithFacilityEquipmentType()
        {
            var equipmentType = GetFactory<EquipmentTypeFactory>().CreateAll();
            var equipmentTypeTwo = GetFactory<EquipmentTypeFactory>().Create();
            var facility = GetEntityFactory<Facility>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new { Facility = facility, EquipmentPurpose = new EquipmentPurpose { Abbreviation = "TEST", EquipmentType = equipmentType[0] }, EquipmentType = equipmentType[0], SAPEquipmentId = 123 });
            var productionWorkOrderEquipment = new ProductionWorkOrderEquipment();
            productionWorkOrderEquipment.Equipment = equipment;
            facility.Equipment.Add(equipment);
            var planningPlant = GetEntityFactory<PlanningPlant>().Create();
            var functionalLocation = GetEntityFactory<FunctionalLocation>().Create();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();

            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = operatingCenter,
                Facility = facility,
                Equipment = equipment
            });

            pwo.Equipments = new HashSet<ProductionWorkOrderEquipment> {
                productionWorkOrderEquipment
            };

            var result = (CascadingActionResult)_target.ByFacilityIdAndSometimesProductionWorkOrder(facility.Id, pwo.Id);
            var data = (IEnumerable<EquipmentType>)result.Data;

            Assert.AreEqual(equipmentType[0].Id, data.Single().Id);
            Assert.AreEqual(equipmentType[0].Description, data.Single().Description);
        }

        [TestMethod]
        public void TestByFacilityIdsReturnsACascadingActionResultWithEmptyDataWhenTheFacilityIsNull()
        {
            var facilityIds = new[] {0, 1, 2};
            
            var result = (CascadingActionResult)_target.ByFacilityIds(facilityIds);

            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void TestByFacilityIdsReturnsACascadingActionResultWithEmptyDataWhenTheFacilityDoesNotHaveEquipment()
        {
            var facilities = GetFactory<FacilityFactory>().CreateArray();

            var result = (CascadingActionResult)_target.ByFacilityIds(facilities.Select(facility => facility.Id).ToArray());

            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(0, actual.Count());
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditThrowsInvalidOperationException()
        {
            MyAssert.Throws<NotImplementedException>(() => _target.Edit());
        }

        [TestMethod]
        public override void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            // noop override because this Edit method only exists for CurrentUserCanEdit checks.
        }

        [TestMethod]
        public override void TestEditReturnsEditViewWithEditViewModel()
        {
            // noop override because this Edit method only exists for CurrentUserCanEdit checks.
        }

        #endregion

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var ent = GetFactory<EquipmentTypeGeneratorFactory>().Create();
                Session.Flush();
                return ent;
            };
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.ProductionEquipment;
            Authorization.Assert(a => {
                a.RequiresRole("~/EquipmentType/Show/", role);
                a.RequiresRole("~/EquipmentType/Index/", role);
                a.RequiresRole("~/EquipmentType/Edit/", role, RoleActions.Edit);

                a.RequiresLoggedInUserOnly("~/EquipmentType/ByFacilityId/");
                a.RequiresLoggedInUserOnly("~/EquipmentType/ByFacilityIds/");
                a.RequiresLoggedInUserOnly("~/EquipmentType/ByFacilityIdAndSometimesProductionWorkOrder/");
                a.RequiresLoggedInUserOnly("~/EquipmentType/ByEquipmentGroupId/");

                a.RequiresSiteAdminUser("~/EquipmentType/RemoveMeasurementPoint/");
            });
        }
    }
}
