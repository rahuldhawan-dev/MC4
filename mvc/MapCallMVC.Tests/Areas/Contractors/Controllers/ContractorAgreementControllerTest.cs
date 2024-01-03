using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Contractors.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MapCallMVC.Tests.Areas.Contractors.Controllers
{
    [TestClass]
    public class ContractorAgreementControllerTest : MapCallMvcControllerTestBase<ContractorAgreementController, ContractorAgreement>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ContractorsAgreements;
            Authorization.Assert((a) => {
                a.RequiresRole("~/Contractors/ContractorAgreement/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/ContractorAgreement/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/ContractorAgreement/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/ContractorAgreement/New/", module, RoleActions.Add);
                a.RequiresRole("~/Contractors/ContractorAgreement/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Contractors/ContractorAgreement/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Contractors/ContractorAgreement/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Contractors/ContractorAgreement/Destroy/", module, RoleActions.Delete);
            });
        }

        #region New

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenters"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion
    }
}
