using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class OperatingCenterServiceMaterialControllerTest : MapCallMvcControllerTestBase<OperatingCenterServiceMaterialController, OperatingCenterServiceMaterial>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "ServiceMaterial", area = "", id = vm.ServiceMaterial };
            options.DestroyRedirectsToRouteOnSuccessArgs = (id) => {
                var serviceMaterialId = Repository.Find(id)?.ServiceMaterial?.Id;
                return new { action = "Show", controller = "ServiceMaterial", area = "", id = serviceMaterialId };
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateOperatingCenterServiceMaterial)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.ServiceMaterial = GetEntityFactory<ServiceMaterial>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/OperatingCenterServiceMaterial/Create", role, RoleActions.Edit);
                a.RequiresRole("~/OperatingCenterServiceMaterial/Destroy", role, RoleActions.Edit);
            });
        }
    }
}