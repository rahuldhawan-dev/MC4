using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using Moq;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        AuditLogEntryRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<AuditLogEntry, AuditLogEntryRepository>
    {
        #region Fields

        #endregion

        #region SearchLogsForRecord

        [TestMethod]
        public void TestSearchLogsForRecordReturnsResultsThatMatchEitherTheEntityNameOrControllerName()
        {
            var logFactory = GetEntityFactory<AuditLogEntry>();
            var goodLog1 = logFactory.Create(new { EntityId = 1, EntityName = "GoodEntity" });
            var goodLog2 = logFactory.Create(new { EntityId = 1, EntityName = "GoodController" });
            var badLogBecauseEntityNameDoesNotMatchAnything = logFactory.Create(new { EntityId = 1, EntityName = "BadEntity" });
            var badLogBecauseEntityIdDoesNotMatchAnything = logFactory.Create(new { EntityId = 10000, EntityName = "GoodEntity" });

            var search = new TestSearchAuditLogEntryForSingleRecord();
            search.EntityId = 1;
            search.EntityTypeName = "GoodEntity";
            search.ControllerName = "GoodController";

            var result = Repository.SearchLogsForSpecificEntityRecord(search);

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(goodLog1));
            Assert.IsTrue(result.Contains(goodLog2));
        }

        #endregion

        [TestMethod]
        public void TestSearchUnionEntriesOnlyReturnsUnionEntries()
        {
            var user = GetEntityFactory<User>().Create(new {IsAdmin = true});
            var aleFactory = GetFactory<AuditLogEntryFactory>();
            var union = GetEntityFactory<Union>().Create();
            var aleUnion = aleFactory.Create(new {User = user, EntityName = "Union", EntityId = union.Id});

            var aleUnionContract = aleFactory.Create(new {User = user, EntityName = "UnionContract"});
            var aleUnionContractProposal = aleFactory.Create(new {User = user, EntityName = "UnionContractProposal"});
            var aleLocal = aleFactory.Create(new {User = user, EntityName = "Local"});
            var aleGrievance = aleFactory.Create(new {User = user, EntityName = "Grievance"});
            var aleDocumentLinkGrievance = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.Grievance).ToString()
            });
            var aleDocumentLinkContract = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.Contract).ToString()
            });
            var aleDocumentLinkContractProposal = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.ContractProposal).ToString()
            });
            var aleDocumentLinkUnion = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.Union).ToString()
            });
            var aleDocumentLinkLocal = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.Local).ToString()
            });
            var aleInvalid1 = aleFactory.Create(new {User = user, EntityName = "Town"});
            var aleInvalid2 = aleFactory.Create(new
                {User = user, EntityName = "DocumentLink", FieldName = "DataType", NewValue = "55"});

            var search = new EmptySearchSet<AuditLogEntry>();
            var target = Repository.SearchUnionEntries(search);

            Assert.AreEqual(10, target.Count());
            Assert.IsTrue(target.Contains(aleUnion));
            Assert.IsTrue(target.Contains(aleUnionContract));
            Assert.IsTrue(target.Contains(aleUnionContractProposal));
            Assert.IsTrue(target.Contains(aleLocal));
            Assert.IsTrue(target.Contains(aleGrievance));

            Assert.IsTrue(target.Contains(aleDocumentLinkGrievance));
            Assert.IsTrue(target.Contains(aleDocumentLinkContract));
            Assert.IsTrue(target.Contains(aleDocumentLinkContractProposal));
            Assert.IsTrue(target.Contains(aleDocumentLinkUnion));
            Assert.IsTrue(target.Contains(aleDocumentLinkLocal));

            Assert.IsFalse(target.Contains(aleInvalid1));
            Assert.IsFalse(target.Contains(aleInvalid2));
        }

        [TestMethod]
        public void TestGetUniqueEntityNamesGetsUniqueNamesInOrder()
        {
            var user = GetEntityFactory<User>().Create();
            var aleFactory = GetEntityFactory<AuditLogEntry>();
            var aleUnion = aleFactory.Create(new {User = user, EntityName = "Union"});
            var aleUnionContract = aleFactory.Create(new {User = user, EntityName = "UnionContract"});
            var aleUnionContractProposal = aleFactory.Create(new {User = user, EntityName = "UnionContractProposal"});
            var aleLocal = aleFactory.Create(new {User = user, EntityName = "Local"});
            var aleGrievance = aleFactory.Create(new {User = user, EntityName = "Local"});
            var aleDocumentLinkGrievance = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.Grievance).ToString()
            });
            var aleDocumentLinkContract = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.Contract).ToString()
            });
            var aleDocumentLinkContractProposal = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.ContractProposal).ToString()
            });
            var aleDocumentLinkUnion = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.Union).ToString()
            });
            var aleDocumentLinkLocal = aleFactory.Create(new {
                User = user, EntityName = "DocumentLink", FieldName = "DataType",
                NewValue = ((int)AuditLogEntryRepository.UnionDataTypes.Local).ToString()
            });
            var aleTown = aleFactory.Create(new {User = user, EntityName = "Town"});

            var target = Repository.GetUniqueEntityNames().ToList();

            Assert.AreEqual(6, target.Count);
            Assert.AreEqual(aleDocumentLinkContract.EntityName, target[0]);
            Assert.AreEqual(aleLocal.EntityName, target[1]);
            Assert.AreEqual(aleTown.EntityName, target[2]);
            Assert.AreEqual(aleUnion.EntityName, target[3]);
            Assert.AreEqual(aleUnionContract.EntityName, target[4]);
            Assert.AreEqual(aleUnionContractProposal.EntityName, target[5]);
        }
        
        #region Test classes

        private class TestSearchAuditLogEntryForSingleRecord : SearchSet<AuditLogEntry>, ISearchAuditLogEntryForSingleRecord
        {
            public string EntityTypeName { get; set; }
            public int? EntityId { get; set; }
            public string ControllerName { get; set; }
        }

        #endregion
    }
}
