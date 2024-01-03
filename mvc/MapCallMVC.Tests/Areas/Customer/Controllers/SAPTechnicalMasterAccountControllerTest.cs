using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Customer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Customer.Controllers
{
    [TestClass]
    public class SAPTechnicalMasterAccountControllerTest : MapCallMvcControllerTestBase<SAPTechnicalMasterAccountController, WorkOrder>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Customer/SAPTechnicalMasterAccount/Search", role, RoleActions.Read);
                a.RequiresRole("~/Customer/SAPTechnicalMasterAccount/Index", role, RoleActions.Read);
                a.RequiresRole("~/Customer/SAPTechnicalMasterAccount/Find", role, RoleActions.Read);
            });
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            Assert.Inconclusive("This is SAP related and is not tested.");
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("This is SAP related and is not tested.");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("This is SAP related and is not tested.");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            Assert.Inconclusive("This is SAP related and is not tested.");
        }

        #endregion
    }
}