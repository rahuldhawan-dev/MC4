using System.Web.Mvc;
using Contractors.Controllers;
using MapCall.Common.Model.Entities;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class WorkDescriptionControllerTest : ContractorControllerTestBase<WorkDescriptionController, WorkDescription>
    {
        #region Tests

        [TestMethod]
        public void TestByAssetTypeIdIsHttpGetOnly()
        {
            var target = _container.GetInstance<WorkDescriptionController>();
            MyAssert.MethodHasAttribute<HttpGetAttribute>(target, "ByAssetTypeId", new[] {typeof (int)});
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WorkDescription/ByAssetTypeId");
            });
        }

        #endregion
    }
}
