using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class AverageCompletionTimeControllerTest : MapCallMvcControllerTestBase<AverageCompletionTimeController, WorkOrder, WorkOrderRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/AverageCompletionTime/Search", role);
                a.RequiresRole("~/Reports/AverageCompletionTime/Index", role);
            });
        }
        
        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var nj7 = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                OperatingCenterCode = "NJ7",
                OperatingCenterName = "Shrewsbury"
            });
            var workOrder1 = GetFactory<CompletedWorkOrderFactory>().Create(new {
                OperatingCenter = nj7,
                ApprovedOn = DateTime.Now.AddHours(2),
                MaterialsApprovedOn = DateTime.Now.AddHours(3)
            });
            var crewAssignment = GetFactory<ClosedCrewAssignmentFactory>().Create(new {
                WorkOrder = workOrder1
            });
            Session.Flush();
            var search = new SearchAverageCompletionTime {
                OperatingCenter = nj7.Id, StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now.EndOfDay()
            };

            _target.Index(search);
            
            Assert.AreEqual(1, search.Results.Count());
        }
    }
}
