using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.Documents;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class DocumentMapTest : InMemoryDatabaseTest<Document>
    {
        #region Fields

        private InMemoryDocumentService _docServ;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Singleton().Use(_docServ = new InMemoryDocumentService());
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDeletingADocumentDoesNotDeleteDocumentDataInstance()
        {
            var doc = GetFactory<DocumentFactory>().Create();
            var data = GetFactory<DocumentDataFactory>().Create();
            doc.DocumentData = data;

            Session.Save(doc);
            Session.Flush();

            Session.Delete(doc);
            Session.Flush();

            Session.Evict(data);

            var dataBestNotBeNull = Session.Query<DocumentData>().Single(x => x.Id == data.Id);
            Assert.IsNotNull(dataBestNotBeNull);

            var docBestBeNull = Session.Query<Document>().SingleOrDefault(x => x.Id == doc.Id);
            Assert.IsNull(docBestBeNull);
        }

        [TestMethod]
        public void TestUpdatingADocumentDoesNotAllowChangingDocumentDataReference()
        {
            var doc = GetFactory<DocumentFactory>().Create();
            var expectedData = doc.DocumentData;

            var newData = GetFactory<DocumentDataFactory>().Create();
            doc.DocumentData = newData;
            Session.Save(doc);

            Session.Evict(doc);
            doc = Session.Query<Document>().Single(x => x.Id == doc.Id);
            Assert.AreEqual(expectedData.Id, doc.DocumentData.Id);
        }

        #endregion
    }
}
