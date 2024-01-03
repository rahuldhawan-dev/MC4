using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Facilities.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class FacilityAreaControllerTest : MapCallMvcControllerTestBase<FacilityAreaController, FacilityArea>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = FacilityAreaController.ROLE;
                const string path = "~/FacilityArea/";
                a.RequiresRole(path + "Show", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
            });
        }
    }
}
