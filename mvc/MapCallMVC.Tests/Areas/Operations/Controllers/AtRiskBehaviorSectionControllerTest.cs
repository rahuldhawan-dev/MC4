using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Operations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Operations.Controllers
{
    [TestClass]
    public class AtRiskBehaviorSectionControllerTest : MapCallMvcControllerTestBase<AtRiskBehaviorSectionController, AtRiskBehaviorSection>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.OperationsIncidents;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Operations/AtRiskBehaviorSection/Index", module, RoleActions.Read);
                a.RequiresRole("~/Operations/AtRiskBehaviorSection/Show", module, RoleActions.Read);
                a.RequiresRole("~/Operations/AtRiskBehaviorSection/New", module, RoleActions.Add);
                a.RequiresRole("~/Operations/AtRiskBehaviorSection/Create", module, RoleActions.Add);
                a.RequiresRole("~/Operations/AtRiskBehaviorSection/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/Operations/AtRiskBehaviorSection/Update", module, RoleActions.Edit);
                a.RequiresRole("~/Operations/AtRiskBehaviorSection/Destroy", module, RoleActions.Delete);
                a.RequiresRole("~/Operations/AtRiskBehaviorSection/AddAtRiskBehaviorSubSection", module, RoleActions.Edit);
                a.RequiresRole("~/Operations/AtRiskBehaviorSection/RemoveAtRiskBehaviorSubSection", module, RoleActions.Edit);
            });
        }

        #endregion
    }
}
