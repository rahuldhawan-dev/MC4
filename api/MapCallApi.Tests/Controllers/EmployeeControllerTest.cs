using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public  class EmployeeControllerTest : MapCallApiControllerTestBase<EmployeeController, Employee, EmployeeRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.HumanResourcesEmployee;

            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/Employee/Index", module);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults() { }

        [TestMethod]
        public void Test_Index_ReturnsProperJson()
        {
            var state = GetEntityFactory<State>().Create();
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var activeEmployeeStatus = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {State = state});
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter, PublicWaterSupply = publicWaterSupply });
            var reportsTo = GetEntityFactory<Employee>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { ReportingFacility = facility, OperatingCenter = operatingCenter, Status = activeEmployeeStatus, ReportsTo = reportsTo });

            var search = new SearchEmployee{State = state.Id, PublicWaterSupply = publicWaterSupply.Id, ReportingFacility = facility.Id};

            var result = (JsonResult)_target.Index(search);
            var helper = new JsonResultTester(result);

            helper.AreEqual(employee.Id, nameof(employee.Id));
            helper.AreEqual(employee.Status.Description, nameof(employee.Status));
            helper.AreEqual(employee.PositionGroup.Description, nameof(employee.PositionGroup));
            helper.AreEqual(employee.FirstName, nameof(employee.FirstName));
            helper.AreEqual(employee.LastName, nameof(employee.LastName));
            helper.AreEqual(employee.EmployeeId, nameof(employee.EmployeeId));
            helper.AreEqual(employee.OperatingCenter.Description, nameof(employee.OperatingCenter));
            helper.AreEqual(employee.EmailAddress, nameof(employee.EmailAddress));
            helper.AreEqual(employee.ReportsToName, nameof(employee.ReportsToName));
            helper.AreEqual(employee.ReportsToEmployeeId, nameof(employee.ReportsToEmployeeId));
        }
    }
}
