using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Facilities.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class FacilitySubAreaControllerTest : MapCallMvcControllerTestBase<FacilitySubAreaController, FacilitySubArea>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = FacilitySubAreaController.ROLE;
                const string path = "~/FacilitySubArea/";
                a.RequiresRole(path + "Show", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
                a.RequiresLoggedInUserOnly("~/FacilitySubArea/ByFacilityArea");
            });
        }
    }
}
