using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class PlanningPlantWasteWaterSystemControllerTest : MapCallMvcControllerTestBase<PlanningPlantWasteWaterSystemController, PlanningPlantWasteWaterSystem>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                return GetEntityFactory<PlanningPlantWasteWaterSystem>().Create(new {
                    PlanningPlant = GetEntityFactory<PlanningPlant>().Create(),
                    WasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create()
                });
            };
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "WasteWaterSystem", area = "Environmental", id = vm.WasteWaterSystem };
            options.DestroyRedirectsToRouteOnSuccessArgs = (vm) => {
                var wws = Repository.Find(vm)?.WasteWaterSystem?.Id;
                return new { action = "Show", controller = "WasteWaterSystem", area = "Environmental", id = wws };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/PlanningPlantWasteWaterSystem/Create");
                a.RequiresSiteAdminUser("~/PlanningPlantWasteWaterSystem/Destroy");
            });
        }

        [TestMethod]
        public void TestCreateCreatesNewPlanningPlantWasteWaterSystem()
        {
            var planningPlant = GetEntityFactory<PlanningPlant>().Create();
            var wws = GetEntityFactory<WasteWaterSystem>().Create();
            ActionResult result = null;
            
            MyAssert.CausesIncrease(
                () => result = _target.Create(new CreatePlanningPlantWasteWaterSystem(_container){PlanningPlant = planningPlant.Id, WasteWaterSystem = wws.Id }),
                () => Repository.GetAll().Count());
        }

        [TestMethod]
        public void TestDestroyRemovesPlanningPlantWasteWaterSystem()
        {
            var planningPlant = GetEntityFactory<PlanningPlant>().Create();
            var wws = GetEntityFactory<WasteWaterSystem>().Create();
            var planningPlantWasteWaterSystem = GetEntityFactory<PlanningPlantWasteWaterSystem>().Create(new {PlanningPlant = planningPlant, WasteWaterSystem = wws});

            ActionResult result = null;

            MyAssert.CausesDecrease(
                () => result = _target.Destroy(planningPlantWasteWaterSystem.Id),
                () => Repository.GetAll().Count());
        }
    }
}
