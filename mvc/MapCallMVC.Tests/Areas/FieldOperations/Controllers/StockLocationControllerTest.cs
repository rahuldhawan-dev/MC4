using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class StockLocationControllerTest : MapCallMvcControllerTestBase<StockLocationController, StockLocation>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/FieldOperations/StockLocation/Index/", module);
                a.RequiresRole("~/FieldOperations/StockLocation/Show/", module);
                a.RequiresRole("~/FieldOperations/StockLocation/Search/", module);
                a.RequiresSiteAdminUser("~/FieldOperations/StockLocation/New/");
                a.RequiresSiteAdminUser("~/FieldOperations/StockLocation/Create/");
                a.RequiresSiteAdminUser("~/FieldOperations/StockLocation/Edit/");
                a.RequiresSiteAdminUser("~/FieldOperations/StockLocation/Update/");
            });
        }
    }
}