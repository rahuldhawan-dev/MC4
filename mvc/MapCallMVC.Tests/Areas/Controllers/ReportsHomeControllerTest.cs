using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class ReportsHomeControllerTest : MapCallMvcControllerTestBase<ReportsHomeController, User>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/Reports/ReportsHome/Index");
            });
        }

        [TestMethod]
        public void TestIndexReturnsIndexView()
        {
            var result = _target.Index();
            MvcAssert.IsViewNamed(result, "Index");
        }

        #endregion
    }
}
