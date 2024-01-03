using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Operations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class AuthenticationLogControllerTest : MapCallMvcControllerTestBase<AuthenticationLogController, AuthenticationLog>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Operations/AuthenticationLog/Index", RoleModules.ManagementGeneral);
                a.RequiresRole("~/Operations/AuthenticationLog/Search", RoleModules.ManagementGeneral);
            });
        }

        #endregion
    }
}
