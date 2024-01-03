using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class MeasurementPointEquipmentTypeControllerTest : MapCallMvcControllerTestBase<MeasurementPointEquipmentTypeController, MeasurementPointEquipmentType>
    {
        #region Tests
        
        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // noop
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            // noop
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/MeasurementPointEquipmentType/Edit/");
                a.RequiresSiteAdminUser("~/MeasurementPointEquipmentType/Update/");
                a.RequiresSiteAdminUser("~/MeasurementPointEquipmentType/Create/");
            });
        }

        #endregion
    }
}
