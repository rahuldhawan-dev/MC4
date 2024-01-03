using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class InspectionProductivityControllerTest : MapCallMvcControllerTestBase<InspectionProductivityController, HydrantInspection, HydrantInspectionRepository>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/InspectionProductivity/Index");
                a.RequiresLoggedInUserOnly("~/Reports/InspectionProductivity/Search");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var expectedDate = DateTime.Today;
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var hydrant = GetFactory<HydrantFactory>().Create(new{ OperatingCenter = opc });
            var valve = GetFactory<ValveFactory>().Create(new { OperatingCenter = opc, ValveSize = GetEntityFactory<ValveSize>().Create() });
            var hi = GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant, DateInspected = expectedDate, HydrantInspectionType = typeof(HydrantInspectionTypeFactory), InspectedBy = typeof(UserFactory) });
            var vi = GetFactory<ValveInspectionFactory>().Create(new { Valve = valve, DateInspected = expectedDate, InspectedBy = typeof(UserFactory) });
            var bi = GetFactory<BlowOffInspectionFactory>().Create(new { Valve = valve, DateInspected = expectedDate, HydrantInspectionType = typeof(HydrantInspectionTypeFactory), InspectedBy = typeof(UserFactory) });

            var search = new SearchInspectionProductivity
            {
                OperatingCenter = opc.Id,
                StartDate = expectedDate
            };

            _target.Index(search);
            Assert.AreEqual(3, search.Count);
        }

        #endregion
    }
}
