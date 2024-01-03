using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Areas.Engineering.Models.ViewModels.AwiaCompliance;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EquipmentGroupControllerTest : MapCallMvcControllerTestBase<EquipmentGroupController, EquipmentGroup>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.ProductionEquipment;
                a.RequiresRole("~/EquipmentGroup/Index/", module, RoleActions.Read);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            //user can't search for equipmentgroups
        }

        #endregion

    }
}
