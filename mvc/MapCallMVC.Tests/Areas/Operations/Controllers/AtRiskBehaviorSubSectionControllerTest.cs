using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Operations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Operations.Controllers
{
    [TestClass]
    public class AtRiskBehaviorSubSectionControllerTest : MapCallMvcControllerTestBase<AtRiskBehaviorSubSectionController, AtRiskBehaviorSubSection>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.OperationsIncidents;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Operations/AtRiskBehaviorSubSection/ByAtRiskBehaviorSectionId", module, RoleActions.Read);
            });
        }

        #endregion
    }
}
