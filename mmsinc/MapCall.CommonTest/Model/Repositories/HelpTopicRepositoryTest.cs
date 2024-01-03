using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.Documents;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class HelpTopicRepositoryTest : InMemoryDatabaseTest<HelpTopic, HelpTopicRepository>
    {
        #region Constants
        
        private const string HELP_TOPICS = "HelpTopics";
        
        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Singleton().Use(ctx => ctx.GetInstance<InMemoryDocumentService>());
        }

        [TestInitialize]
        public void InitializeTest() { }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSearchReturnsHelpTopicsWhenNoDocumentCriteriaSelected()
        {
            var helpTopic = GetFactory<HelpTopicFactory>().Create();

            var model = new TestSearchHelpTopic();
            var result = Repository.SearchHelpTopicsWithDocuments(model);

            Assert.IsTrue(result.Contains(helpTopic));
        }

        [DataRow(SearchStringMatchType.Exact, "abc")]
        [DataRow(SearchStringMatchType.Wildcard, "*abc*")]
        [DataRow(SearchStringMatchType.Wildcard, "abc")]
        [DataTestMethod]
        public void TestSearchReturnsHelpTopicsWithDocumentsFilteredByFilename(SearchStringMatchType matchType,
            string searchFilename)
        {
            var helpTopic = CreateHelpTopicWithDocument("abc");
            var helpTopic2 = CreateHelpTopicWithDocument("123 abc xyz");
            var helpTopicWithNoDocuments = GetFactory<HelpTopicFactory>().Create();

            var model = new TestSearchHelpTopic
                { DocumentTitle = new SearchString { MatchType = matchType, Value = searchFilename } };
            var result = Repository.SearchHelpTopicsWithDocuments(model).ToList();

            Assert.IsTrue(result.Contains(helpTopic));
            Assert.AreEqual(matchType == SearchStringMatchType.Wildcard, result.Contains(helpTopic2));
            Assert.IsFalse(result.Contains(helpTopicWithNoDocuments));
        }

        [TestMethod]
        public void TestSearchReturnsHelpTopicsWithDocumentsFilteredByDocumentType()
        {
            var dataType = GetFactory<DataTypeFactory>().Create(new { TableName = HELP_TOPICS });
            var documentType = GetFactory<DocumentTypeFactory>().Create(new { DataType = dataType });
            var helpTopic = CreateHelpTopicWithDocument("abc", documentType);
            var helpTopic2 = CreateHelpTopicWithDocument("123 abc xyz");
            var helpTopicWithNoDocuments = GetFactory<HelpTopicFactory>().Create();

            var model = new TestSearchHelpTopic { DocumentType = new[] { documentType.Id } };
            var result = Repository.SearchHelpTopicsWithDocuments(model).ToList();

            Assert.IsTrue(result.Contains(helpTopic));
            Assert.IsFalse(result.Contains(helpTopic2));
            Assert.IsFalse(result.Contains(helpTopicWithNoDocuments));
        }

        [TestMethod]
        public void TestSearchReturnsHelpTopicsWithDocumentsFilteredByDocumentStatus()
        {
            var documentStatus = GetFactory<DocumentStatusFactory>().Create();
            var helpTopic = CreateHelpTopicWithDocument("abc", ds: documentStatus);
            var helpTopic2 = CreateHelpTopicWithDocument("123 abc xyz");
            var helpTopicWithNoDocuments = GetFactory<HelpTopicFactory>().Create();

            var model = new TestSearchHelpTopic { Active = documentStatus.Id };
            var result = Repository.SearchHelpTopicsWithDocuments(model).ToList();

            Assert.IsTrue(result.Contains(helpTopic));
            Assert.IsFalse(result.Contains(helpTopic2));
            Assert.IsFalse(result.Contains(helpTopicWithNoDocuments));
        }

        [DataRow(RangeOperator.Between, "10/20/1997", "10/20/1997")]
        [DataRow(RangeOperator.Equal, "", "10/20/1997")]
        [DataRow(RangeOperator.GreaterThan, "", "9/10/1996")]
        [DataRow(RangeOperator.GreaterThanOrEqualTo, "", "9/10/1996")]
        [DataRow(RangeOperator.LessThan, "", "12/30/2000")]
        [DataRow(RangeOperator.LessThanOrEqualTo, "", "12/30/2000")]
        [DataTestMethod]
        public void TestSearchReturnsHelpTopicsWithDocumentsFilteredByDocumentUpdatedAt(RangeOperator @operator,
            string start, string end)
        {
            DateTime.TryParse(start, out var startDate);
            DateTime.TryParse(end, out var endDate);
            var helpTopic = CreateHelpTopicWithDocument("abc", updatedAt: DateTime.Parse("10/20/1997"));
            var helpTopic2 = CreateHelpTopicWithDocument("123 abc xyz",
                updatedAt: @operator == RangeOperator.LessThan || @operator == RangeOperator.LessThanOrEqualTo
                    ? DateTime.Parse("12/31/2111")
                    : (DateTime?)null);
            var helpTopicWithNoDocuments = GetFactory<HelpTopicFactory>().Create();
            var model = new TestSearchHelpTopic {
                DocumentUpdated = new DateRange {
                    Start = startDate,
                    End = endDate,
                    Operator = @operator
                }
            };
            var result = Repository.SearchHelpTopicsWithDocuments(model).ToList();

            Assert.IsTrue(result.Contains(helpTopic));
            Assert.IsFalse(result.Contains(helpTopic2));
            Assert.IsFalse(result.Contains(helpTopicWithNoDocuments));
        }

        private HelpTopic CreateHelpTopicWithDocument(string filename, DocumentType dt = null, DocumentStatus ds = null,
            DateTime? createdAt = null, DateTime? updatedAt = null, DateTime? nextReviewDate = null)
        {
            var dataType = GetFactory<DataTypeFactory>().Create(new { TableName = HELP_TOPICS });
            var documentType = dt ?? GetFactory<DocumentTypeFactory>().Create(new { DataType = dataType });
            var documentStatus = ds ?? GetFactory<DocumentStatusFactory>().Create();
            var document = GetFactory<DocumentFactory>().Create(new {
                FileName = filename,
                DocumentType = documentType,
                CreatedAt = createdAt ?? DateTime.MinValue,
            });
            var helpTopic = GetFactory<HelpTopicFactory>().Create();
            GetFactory<DocumentLinkFactory>().Create(new {
                document.DocumentType,
                document.DocumentType.DataType,
                Document = document,
                LinkedId = helpTopic.Id,
                DocumentStatus = documentStatus,
                ReviewFrequency = 1,
                ReviewFrequencyUnit = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create(),
                UpdatedAt = updatedAt ?? DateTime.MinValue,
                NextReviewDate = nextReviewDate ?? DateTime.MinValue,
            });
            return helpTopic;
        }

        #endregion

        #region Private Members

        private class TestSearchHelpTopic : SearchSet<HelpTopic>, ISearchHelpTopicWithDocument
        {
            public DateRange DocumentUpdated { get; set; }

            public DateRange NewlyUpdated { get; set; }

            public SearchString DocumentTitle { get; set; }

            public int? Active { get; set; }

            public int[] DocumentType { get; set; }

            public DateRange DocumentNextReviewDate { get; set; }
        }

        #endregion
    }
}
