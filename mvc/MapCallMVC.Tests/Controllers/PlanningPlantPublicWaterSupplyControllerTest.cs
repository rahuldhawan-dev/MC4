using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PlanningPlantPublicWaterSupplyControllerTest : MapCallMvcControllerTestBase<PlanningPlantPublicWaterSupplyController, PlanningPlantPublicWaterSupply>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                return GetEntityFactory<PlanningPlantPublicWaterSupply>().Create(new {
                    PlanningPlant = GetEntityFactory<PlanningPlant>().Create(),
                    PublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create()
                });
            };
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "PublicWaterSupply", area = "", id = vm.PublicWaterSupply };
            options.DestroyRedirectsToRouteOnSuccessArgs = (vm) => {
                var pws = Repository.Find(vm)?.PublicWaterSupply?.Id;
                return new { action = "Show", controller = "PublicWaterSupply", area = "", id = pws };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/PlanningPlantPublicWaterSupply/Create");
                a.RequiresSiteAdminUser("~/PlanningPlantPublicWaterSupply/Destroy");
            });
        }

        [TestMethod]
        public void TestCreateCreatesNewPlanningPlantPublicWaterSupply()
        {
            var PlanningPlant = GetEntityFactory<PlanningPlant>().Create();
            var pws = GetEntityFactory<PublicWaterSupply>().Create();
            ActionResult result = null;
            
            MyAssert.CausesIncrease(
                () => result = _target.Create(new CreatePlanningPlantPublicWaterSupply(_container){PlanningPlant = PlanningPlant.Id, PublicWaterSupply = pws.Id }),
                () => Repository.GetAll().Count());
        }

        [TestMethod]
        public void TestDestroyRemovesPlanningPlantPublicWaterSupply()
        {
            var PlanningPlant = GetEntityFactory<PlanningPlant>().Create();
            var pws = GetEntityFactory<PublicWaterSupply>().Create();
            var PlanningPlantPublicWaterSupply = GetEntityFactory<PlanningPlantPublicWaterSupply>().Create(new {PlanningPlant = PlanningPlant, PublicWaterSupply = pws});

            ActionResult result = null;

            MyAssert.CausesDecrease(
                () => result = _target.Destroy(PlanningPlantPublicWaterSupply.Id),
                () => Repository.GetAll().Count());
        }
    }
}