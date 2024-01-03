using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class WaterQualityComplaintCountByStateForYearControllerTest : MapCallMvcControllerTestBase<WaterQualityComplaintCountByStateForYearController, WaterQualityComplaint, WaterQualityComplaintRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/WaterQualityComplaintCountByStateForYear/Search", RoleModules.WaterQualityGeneral);
                a.RequiresRole("~/Reports/WaterQualityComplaintCountByStateForYear/Index", RoleModules.WaterQualityGeneral);
            });
        }

        [TestMethod]
        public void TestYearViewDataContainsRelevantYearsAndStates()
        {
            var now = DateTime.Now;
            var state = GetEntityFactory<State>().Create();
            GetEntityFactory<WaterQualityComplaint>().Create(new {
                DateComplaintReceived = DateTime.Now,
                OperatingCenter = GetEntityFactory<OperatingCenter>().Create(new {
                    State = state
                })
            });

            _target.Search();
            var viewYears = _target.ViewData["Year"] as IEnumerable<SelectListItem>;
            var viewStates = _target.ViewData["State"] as IEnumerable<SelectListItem>;

            Assert.AreEqual(now.Year.ToString(), viewYears.First().Value);
            Assert.AreEqual(now.Year.ToString(), viewYears.First().Text);

            Assert.AreEqual(state.Abbreviation, viewStates.First().Value);
            Assert.AreEqual(state.Abbreviation, viewStates.First().Text);
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            Assert.Inconclusive("Repo method uses a stored procedure");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            Assert.Inconclusive("Repo method uses a stored procedure");
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            Assert.Inconclusive("Repo method uses a stored procedure");
        }
    }
}
