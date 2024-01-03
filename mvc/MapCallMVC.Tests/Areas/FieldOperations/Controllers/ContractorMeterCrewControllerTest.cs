using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ContractorMeterCrewControllerTest : MapCallMvcControllerTestBase<ContractorMeterCrewController, ContractorMeterCrew>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesMeterChangeOuts;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/ContractorMeterCrew/Show/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/ContractorMeterCrew/Index/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/ContractorMeterCrew/Search/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/ContractorMeterCrew/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ContractorMeterCrew/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ContractorMeterCrew/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ContractorMeterCrew/Update/", module, RoleActions.Edit);
            });
        }
    }
}