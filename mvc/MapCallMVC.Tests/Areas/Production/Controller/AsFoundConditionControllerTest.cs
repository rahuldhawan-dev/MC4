using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class AsFoundConditionControllerTest : MapCallMvcControllerTestBase<AsFoundConditionController, AsFoundCondition>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.ExpectedEditViewName = "~/Areas/Production/Views/AsFoundCondition/Edit.cshtml";
            options.ExpectedNewViewName = "~/Areas/Production/Views/AsFoundCondition/New.cshtml";
            options.ExpectedShowViewName = "~/Areas/Production/Views/AsFoundCondition/Show.cshtml";
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ProductionDataAdministration;
            
            Authorization.Assert(a => {
                a.RequiresRole("~/Production/AsFoundCondition/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Production/AsFoundCondition/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Production/AsFoundCondition/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Production/AsFoundCondition/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Production/AsFoundCondition/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Production/AsFoundCondition/New/", module, RoleActions.Add);
            });
        }

        #endregion
    }
}