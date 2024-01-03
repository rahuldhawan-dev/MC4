using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class RegulatoryComplianceReportControllerTest : MapCallMvcControllerTestBase<RegulatoryComplianceReportController, ProductionWorkOrder, ProductionWorkOrderRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/RegulatoryComplianceReport/Search/");
                a.RequiresLoggedInUserOnly("~/Reports/RegulatoryComplianceReport/Index/");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
             var state = GetEntityFactory<State>().Create();
            var opCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var pp1 = GetEntityFactory<PlanningPlant>().Create(new { OperatingCenter = opCenter });
            var pp2 = GetEntityFactory<PlanningPlant>().Create(new { OperatingCenter = opCenter });
            var orderTypeRoutine = GetFactory<RoutineOrderTypeFactory>().Create();
            var orderTypeCorrectiveAction = GetFactory<CorrectiveActionOrderTypeFactory>().Create();
            var orderTypePm = GetFactory<PlantMaintenanceOrderTypeFactory>().Create();
            var productionWorkDescriptionRoutine = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypeRoutine });
            var productionWorkDescriptionPm = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypePm });
            var productionWorkDescriptionCorrectiveAction = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypeCorrectiveAction });
            var equipment1 = GetEntityFactory<Equipment>().Create(new { HasProcessSafetyManagement = true });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { HasCompanyRequirement = true });
            var withPP1 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opCenter,
                PlanningPlant = pp1,
                DateReceived = _now,
                ProductionWorkDescription = productionWorkDescriptionRoutine
            });
            var withPP2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opCenter,
                PlanningPlant = pp2,
                DateReceived = _now,
                ProductionWorkDescription = productionWorkDescriptionCorrectiveAction
            });
            var withPP3 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opCenter,
                PlanningPlant = pp2,
                DateReceived = _now,
                ProductionWorkDescription = productionWorkDescriptionPm
            });
            var pwoe1 = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = withPP1,
                IsParent = true,
                Equipment = equipment1
            });
            withPP1.Equipments.Add(pwoe1);
            var pwoe2 = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = withPP2,
                IsParent = true,
                Equipment = equipment2
            });
            withPP2.Equipments.Add(pwoe2);
            var pwoe3 = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = withPP3,
                IsParent = true,
                Equipment = equipment2
            });
            withPP3.Equipments.Add(pwoe3);
            var search = new SearchRegulatoryCompliance {
                DateReceived = new RequiredDateRange { Operator = RangeOperator.Equal, End = _now },
                State = new[] { state.Id },
                OperatingCenter = new[] { opCenter.Id }
            };
            var result = _target.Index(search);
            Assert.IsTrue(search.Results.All(r => r.State == state.Abbreviation));
            Assert.IsTrue(search.Results.All(r => r.OperatingCenterName == opCenter.OperatingCenterName));
            Assert.IsTrue(search.Results.Any(r => r.PlanningPlantDescription == pp1.Description));
            Assert.IsTrue(search.Results.Any(r => r.PlanningPlantDescription == pp2.Description));
            Assert.AreEqual(2, search.Results.Count());
            Assert.IsTrue(search.Results.Any(r => r.EquipmentId == withPP1.Equipment.Id));
            Assert.IsTrue(search.Results.Any(r => r.EquipmentId == withPP3.Equipment.Id));
            MvcAssert.IsViewNamed(result, "Index");
            MvcAssert.IsViewWithNameAndModel(result, "Index", search);
        }
    }
}
