using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallIntranet.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using System.Linq;

namespace MapCallIntranet.Tests
{
    [TestClass]
    public class FacilityControllerTest : MapCallIntranetControllerTestBase<FacilityController, Facility>
    {
        [TestMethod]
        public override void TestControllerAuthorization() {}

        #region GetFacilityBy

        [TestMethod]
        public void TestGetFacilityBy()
        {
            _currentUser.IsAdmin = true;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodFacility = GetFactory<FacilityFactory>().Create(new { OperatingCenter = opc });
            var badFacility = GetFactory<FacilityFactory>().Create();

            var result = (CascadingActionResult)_target.GetFacilityBy(opc.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodFacility.Id.ToString(), actual[1].Value);
        }

        #endregion
    }
}
