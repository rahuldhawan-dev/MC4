using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class WaterSampleComplianceFormControllerTest : MapCallMvcControllerTestBase<WaterSampleComplianceFormController, WaterSampleComplianceForm>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.EnvironmentalGeneral;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/WaterSampleComplianceForm/Show/", role, RoleActions.Read);
                a.RequiresRole("~/Environmental/WaterSampleComplianceForm/Search/", role, RoleActions.Read);
                a.RequiresRole("~/Environmental/WaterSampleComplianceForm/Index/", role, RoleActions.Read);
                a.RequiresRole("~/Environmental/WaterSampleComplianceForm/New/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/WaterSampleComplianceForm/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/WaterSampleComplianceForm/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/WaterSampleComplianceForm/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/WaterSampleComplianceForm/Destroy/", role, RoleActions.Delete);
            });
        }

        #endregion

        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needs to use Create rather than BuildWithConcreteDependencies due to Build method in factory.
            options.CreateValidEntity = () => GetEntityFactory<WaterSampleComplianceForm>().Create();
            options.InitializeCreateViewModel = (vm) => {
                var pwsid = GetEntityFactory<PublicWaterSupply>().Create(new { AWOwned = true });
                var model = (CreateWaterSampleComplianceForm)vm;
                model.PublicWaterSupply = pwsid.Id;
            };
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override needed for New parameter
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var result = (ViewResult)_target.New(pwsid.Id);

            MyAssert.IsInstanceOfType<ActionResult>(result);

            var model = (CreateWaterSampleComplianceForm)result.Model;

            Assert.AreEqual(pwsid.Id, model.PublicWaterSupply.Value);
        }

        #endregion
    }
}
