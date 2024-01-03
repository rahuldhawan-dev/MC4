using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for DocumentTypeTest.
    /// </summary>
    [TestClass]
    public class DocumentTypeTest
    {
        #region Private Members

        private MockRepository<DocumentType> _repository;
        private DocumentType _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void DocumentTypeTestInitialize()
        {
            _repository = new MockRepository<DocumentType>();
            _target = new TestDocumentTypeBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringMethodReturnsDocumentTypeName()
        {
            var documentTypeName = "DocTypeName";

            _target.DocumentTypeName = documentTypeName;
            Assert.AreEqual(documentTypeName, _target.ToString());
        }
    }

    internal class TestDocumentTypeBuilder : TestDataBuilder<DocumentType>
    {
        #region Exposed Methods

        public override DocumentType Build()
        {
            var obj = new DocumentType();
            return obj;
        }

        #endregion
    }
}
