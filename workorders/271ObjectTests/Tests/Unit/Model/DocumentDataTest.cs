using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    [TestClass]
    public class DocumentDataTest
    {
        #region Private Members

        private MockRepository<DocumentData> _repository;
        private DocumentData _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void DocumentTestInitialize()
        {
            _repository = new MockRepository<DocumentData>();
            _target = new TestDocumentDataBuilder()
                .WithHash("1234567890123456789012345678901234567890")
                .WithFileSize(10);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestCreateNewDocumentData()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithFileSizeLessThanOrEqualToZero()
        {
            _target.FileSize = 0;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));

            _target.FileSize = -1;
            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithNullOrEmptyOrWhiteSpaceHash()
        {
            _target.Hash = null;
            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));

            _target.Hash = string.Empty;
            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));

            _target.Hash = "     ";
            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveHashThatIsNot40Characters()
        {
            _target.Hash = "123456789012345678901234567890123456789";
            Assert.AreEqual(39, _target.Hash.Length);
            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));

            _target.Hash = "12345678901234567890123456789012345678901";
            Assert.AreEqual(41, _target.Hash.Length);
            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }
        

        #endregion
    }

    internal class TestDocumentDataBuilder : TestDataBuilder<DocumentData>
    {
        #region Private Members

        private int? _fileSize;
        private string _hash;

        #endregion

        #region Exposed Methods

        public override DocumentData Build()
        {
            var obj = new DocumentData();
            obj.Hash = _hash;
            obj.FileSize = _fileSize.GetValueOrDefault();
            return obj;
        }

        public TestDocumentDataBuilder WithFileSize(int size)
        {
            _fileSize = size;
            return this;
        }

        public TestDocumentDataBuilder WithHash(string hash)
        {
            _hash = hash;
            return this;
        }

        #endregion

    }
}
