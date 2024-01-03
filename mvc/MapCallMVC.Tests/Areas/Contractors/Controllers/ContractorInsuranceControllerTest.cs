using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Contractors.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Contractors.Controllers
{
    [TestClass]
    public class ContractorInsuranceControllerTest : MapCallMvcControllerTestBase<ContractorInsuranceController, ContractorInsurance>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ContractorsGeneral;
            Authorization.Assert((a) => {
                a.RequiresRole("~/Contractors/ContractorInsurance/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/ContractorInsurance/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/ContractorInsurance/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/ContractorInsurance/New/", module, RoleActions.Add);
                a.RequiresRole("~/Contractors/ContractorInsurance/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Contractors/ContractorInsurance/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Contractors/ContractorInsurance/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Contractors/ContractorInsurance/Destroy/", module, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/Contractors/ContractorInsurance/ByContractorId/");
            });
        }
    }
}
