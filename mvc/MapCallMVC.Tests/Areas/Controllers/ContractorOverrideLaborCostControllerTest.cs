using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class ContractorOverrideLaborCostControllerTest : MapCallMvcControllerTestBase<ContractorOverrideLaborCostController, ContractorOverrideLaborCost>
    {
        #region Fields


        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesEstimatingProjects;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/ProjectManagement/ContractorOverrideLaborCost/Show", module, RoleActions.Read);
                a.RequiresRole("~/ProjectManagement/ContractorOverrideLaborCost/Search", module, RoleActions.Read);
                a.RequiresRole("~/ProjectManagement/ContractorOverrideLaborCost/Index", module, RoleActions.Read);
                a.RequiresRole("~/ProjectManagement/ContractorOverrideLaborCost/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/ContractorOverrideLaborCost/Update", module, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/ContractorOverrideLaborCost/New", module, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/ContractorOverrideLaborCost/Create", module, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/ContractorOverrideLaborCost/Destroy", module, RoleActions.Delete);
            });
        }

        #endregion
    }
}
