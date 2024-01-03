using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallIntranet.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;

namespace MapCallIntranet.Tests
{
    [TestClass]
    public class EmployeeControllerTest : MapCallIntranetControllerTestBase<EmployeeController, Employee, EmployeeRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization() { }

        #region

        [TestMethod]
        public void TestGetEmployeeByReturnsEmpty()
        {
            var state = GetFactory<StateFactory>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state });
            var emp = GetFactory<EmployeeFactory>().Create();

            var result = (AutoCompleteResult)_target.GetEmployeeBy(emp.EmployeeId, opc.Id);
            var data = (IEnumerable<Employee>)result.Data;
            Assert.AreEqual(0, data.Count());
        }

        [TestMethod]
        public void TestGetEmployeeBy()
        {
            var state = GetFactory<StateFactory>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state });
            var emp = GetFactory<EmployeeFactory>().Create(new { OperatingCenter = opc, EmployeeId = "12244" });

            var result = (AutoCompleteResult)_target.GetEmployeeBy(emp.EmployeeId, opc.Id);
            var data = (IEnumerable<Employee>)result.Data;
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(emp.Id, data.Single().Id);
        }

        #endregion
    }
}
