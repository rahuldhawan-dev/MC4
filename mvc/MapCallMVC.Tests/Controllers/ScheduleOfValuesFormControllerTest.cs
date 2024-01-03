using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class ScheduleOfValuesFormControllerTest : MapCallMvcControllerTestBase<ScheduleOfValuesFormController, EstimatingProject, EstimatingProjectRepository>
    {
        #region Constants

        public const RoleModules ROLE = MaterialRequisitionFormController.ROLE;

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/ScheduleOfValuesForm/Show/", ROLE);
            });
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            Assert.Inconclusive("Test me. I return a view model rather than the entity.");
        }
    }
}
