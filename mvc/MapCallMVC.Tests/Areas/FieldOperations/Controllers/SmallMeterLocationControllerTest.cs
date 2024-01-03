using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SmallMeterLocationControllerTest : MapCallMvcControllerTestBase<SmallMeterLocationController, WorkOrder>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/FieldOperations/SmallMeterLocation/ByMeterSupplementalLocationId");
            });
        }

        [TestMethod]
        public void TestByMeterSupplementalLocationIdReturnsAppropriately()
        {
            var supp = GetEntityFactory<MeterSupplementalLocation>().Create();
            var smallMeterLocation = GetEntityFactory<SmallMeterLocation>().Create();
            smallMeterLocation.MeterSupplementalLocations.Add(supp);
            var smallMeterLocationInvalid = GetEntityFactory<SmallMeterLocation>().Create();
            Session.Save(smallMeterLocation);
            Session.Flush();

            var results = _target.ByMeterSupplementalLocationId(supp.Id) as CascadingActionResult;
            var actual = results.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Length - 1);
            Assert.AreEqual(smallMeterLocation.Id.ToString(), actual[1].Value);
        }
    }
}