using System;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PublicWaterSupplyLicensedOperatorControllerTest : MapCallMvcControllerTestBase<PublicWaterSupplyLicensedOperatorController, PublicWaterSupplyLicensedOperator>
    {
        #region Init/Cleanup 

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            Func<dynamic, object> createRedirect = (vm) => {
                var model = (CreatePublicWaterSupplyLicensedOperator)vm;
                return new { action = "Show", controller = "OperatorLicense", area = "", id = model.LicensedOperator };
            };
            options.CreateRedirectsToRouteOnErrorArgs = createRedirect;
            options.CreateRedirectsToRouteOnSuccessArgs = createRedirect;
            Func<dynamic, object> destroyRedirect = (vm) => {
                var id = (int)vm;
                var routeId = Repository.Find(id)?.LicensedOperator?.Id;
                return new { action = "Show", controller = "OperatorLicense", area = "", id = routeId };
            };
            options.DestroyRedirectsToRouteOnErrorArgs = destroyRedirect;
            options.DestroyRedirectsToRouteOnSuccessArgs = destroyRedirect;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.EnvironmentalGeneral;
                const string path = "~/PublicWaterSupplyLicensedOperator/";
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "Destroy", role, RoleActions.Delete);
            });
        }
    }
}

