using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EmployeeUserReconciliationControllerTest : MapCallMvcControllerTestBase<EmployeeUserReconciliationController, Employee>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = EmployeeUserReconciliationController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/EmployeeUserReconciliation/Search/", role);
                a.RequiresRole("~/Reports/EmployeeUserReconciliation/Index/", role);
            });
        }

        #endregion

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }
    }
}