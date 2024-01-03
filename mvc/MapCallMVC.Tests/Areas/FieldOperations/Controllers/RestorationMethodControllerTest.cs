using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class RestorationMethodControllerTest : MapCallMvcControllerTestBase<RestorationMethodController, RestorationMethod, RestorationMethodRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/FieldOperations/RestorationMethod/ByRestorationTypeID/");
            });
        }
    }
}
