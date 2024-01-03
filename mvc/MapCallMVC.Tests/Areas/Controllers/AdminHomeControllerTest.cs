using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Admin.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class AdminHomeControllerTest : MapCallMvcControllerTestBase<AdminHomeController, User>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresSiteAdminUser("~/Admin/AdminHome/Index");
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
