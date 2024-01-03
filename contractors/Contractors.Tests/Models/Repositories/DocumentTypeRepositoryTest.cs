using System.Linq;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class DocumentTypeRepositoryTest : ContractorsControllerTestBase<DocumentType, DocumentTypeRepository>
    {
        #region Setup/Teardown

        [TestInitialize]
        public void RepositoryTestInitialize()
        {
            Repository = _container.GetInstance<DocumentTypeRepository>();
        }

        #endregion

        [TestMethod]
        public void TestGetAllWorkOrderDocumentTypesReturnsOnlyWorkOrderDocumentTypes()
        {
            var documentTypes = GetFactory<DocumentTypeFactory>().CreateArray(3, new { DataType = typeof(DataTypeFactory) });
            var woDocumentTypes = GetFactory<DocumentTypeFactory>().CreateArray(3, new { DataType = typeof(WorkOrdersDataTypeFactory) });

            var actual = Repository.GetAllWorkOrderDocumentTypes().ToArray();
            Assert.AreEqual(woDocumentTypes.Length, actual.Count());
            foreach (var documentType in documentTypes)
            {
                Assert.IsFalse(actual.Contains(documentType));
            }
        }
    }
}