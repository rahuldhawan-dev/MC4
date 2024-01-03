using System;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for DocumentTest.
    /// </summary>
    [TestClass]
    public class DocumentTest
    {
        #region Private Members

        private MockRepository<Document> _repository;
        private Document _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void DocumentTestInitialize()
        {
            _repository = new MockRepository<Document>();
            _target = new TestDocumentBuilder()
                .WithDocumentData(new DocumentData())
                .WithFileName("file nane.ext")
                .WithEmployeeCreatedBy(new Employee())
                .WithEmployeeModifiedBy(new Employee())
                .WithDocumentType(new DocumentType());        
        }

        #endregion

        [TestMethod]
        public void TestCreateNewDocument()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutDocumentType()
        {
            _target.DocumentType = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutCreator()
        {
            _target.EmployeeCreatedBy = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCanSaveWithoutModifiedByID()
        {
            _target.EmployeeModifiedBy = null;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCanSaveWithoutModifiedOn()
        {
            _target.ModifiedOn = null;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));

            _target.ModifiedOn = DateTime.MinValue;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutDocumentData()
        {
            _target.DocumentData = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }


        [TestMethod]
        public void TestCannotSaveWithoutFileName()
        {
            _target.FileName = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestSetsCreatedOnOnInsert()
        {
            _repository.InsertNewEntity(_target);

            MyAssert.AreClose(DateTime.Now, _target.CreatedOn);
        }

        [TestMethod]
        public void TestCannotUpdateWithoutValueForModifiedBy()
        {
            _target.EmployeeModifiedBy = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestSetsModifiedOnOnUpdate()
        {
            _repository.UpdateCurrentEntity(_target);

            MyAssert.AreClose(DateTime.Now, _target.ModifiedOn.Value);

            _target.ModifiedOn = DateTime.Now.AddDays(-1);

            _repository.UpdateCurrentEntity(_target);

            MyAssert.AreClose(DateTime.Now, _target.ModifiedOn.Value);
        }
    }

    internal class TestDocumentBuilder : TestDataBuilder<Document>
    {
        #region Private Members

        private DocumentData _docData;
        private int? _fileSize;
        private string _fileName;
        private Employee _employeeCreatedBy, _employeeModifiedBy;
        private DocumentType _documentType;

        #endregion

        #region Exposed Methods

        public override Document Build()
        {
            var obj = new Document();
            if (_docData != null)
                obj.DocumentData = _docData;
            if (!String.IsNullOrEmpty(_fileName))
                obj.FileName = _fileName;
            if (_employeeCreatedBy != null)
                obj.EmployeeCreatedBy = _employeeCreatedBy;
            if (_employeeModifiedBy != null)
                obj.EmployeeModifiedBy = _employeeModifiedBy;
            if (_documentType != null)
                obj.DocumentType = _documentType;
            return obj;
        }

        public TestDocumentBuilder WithDocumentData(DocumentData docData)
        {
            _docData = docData;
            return this;
        }

        public TestDocumentBuilder WithFileName(string s)
        {
            _fileName = s;
            return this;
        }

        public TestDocumentBuilder WithEmployeeCreatedBy(Employee employee)
        {
            _employeeCreatedBy = employee;
            return this;
        }

        #endregion

        public TestDocumentBuilder WithEmployeeModifiedBy(Employee employee)
        {
            _employeeModifiedBy = employee;
            return this;
        }

        public TestDocumentBuilder WithDocumentType(DocumentType documentType)
        {
            _documentType = documentType;
            return this;
        }
    }
}
