using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Testing.MSTest.TestExtensions;

namespace Contractors.Tests.Controllers 
{
    [TestClass]
    public class ServiceMaterialControllerTest : ContractorControllerTestBase<ServiceMaterialController, ServiceMaterial>
    {
        #region Tests

        [TestMethod]
        public void TestByOperatingCenterIdIsHttpGetOnly()
        {
            var target = _container.GetInstance<ServiceMaterialController>();
            MyAssert.MethodHasAttribute<HttpGetAttribute>(target, "ByOperatingCenterId", new[] { typeof(int) });
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/ServiceMaterial/ByOperatingCenterId");
            });
        }

        [TestMethod]
        public void TestByOperatingCenterIdGetsByOperatingCenterId()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { OperatingCenterCode = "NJ4" });
            var serviceMaterials = GetEntityFactory<ServiceMaterial>().CreateList(3);
            var operatingCenterMaterial1 = GetEntityFactory<OperatingCenterServiceMaterial>().Create(new {
                    OperatingCenter = operatingCenter,
                    ServiceMaterial = serviceMaterials.First()
                });
            var operatingCenterMaterial2 = GetEntityFactory<OperatingCenterServiceMaterial>().Create(new {
                    OperatingCenter = operatingCenter,
                    ServiceMaterial = serviceMaterials.Last()
                });
            Session.Flush();

            var target = _container.GetInstance<ServiceMaterialController>();

            var result = (CascadingActionResult)target.ByOperatingCenterId(operatingCenter.Id);
            var items = result.GetSelectListItems();
            Assert.AreEqual(3, items.Count()); // first one is --Select Here--
            Assert.AreEqual("1", items.Skip(1).First().Value);
            Assert.AreEqual("3", items.Last().Value);
        }

        #endregion
    }
}
