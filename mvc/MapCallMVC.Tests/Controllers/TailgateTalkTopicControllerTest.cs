using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TailgateTalkTopicControllerTest : MapCallMvcControllerTestBase<TailgateTalkTopicController, TailgateTalkTopic>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.OperationsHealthAndSafety;

            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/HealthAndSafety/TailgateTalkTopic/ByCategoryId/");
                a.RequiresRole("~/HealthAndSafety/TailgateTalkTopic/Search/", module);
                a.RequiresRole("~/HealthAndSafety/TailgateTalkTopic/Show/", module);
                a.RequiresRole("~/HealthAndSafety/TailgateTalkTopic/Index/", module);
                a.RequiresRole("~/HealthAndSafety/TailgateTalkTopic/New/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/TailgateTalkTopic/Create/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/TailgateTalkTopic/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/TailgateTalkTopic/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/TailgateTalkTopic/Destroy/", module, RoleActions.Delete);
            });
        }
    }
}
