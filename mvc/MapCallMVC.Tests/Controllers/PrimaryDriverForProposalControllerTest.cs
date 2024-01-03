using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PrimaryDriverForProposalControllerTest : MapCallMvcControllerTestBase<PrimaryDriverForProposalController, PrimaryDriverForProposal>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needed because controller inherits from EntityLookupControllerBase
            options.ExpectedEditViewName = "~/Views/EntityLookup/Edit.cshtml";
            options.ExpectedNewViewName = "~/Views/EntityLookup/New.cshtml";
            options.ExpectedShowViewName = "~/Views/EntityLookup/Show.cshtml";
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRoles("~/PrimaryDriverForProposal/Destroy",
                    new Dictionary<RoleModules, RoleActions> {
                        {PrimaryDriverForProposalController.ROLE, RoleActions.Delete},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Delete}
                    });
                a.RequiresRoles("~/PrimaryDriverForProposal/Index",
                    new Dictionary<RoleModules, RoleActions> {
                        {PrimaryDriverForProposalController.ROLE, RoleActions.Read},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Read}
                    });
                a.RequiresRoles("~/PrimaryDriverForProposal/Show",
                    new Dictionary<RoleModules, RoleActions> {
                        {PrimaryDriverForProposalController.ROLE, RoleActions.Read},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Read}
                    });
                a.RequiresRoles("~/PrimaryDriverForProposal/Create",
                    new Dictionary<RoleModules, RoleActions> {
                        {PrimaryDriverForProposalController.ROLE, RoleActions.Add},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Add}
                    });
                a.RequiresRoles("~/PrimaryDriverForProposal/New",
                    new Dictionary<RoleModules, RoleActions> {
                        {PrimaryDriverForProposalController.ROLE, RoleActions.Add},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Add}
                    });
                a.RequiresRoles("~/PrimaryDriverForProposal/Update",
                    new Dictionary<RoleModules, RoleActions> {
                        {PrimaryDriverForProposalController.ROLE, RoleActions.Edit},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Edit}
                    });
                a.RequiresRoles("~/PrimaryDriverForProposal/Edit",
                    new Dictionary<RoleModules, RoleActions> {
                        {PrimaryDriverForProposalController.ROLE, RoleActions.Edit},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Edit}
                    });
            });
        }

        #endregion
    }
}