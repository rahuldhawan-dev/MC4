using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TrainingRecordAttendanceFormControllerTest : MapCallMvcControllerTestBase<TrainingRecordAttendanceFormController, TrainingRecord>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/TrainingRecordAttendanceForm/Create/", TrainingRecordAttendanceFormController.ROLE);
            });
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // override because this should be a Show action, in which case this test would never run anyway.
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            Assert.Inconclusive("test me");
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // override because this should be a Show action.
            Assert.Inconclusive("Test me");
        }

    }
}
