using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class AuditLogEntryControllerTest : MapCallMvcControllerTestBase<AuditLogEntryController, AuditLogEntry, AuditLogEntryRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/AuditLogEntry/SecureIndexForSingleRecord");
            });
        }

        #region Search

        [TestMethod]
        public void TestSecureSearchCanDoSearchThings()
        {
            RunActionCanPerformSearchWithSearchModel("SecureIndexForSingleRecord",
                (searchModel) => { _target.SecureIndexForSingleRecord(searchModel); });
        }

        [TestMethod]
        public void TestSecureIndexForSingleRecordReturnsEmptyResultIfThereAreNoResults()
        {
            InitializeControllerAndRequest("~/AuditLogEntry/SecureIndexForSingleRecord.frag");           
            var result = _target.SecureIndexForSingleRecord(new SecureSearchAuditLogEntryForSingleRecord {
                EntityId = 0,
                EntityTypeName = "Blah blah"
            });

            Assert.IsInstanceOfType(result, typeof(EmptyResult));
        }

        [TestMethod]
        public void TestSecureIndexForSingleRecordSearchsCorrectlyWhenEntitiesHaveSimilarNames()
        {
            // MC-561: Valve logs were including ValveInspections because it was doing a wildcard search.

            InitializeControllerAndRequest("~/AuditLogEntry/SecureIndexForSingleRecord.frag");
            var valveEntry = GetFactory<AuditLogEntryFactory>().Create(new { EntityName = "Valve", EntityId = 1 });
            var valveInspectionEntry = GetFactory<AuditLogEntryFactory>().Create(new { EntityName = "ValveInspection", EntityId = 1 });
            var search = new SecureSearchAuditLogEntryForSingleRecord {
                EntityTypeName = "Valve",
                EntityId = 1
            };

            _target.SecureIndexForSingleRecord(search);

            Assert.IsTrue(search.Results.Contains(valveEntry));
            Assert.IsFalse(search.Results.Contains(valveInspectionEntry));
        }

        #endregion
    }
}
