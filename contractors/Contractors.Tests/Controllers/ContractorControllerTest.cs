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
    public class ContractorControllerTest : ContractorControllerTestBase<ContractorController, Contractor>
    {
        #region Tests

        [TestMethod]
        public void TestByOperatingCenterIdIsHttpGetOnly()
        {
            var target = _container.GetInstance<ContractorController>();
            MyAssert.MethodHasAttribute<HttpGetAttribute>(target, "ByOperatingCenterId", new[] { typeof(int) });
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Contractor/ByOperatingCenterId");
            });
        }

        [TestMethod]
        public void TestByOperatingCenterIdGetsByOperatingCenterId()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { OperatingCenterCode = "NJ4" });
            var contractor1 = GetEntityFactory<Contractor>().Create(new { Name = "Tina" });
            var contractor2 = GetEntityFactory<Contractor>().Create(new { Name = "Gene" });
            var contractor3 = GetEntityFactory<Contractor>().Create(new { Name = "Louise" });
            contractor1.OperatingCenters.Add(operatingCenter);
            contractor2.OperatingCenters.Add(operatingCenter);
            contractor3.OperatingCenters.Add(operatingCenter);
            Session.Save(contractor1);
            Session.Save(contractor2);
            Session.Save(contractor3);
            Session.Evict(operatingCenter);
            
            Session.Flush();

            var target = _container.GetInstance<ContractorController>();

            var result = (CascadingActionResult)target.ByOperatingCenterId(operatingCenter.Id);
            var items = result.GetSelectListItems();
            Assert.AreEqual(4, items.Count()); // first one is --Select Here--
            Assert.AreEqual(contractor2.Id.ToString(), items.Skip(1).First().Value);  // returned alphabetical
            Assert.AreEqual(contractor3.Id.ToString(), items.Skip(2).First().Value);  // returned alphabetical
            Assert.AreEqual(contractor1.Id.ToString(), items.Last().Value);
        }

        #endregion
    }
}
