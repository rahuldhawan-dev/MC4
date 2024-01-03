using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class IncidentsOSHARecordableSummaryControllerTest : MapCallMvcControllerTestBase<IncidentsOSHARecordableSummaryController, Incident, IncidentRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = IncidentsOSHARecordableSummaryController.ROLE;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/IncidentsOSHARecordableSummary/Search", role);
                a.RequiresRole("~/Reports/IncidentsOSHARecordableSummary/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public void TestIndexReturnsViewWithPdfOfOnlyOSHAIncidents()
        {
            _currentUser.IsAdmin = true;
            DateTime timeToUse = new DateTime(2012, 2, 4, 10, 15, 30);
            var entity0 = GetFactory<IncidentFactory>().Create(new{ IsOSHARecordable = true });
            var entity1 = GetFactory<IncidentFactory>().Create(new { IsOSHARecordable = true });

            var search = new SearchIncidentOSHARecordableSummary {
                OperatingCenter = new[] { entity0.OperatingCenter.Id },
                IncidentDate = new MMSINC.Data.DateRange { Start = timeToUse, End = DateTime.Now }
            };

            var result = (PdfResult)_target.Index(search);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, search.Results.Count());

            entity0.IsOSHARecordable = false;
            entity1.IsOSHARecordable = false;
            Session.Save(entity0);
            Session.Save(entity1);
            Session.Flush();
            result = (PdfResult)_target.Index(search);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, search.Results.Count());
        }

        [TestMethod]
        public void TestIndexDoesntReturnPdfResultsWhenNoOSHAIncidents()
        {
            _currentUser.IsAdmin = true;
            var entity0 = GetFactory<IncidentFactory>().Create();
            var entity1 = GetFactory<IncidentFactory>().Create();
            var search = new SearchIncidentOSHARecordableSummary
            {
                OperatingCenter = new[] { entity0.OperatingCenter.Id },
                IncidentDate = new MMSINC.Data.DateRange { Start = DateTime.Now, End = DateTime.Now }
            };

            var result = (PdfResult)_target.Index(search);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, search.Results.Count());
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            Assert.Inconclusive("Test me. I return a pdf when there aren't results");
        }
    }
}