using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class FilterMediaControllerTest : MapCallMvcControllerTestBase<FilterMediaController, FilterMedia>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.ProductionFacilities;
                a.RequiresRole("~/FilterMedia/Search", module, RoleActions.Read);
                a.RequiresRole("~/FilterMedia/Show", module, RoleActions.Read);
                a.RequiresRole("~/FilterMedia/Index", module, RoleActions.Read);
                a.RequiresRole("~/FilterMedia/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/FilterMedia/Update", module, RoleActions.Edit);
                a.RequiresRole("~/FilterMedia/New", module, RoleActions.Add);
                a.RequiresRole("~/FilterMedia/Create", module, RoleActions.Add);
            });
        }
    }
}
