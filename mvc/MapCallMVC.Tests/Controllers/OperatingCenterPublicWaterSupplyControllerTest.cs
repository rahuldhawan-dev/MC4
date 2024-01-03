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
    public class OperatingCenterPublicWaterSupplyControllerTest : MapCallMvcControllerTestBase<OperatingCenterPublicWaterSupplyController, OperatingCenterPublicWaterSupply>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "PublicWaterSupply", area = "", id = vm.PublicWaterSupply };
            options.DestroyRedirectsToRouteOnSuccessArgs = (vm) => {
                var pws = Repository.Find(vm)?.PublicWaterSupply?.Id;
                return new { action = "Show", controller = "PublicWaterSupply", area = "", id = pws };
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateOperatingCenterPublicWaterSupply)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.PublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/OperatingCenterPublicWaterSupply/Create");
                a.RequiresSiteAdminUser("~/OperatingCenterPublicWaterSupply/Destroy");
            });
        }

        [TestMethod]
        public void TestCreateCreatesNewOperatingCenterPublicWaterSupply()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var pws = GetEntityFactory<PublicWaterSupply>().Create();
            ActionResult result = null;
            
            MyAssert.CausesIncrease(
                () => result = _target.Create(new CreateOperatingCenterPublicWaterSupply(_container){OperatingCenter = operatingCenter.Id, PublicWaterSupply = pws.Id }),
                () => Repository.GetAll().Count());
        }

        [TestMethod]
        public void TestDestroyRemovesOperatingCenterPublicWaterSupply()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var pws = GetEntityFactory<PublicWaterSupply>().Create();
            var operatingCenterPublicWaterSupply = GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {OperatingCenter = operatingCenter, PublicWaterSupply = pws});

            ActionResult result = null;

            MyAssert.CausesDecrease(
                () => result = _target.Destroy(operatingCenterPublicWaterSupply.Id),
                () => Repository.GetAll().Count());
        }
    }
}