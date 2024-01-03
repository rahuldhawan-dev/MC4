using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.Data;
using System.Web.Mvc;
using MMSINC.Testing;
using MapCall.Common.Testing.Data;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class CrewAssignmentSummaryControllerTest : MapCallMvcControllerTestBase<CrewAssignmentSummaryController, CrewAssignment>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.FieldServicesWorkManagement;
                const string path = "~/Reports/CrewAssignmentSummary/";
                a.RequiresRole(path + "Search", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            _currentUser.IsAdmin = true;
            var date = DateTime.Now;
            var workorder = GetFactory<WorkOrderFactory>().Create();
            var crewAssignment = GetEntityFactory<CrewAssignment>().Create(new { WorkOrder = workorder, AssignedFor = date });
            var search = new SearchCrewAssignmentSummary {
                AssignedFor = new RequiredDateRange() { Start = date.AddDays(-1), Operator = RangeOperator.GreaterThan },
                OperatingCenter = workorder.OperatingCenter.Id
            };
            var result = _target.Index(search);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, search.Results.Count());
        }
    }
}
