using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class ReportUseControllerTest : MapCallMvcControllerTestBase<ReportUseController, ReportViewing>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.FieldServicesWorkManagement;
                const string path = "~/Reports/ReportUse/";
                a.RequiresRole(path + "Search", role);
                a.RequiresRole(path + "Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var user1 = GetFactory<UserFactory>().Create(new { DefaultOperatingCenter = opc1 });
            var user2 = GetFactory<UserFactory>().Create(new { DefaultOperatingCenter = opc2 });
            GetEntityFactory<ReportViewing>().CreateList(2, new {
                User = user1,
                DateViewed = DateTime.Now,
                ReportName = "Foo"
            });
            GetEntityFactory<ReportViewing>().CreateList(5, new {
                User = user2,
                DateViewed = DateTime.Now,
                ReportName = "Bar"
            });

            var search = new SearchReportUse { OperatingCenter = opc1.Id };
            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");

            Assert.AreEqual(2, search.Count);
        }

        #endregion
    }
}
