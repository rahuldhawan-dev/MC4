using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class AsLeftConditionControllerTest : MapCallMvcControllerTestBase<AsLeftConditionController, AsLeftCondition>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.ExpectedEditViewName = "~/Areas/Production/Views/AsLeftCondition/Edit.cshtml";
            options.ExpectedNewViewName = "~/Areas/Production/Views/AsLeftCondition/New.cshtml";
            options.ExpectedShowViewName = "~/Areas/Production/Views/AsLeftCondition/Show.cshtml";
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ProductionDataAdministration;

            Authorization.Assert(a => {
                a.RequiresRole("~/Production/AsLeftCondition/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Production/AsLeftCondition/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Production/AsLeftCondition/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Production/AsLeftCondition/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Production/AsLeftCondition/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Production/AsLeftCondition/New/", module, RoleActions.Add);
            });
        }

        #endregion
    }
}