using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.Documents;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class DocumentDataRepositoryTest : InMemoryDatabaseTest<DocumentData, DocumentDataRepository>
    {
        #region Fields

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Singleton().Use<InMemoryDocumentService>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestFindByHashReturnsMatchingInstance()
        {
            var data = GetFactory<DocumentDataFactory>().Create();
            var result = Repository.FindByHash(data.Hash);
            Assert.AreSame(data, result);
        }

        [TestMethod]
        public void TestFindByHashReturnsNullIfNoMatchIsFound()
        {
            Assert.IsNull(Repository.FindByHash("bafouaefh"));
        }

        [TestMethod]
        public void TestFindByBinaryDataReturnsMatchingInstance()
        {
            var data = GetFactory<DocumentDataFactory>().Build();
            Session.Save(data);
            Session.Flush();
            Session.Clear();

            var result = Repository.FindByBinaryData(data.BinaryData);

            Assert.AreEqual(data.Id, result.Id);
        }

        [TestMethod]
        public void TestFindByBinaryDataReturnsNullIfNoMatchIsFound()
        {
            Assert.IsNull(Repository.FindByBinaryData(new byte[] {0, 0, 0, 0, 0, 0, 1, 0, 2, 42}));
        }

        [TestMethod]
        public void TestDeleteThrowsNotSupportedException()
        {
            MyAssert.Throws<NotSupportedException>(() => Repository.Delete(null));
        }

        [TestMethod]
        public void TestSaveSavesBinaryDataToDocumentService()
        {
            var docServ = new Mock<IDocumentService>();
            _container.Inject(docServ.Object);
            var docData = GetFactory<DocumentDataFactory>().Build();
            docServ.Setup(x => x.Save(docData.BinaryData)).Returns("blurp");

            Repository.Save(docData);
            docServ.Verify(x => x.Save(docData.BinaryData));
        }

        [TestMethod]
        public void TestSaveSetsHashOnDocumentDataFromTheHashReturnedByDocumentService()
        {
            var docServ = new Mock<IDocumentService>();
            _container.Inject(docServ.Object);
            var expectedHash = "this isn't really a hash";
            var docData = GetFactory<DocumentDataFactory>().Build();
            docServ.Setup(x => x.Save(docData.BinaryData)).Returns(expectedHash);

            Repository = CreateRepository();

            Assert.AreNotEqual(expectedHash, docData.Hash);
            Repository.Save(docData);
            Assert.AreEqual(expectedHash, docData.Hash);
        }

        [TestMethod]
        public void TestSaveSetsFileSizeOnDocumentDataBasedOnBinaryDataLength()
        {
            var docData = GetFactory<DocumentDataFactory>().Build();
            var expectedFileSize = docData.BinaryData.Length;
            Assert.AreNotEqual(expectedFileSize, 0, "Sanity check.");

            docData.FileSize = 0;

            Repository.Save(docData);
            Assert.AreEqual(expectedFileSize, docData.FileSize);
        }

        [TestMethod]
        public void TestSaveThrowsAnExceptionIfANewDocumentDataBeingSavedMatchesAnExistingRecordsHash()
        {
            var existingDocData = GetFactory<DocumentDataFactory>().Create(new {BinaryData = new byte[] {1}});
            var saving = GetFactory<DocumentDataFactory>().Build(new {BinaryData = new byte[] {1}});
            MyAssert.Throws(() => Repository.Save(saving));
        }

        [TestMethod]
        public void TestSaveShouldNotThrowAnyExceptionsWhenCalledAsAnUpdateBecauseBinaryDataWillBeNull()
        {
            // DocumentData.BinaryData will almost always be null when the Save method is being
            // called to update. this is because BinaryData won't be sitting in memory when a
            // Document is being modified(which then calls the DocumentData repo).
            var docData = GetFactory<DocumentDataFactory>().Create();
            docData.BinaryData = null;
            MyAssert.DoesNotThrow(() => Repository.Save(docData));
        }

        #endregion
    }
}
