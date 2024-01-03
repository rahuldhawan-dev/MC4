using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models.ShortCycleWorkOrders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class OpenCrewAssignmentsControllerTest : MapCallApiControllerTestBase<OpenCrewAssignmentsController, CrewAssignment, CrewAssignmentRepository>
    {
        protected override User CreateUser()
        {
            var user = GetFactory<AdminUserFactory>().Create(new
            {
                DefaultOperatingCenter = GetFactory<OperatingCenterFactory>().Create()
            });

            Session.Save(user.UserType);

            return user;
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = OpenCrewAssignmentsController.ROLE;

            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/OpenCrewAssignments/Index/", module);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // noop override: returns json. If the below test covers this then move it into this test.
        }

        [TestMethod]
        public void TestIndexReturnsJSON()
        {
            var now = new DateTime(2018, 5, 15, 12, 12, 00);
            _dateTimeProvider.SetNow(now);

            var wo = GetFactory<WorkOrderFactory>().Create();
            var crew = GetEntityFactory<Crew>().Create(new {Description = "Crew1"});
            var e0 = GetEntityFactory<CrewAssignment>().Create(new { WorkOrder = wo, DateStarted = now, Crew = crew });

            var result = _target.Index(new SearchOpenCrewAssignments());
            var helper = new JsonResultTester(result);

            helper.AreEqual(wo.Id, "WorkOrderID");
        }
    }
}
