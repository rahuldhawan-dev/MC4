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
    public class ServiceRestorationContractorControllerTest : ContractorControllerTestBase<ServiceRestorationContractorController, ServiceRestorationContractor>
    {
        #region Tests

        [TestMethod]
        public void TestByOperatingCenterIdIsHttpGetOnly()
        {
            var target = _container.GetInstance<ServiceRestorationContractorController>();
            MyAssert.MethodHasAttribute<HttpGetAttribute>(target, "ByOperatingCenterId", new[] { typeof(int) });
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/ServiceRestorationContractor/ByOperatingCenterId");
            });
        }

        [TestMethod]
        public void TestByOperatingCenterIdGetsByOperatingCenterId()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { OperatingCenterCode = "NJ4" });
            var serviceRestorationContractor1 = GetEntityFactory<ServiceRestorationContractor>().Create(new { OperatingCenter = operatingCenter, Contractor = "Tina"});
            var serviceRestorationContractor2 = GetEntityFactory<ServiceRestorationContractor>().Create(new { OperatingCenter = operatingCenter, Contractor = "Gene"});
            var serviceRestorationContractor3 = GetEntityFactory<ServiceRestorationContractor>().Create(new { OperatingCenter = operatingCenter, Contractor = "Louise"});

            Session.Flush();

            var target = _container.GetInstance<ServiceRestorationContractorController>();

            var result = (CascadingActionResult)target.ByOperatingCenterId(operatingCenter.Id);
            var items = result.GetSelectListItems();
            Assert.AreEqual(4, items.Count()); // first one is --Select Here--
            Assert.AreEqual("2", items.Skip(1).First().Value);  // returned alphabetical
            Assert.AreEqual("3", items.Skip(2).First().Value);  // returned alphabetical
            Assert.AreEqual("1", items.Last().Value);
        }

        #endregion
    }
}
