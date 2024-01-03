using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EnvironmentalNonComplianceEventSubTypeControllerTest : MapCallMvcControllerTestBase<EnvironmentalNonComplianceEventSubTypeController, EnvironmentalNonComplianceEventSubType>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needed because the controller inherits from EntityLookupControllerBase.
            options.ExpectedEditViewName = "~/Areas/Environmental/Views/EnvironmentalNonComplianceEventSubType/Edit.cshtml";
            options.ExpectedNewViewName = "~/Areas/Environmental/Views/EnvironmentalNonComplianceEventSubType/New.cshtml";
            options.ExpectedShowViewName = "~/Areas/Environmental/Views/EnvironmentalNonComplianceEventSubType/Show.cshtml";
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventSubType/Index/",
                        RoleModules.FieldServicesDataLookups);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventSubType/Show/",
                    RoleModules.FieldServicesDataLookups);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventSubType/New/",
                    RoleModules.FieldServicesDataLookups, RoleActions.Add);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventSubType/Create/",
                    RoleModules.FieldServicesDataLookups, RoleActions.Add);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventSubType/Edit/",
                    RoleModules.FieldServicesDataLookups, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventSubType/Update/",
                    RoleModules.FieldServicesDataLookups, RoleActions.Edit);

                a.RequiresLoggedInUserOnly("~/Environmental/EnvironmentalNonComplianceEventSubType/ByTypeId/");
            });
        }
    }
}