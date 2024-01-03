using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.SAP.Controllers;
using MapCallMVC.Areas.SAP.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.SAP.Controllers
{
    [TestClass]
    public class SAPMaintenancePlanControllerTest : MapCallMvcControllerTestBase<SAPMaintenancePlanController, ProductionWorkOrder>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/SAP/SAPMaintenancePlan/Show", role, RoleActions.Read);
                a.RequiresRole("~/SAP/SAPMaintenancePlan/Search", role, RoleActions.Read);
                a.RequiresRole("~/SAP/SAPMaintenancePlan/Index", role, RoleActions.Read);
                a.RequiresRole("~/SAP/SAPMaintenancePlan/RemoveEquipmentFromMaintenancePlan", role, RoleActions.Read);
                a.RequiresRole("~/SAP/SAPMaintenancePlan/SapFixDate", role, RoleActions.Read);
                a.RequiresRole("~/SAP/SAPMaintenancePlan/SapManualCall", role, RoleActions.Read);
                a.RequiresRole("~/SAP/SAPMaintenancePlan/SapSkipCall", role, RoleActions.Read);
                a.RequiresRole("~/SAP/SAPMaintenancePlan/AddEquipmentToMaintenancePlan", role, RoleActions.Read);
            });
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            Assert.Inconclusive("No one wrote tests for this. Automatic tests will not work here.");
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            // override needed because not ISearchSet
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            // override needed because not ISearchSet
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            Assert.Inconclusive("I either don't do this intentionally or I need this to be implemented.");
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // override needed because of SAP repo. 

            //wtf
            var sapRepo = new Mock<ISAPMaintenancePlanLookupRepository>();
            _container.Inject(sapRepo.Object);
            var planningPlantRepo = new Mock<IRepository<PlanningPlant>>();
            _container.Inject(planningPlantRepo.Object);

            var operatingCenter = new OperatingCenter { Id = 1 };
            var planningPlant = new PlanningPlant {Code = "P111", OperatingCenter = operatingCenter};
            var planningPlants = new List<PlanningPlant>();
            planningPlants.Add(planningPlant);
            planningPlantRepo.Setup(x => x.Where(It.IsAny<Expression<Func<PlanningPlant,bool>>>()))
                .Returns(new List<PlanningPlant>().AsQueryable);

            var results = new SAPMaintenancePlanLookupCollection();
            results.Items.Add(new SAPMaintenancePlanLookup() { SAPErrorCode = "Success"});

            var search = new SearchSAPMaintenancePlan {MaintenancePlan = "700000096201", PlanningPlant = new [] {planningPlant.Id.ToString()}, OperatingCenter = operatingCenter.Id };
            sapRepo.Setup(x => x.Search(It.IsAny<SAPMaintenancePlanLookup>())).Returns(results);
            var result = _target.Show(search);

            Assert.IsNotNull(result);
            MvcAssert.IsViewNamed(result,"Show");
        }

        [TestMethod]
        public void TestRemoveEquipmentFromMaintenancePlanCallsSAPRepoAndPassesBlankFunctionalLocation()
        {
            var sapRepo = new Mock<ISAPMaintenancePlanLookupRepository>();
            _container.Inject(sapRepo.Object);
            var equipmentRepo = new Mock<IEquipmentRepository>();
            _container.Inject(equipmentRepo.Object);

            var equipment = GetEntityFactory<Equipment>().Create(new{FunctionalLocation = "lol functional", SAPEquipmentId = 123});

            equipmentRepo.Setup(x => x.Find(equipment.Id)).Returns(equipment);

            var remove = new RemoveEquipmentFromMaintenancePlan {
                Equipment = equipment.SAPEquipmentId.ToString(),
                FunctionalLocation = "",
                MapCallEquipmentId = equipment.Id,
                MaintenancePlan = "700000096201",
                MaintenanceItem = "123"
            };

            var results = new SAPMaintenancePlanUpdateCollection();
            results.Items.Add(new SAPMaintenancePlanUpdate() { SAPErrorCode = "Success"});
            sapRepo.Setup(x => x.Save(It.IsAny<SAPMaintenancePlanUpdate>())).Returns(results);

            var result = _target.RemoveEquipmentFromMaintenancePlan(remove);

            Assert.IsNotNull(result);
            MvcAssert.RedirectsToRoute(result, "Equipment", "Show", new { area = "", id = equipment.Id});
            sapRepo.Verify(x => x.Save(It.Is<SAPMaintenancePlanUpdate>(u => u.SapAddRemoveItem.First().FunctionalLocation == equipment.FunctionalLocation)), Times.Once);

        }

        [TestMethod]
        public void TestRemoveEquipmentFromMaintenancePlanCallsSAPRepoAndPassesAFunctionalLocation()
        {
            var sapRepo = new Mock<ISAPMaintenancePlanLookupRepository>();
            _container.Inject(sapRepo.Object);
            var equipmentRepo = new Mock<IEquipmentRepository>();
            _container.Inject(equipmentRepo.Object);

            var equipment = GetEntityFactory<Equipment>().Create(new{FunctionalLocation = "lol functional", SAPEquipmentId = 123});

            equipmentRepo.Setup(x => x.Find(equipment.Id)).Returns(equipment);

            var remove = new RemoveEquipmentFromMaintenancePlan {
                Equipment = equipment.SAPEquipmentId.ToString(),
                FunctionalLocation = "Random Functional Location",
                MapCallEquipmentId = equipment.Id,
                MaintenancePlan = "700000096201",
                MaintenanceItem = "123"
            };

            var results = new SAPMaintenancePlanUpdateCollection();
            results.Items.Add(new SAPMaintenancePlanUpdate() { SAPErrorCode = "Success"});
            sapRepo.Setup(x => x.Save(It.IsAny<SAPMaintenancePlanUpdate>())).Returns(results);

            var result = _target.RemoveEquipmentFromMaintenancePlan(remove);

            Assert.IsNotNull(result);
            MvcAssert.RedirectsToRoute(result, "Equipment", "Show", new { area = "", id = equipment.Id});
            sapRepo.Verify(x => x.Save(It.Is<SAPMaintenancePlanUpdate>(u => u.SapAddRemoveItem.First().FunctionalLocation == equipment.FunctionalLocation)), Times.Once);

        }
    }
}
