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
    public class DocumentDataMapTest : InMemoryDatabaseTest<Document>
    {
        #region Fields

        private InMemoryDocumentService _docServ;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Use((_docServ = new InMemoryDocumentService()));
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestHashCanNotBeUpdated()
        {
            var data = GetFactory<DocumentDataFactory>().Create();
            var expectedHash = data.Hash;
            data.Hash = "LOL NOPE";
            Session.Save(data);
            Session.Flush();

            Session.Evict(data);

            var dataAgain = Session.Query<DocumentData>().Single(x => x.Id == data.Id);
            Assert.AreEqual(expectedHash, dataAgain.Hash);
        }

        #endregion
    }
}
