using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using System.Web.Mvc;
using MapCallMVC.Models.ViewModels;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EquipmentLifespanControllerTest : MapCallMvcControllerTestBase<EquipmentLifespanController, EquipmentLifespan>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needed because the controller inherits from EntityLookupControllerBase.
            options.ExpectedEditViewName = "~/Views/EquipmentLifespan/Edit.cshtml";
            options.ExpectedNewViewName = "~/Views/EquipmentLifespan/New.cshtml";
            options.ExpectedShowViewName = "~/Views/EquipmentLifespan/Show.cshtml";
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            // We need to use a url for an actual EntityLookup here, can't just use EntityLookup since it's not valid.
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesDataLookups; 
                var eamModule = RoleModules.EngineeringEAMAssetManagement;

                a.RequiresRole("~/EquipmentLifespan/Show/", module, RoleActions.Read);
                a.RequiresRole("~/EquipmentLifespan/Index/", module, RoleActions.Read);
                a.RequiresRole("~/EquipmentLifespan/Edit/", eamModule, RoleActions.Edit);
                a.RequiresRole("~/EquipmentLifespan/Update/", eamModule, RoleActions.Edit);
                a.RequiresRole("~/EquipmentLifespan/Create/", module, RoleActions.Add);
                a.RequiresRole("~/EquipmentLifespan/New/", eamModule, RoleActions.Add);
            });
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            var vm = new EquipmentLifespanViewModel(_container) { Description = "any description will do - big blue police box" };
            ActionResult result;

            MyAssert.CausesIncrease(() => result = _target.Create(vm),
                () => Repository.GetAll().Count());
        }
    }
}
