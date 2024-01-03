using System.Web.Mvc;
using Contractors.Controllers;
using MapCall.Common.Model.Entities;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class RestorationMethodControllerTest : ContractorControllerTestBase<RestorationMethodController, RestorationMethod>
    {
        #region Tests

        [TestMethod]
        public void TestByRestorationTypeIDIsHttpGetOnly()
        {
            var target = _container.GetInstance<RestorationMethodController>();
            MyAssert.MethodHasAttribute<HttpGetAttribute>(target, "ByRestorationTypeID", new[] {typeof (int)});
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/RestorationMethod/ByRestorationTypeID");
            });
        }

        #endregion
    }
}