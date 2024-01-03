using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Controllers
{
    [TestClass]
    public class InvestmentProjectControllerTest : MapCallMvcControllerTestBase<InvestmentProjectController, InvestmentProject>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules module = RoleModules.FieldServicesProjects;
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/InvestmentProject/Index", module);
                a.RequiresRole("~/ProjectManagement/InvestmentProject/Search", module);
                a.RequiresRole("~/ProjectManagement/InvestmentProject/Show", module);
                a.RequiresRole("~/ProjectManagement/InvestmentProject/New", module, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/InvestmentProject/Create", module, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/InvestmentProject/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/InvestmentProject/Update", module, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/ProjectManagement/InvestmentProject/Destroy");
            });
        }
        
        #region New

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion
    }
}
