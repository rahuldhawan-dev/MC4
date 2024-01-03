using System.Web.Mvc;
using Contractors.Controllers;
using MapCall.Common.Model.Entities;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class StreetControllerTest : ContractorControllerTestBase<StreetController, Street>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Street/ByTownId");
            });
        }
        
        [TestMethod]
        public void TestByTownIdIsHttpGetOnly()
        {
            var target = _container.GetInstance<StreetController>();
            MyAssert.MethodHasAttribute<HttpGetAttribute>(target, "ByTownId", new[] {typeof (int)});
        }

        #endregion
    }
}
