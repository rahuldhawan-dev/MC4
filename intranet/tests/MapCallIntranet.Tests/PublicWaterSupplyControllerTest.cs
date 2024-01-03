using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallIntranet.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallIntranet.Tests
{
    [TestClass]
    public class PublicWaterSupplyControllerTest : MapCallIntranetControllerTestBase<PublicWaterSupplyController, PublicWaterSupply>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization() { }

        [TestMethod]
        public void TestGetSystemNameByOperatingCenter()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var pws = GetEntityFactory<PublicWaterSupply>().Create();
            var operatingCenterPublicWaterSupply = GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = opc, PublicWaterSupply = pws });
            var result = (CascadingActionResult)_target.GetSystemNameByOperatingCenter(opc.Id);
            var data = (IEnumerable<PublicWaterSupplyDisplayItemForNearMiss>)result.Data;
            Assert.AreEqual(operatingCenterPublicWaterSupply.Id, data.Single().Id);
        }

        #endregion
    }
}