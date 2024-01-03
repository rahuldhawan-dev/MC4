using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.SAP.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.SAP.Controllers
{
    [TestClass]
    public class SAPCreatePreventiveWorkOrderControllerTest : MapCallMvcControllerTestBase<SAPCreatePreventiveWorkOrderController, ProductionWorkOrder>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/SAP/SAPCreatePreventiveWorkOrder/Show", role, RoleActions.Read);
            });
        }

        #region Show

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            Assert.Inconclusive("Test me");
        }

        #endregion
    }
}
