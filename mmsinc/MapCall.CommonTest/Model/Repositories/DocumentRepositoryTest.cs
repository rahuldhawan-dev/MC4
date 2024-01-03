using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.Documents;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class DocumentRepositoryTest : InMemoryDatabaseTest<Document, DocumentRepository>
    {
        #region Init/Cleanup

        private Mock<IDocumentDataRepository> _dataRepo;

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Singleton().Use(ctx => ctx.GetInstance<InMemoryDocumentService>());
            e.For<IDocumentDataRepository>().Use((_dataRepo = new Mock<IDocumentDataRepository>()).Object);
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSaveSavesDocumentData()
        {
            var user = GetFactory<Common.Testing.Data.UserFactory>().Create();
            var doc = GetFactory<DocumentFactory>().BuildWithConcreteDependencies(new {
                CreatedBy = user,
                UpdatedBy = user
            });

            var expected = doc.DocumentData;
            Assert.IsNotNull(expected, "Sanity check");

            Repository.Save(doc);

            _dataRepo.Verify(x => x.Save(expected));
        }

        #endregion
    }
}
