using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCall.Common.Testing.Data;
using MMSINC;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class CrewControllerTest : MapCallMvcControllerTestBase<CrewController, Crew, CrewRepository>
    {
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Crew/ByOperatingCenterOrAll/");
                a.RequiresLoggedInUserOnly("~/Crew/ByOperatingCenterId/");
            });
        }

        [TestMethod]
        public void TestByOperatingCenterOrAll()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var crew1 = GetEntityFactory<Crew>().Create(new { OperatingCenter = opc1 });
            var crew2 = GetEntityFactory<Crew>().Create(new { OperatingCenter = opc2 });
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = opc2 });

            var result = (CascadingActionResult)_target.ByOperatingCenterOrAll(null);
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(2, data.Count());

            result = (CascadingActionResult)_target.ByOperatingCenterOrAll(opc1.Id);
            data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(crew1.Id, data.Single().Id);
        }

        #endregion
    }
}
