using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Documents;
using Moq;
using StructureMap;
using DocumentRepository = Contractors.Data.Models.Repositories.DocumentRepository;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class DocumentRepositoryTest : ContractorsControllerTestBase<Document, DocumentRepository>
    {
        #region Setup/Teardown

        private Mock<IDocumentDataRepository> _dataRepo;

        protected override void InitializeObjectFactory(
            ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Use<InMemoryDocumentService>();
            _dataRepo = e.For<IDocumentDataRepository>().Mock();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            Repository = _container.GetInstance<DocumentRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSaveSavesDocumentData()
        {
            var doc = GetFactory<DocumentFactory>().Build(new {
                CreatedByStr = "Some guy",
                ModifiedByStr = "Another guy"
            });

            var expected = doc.DocumentData;
            Assert.IsNotNull(expected, "Sanity check");

            Repository.Save(doc);

            _dataRepo.Verify(x => x.Save(expected));
        }

        #endregion
    }
}
