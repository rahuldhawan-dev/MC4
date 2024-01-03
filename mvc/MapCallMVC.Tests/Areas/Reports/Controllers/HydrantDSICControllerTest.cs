using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class HydrantDSICControllerTest : MapCallMvcControllerTestBase<HydrantDSICController, Hydrant, HydrantRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/HydrantDSIC/Search", role);
                a.RequiresRole("~/Reports/HydrantDSIC/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var hydrant = GetEntityFactory<Hydrant>().Create(new {
                WorkOrderNumber = "R18-18B1.17-P-0019", DateInstalled = DateTime.Now
            });
            var search = new SearchHydrantDSIC();

            var result = _target.Index(search) as ViewResult;

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(hydrant.HydrantNumber, search.Results.Single().HydrantNumber);
        }
    }
}
