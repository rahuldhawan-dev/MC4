using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCallIntranet.Controllers;
using MMSINC;

// ReSharper disable once CheckNamespace
namespace MapCallIntranet.Tests
{
    [TestClass]
    public class WasteWaterSystemControllerTest : MapCallIntranetControllerTestBase<WasteWaterSystemController, WasteWaterSystem>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization() { }

        #region GetSystemNameByOperatingCenter

        [TestMethod]
        public void TestGetSystemNameByOperatingCenter()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var wasteWater = GetEntityFactory<WasteWaterSystem>().Create(new { OperatingCenter = opc });

            var result = (CascadingActionResult)_target.GetSystemNameByOperatingCenter(opc.Id);
            var data = (IEnumerable<WasteWaterSystemDisplayItem>)result.Data;

            Assert.AreEqual(wasteWater.Id, data.Single().Id);
        }

        #endregion

        #endregion
    }
}