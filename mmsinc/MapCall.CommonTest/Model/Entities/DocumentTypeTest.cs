using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class DocumentTypeTest
    {
        #region Private Members

        private DocumentType _target;

        #endregion

        [TestInitialize]
        public void DocumentTypeTestInitialize()
        {
            _target = new DocumentType();
        }

        [TestMethod]
        public void TestToStringReturnsDocumentTypeName()
        {
            var description = "some description";
            _target.Name = description;

            Assert.AreEqual(description, _target.ToString());
        }
    }
}
