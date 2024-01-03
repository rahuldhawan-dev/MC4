using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for DocumentWorkOrderTest.
    /// </summary>
    [TestClass]
    public class DocumentWorkOrderTest
    {
        #region Private Members

        private MockRepository<DocumentWorkOrder> _repository;
        private DocumentWorkOrder _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void DocumentWorkOrderTestInitialize()
        {
            _repository = new MockRepository<DocumentWorkOrder>();
            _target = new TestDocumentWorkOrderBuilder()
                .WithWorkOrder(new WorkOrder())
                .WithDocument(new Document());
        }

        #endregion

        [TestMethod]
        public void TestCreateNewDocumentWorkOrder()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            _target.WorkOrder = null;
            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutDocument()
        {
            _target.Document = null;
            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }
    }

    internal class TestDocumentWorkOrderBuilder : TestDataBuilder<DocumentWorkOrder>
    {

        #region Private Methods

        private WorkOrder _workOrder;
        private Document _document;
    
        #endregion
        
        #region Exposed Methods

        public override DocumentWorkOrder Build()
        {
            var obj = new DocumentWorkOrder();
            if (_workOrder != null)
                obj.WorkOrder = _workOrder;
            if (_document != null)
                obj.Document = _document;
            return obj;
        }

        #endregion

        public TestDocumentWorkOrderBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }

        public TestDocumentWorkOrderBuilder WithDocument(Document document)
        {
            _document = document;
            return this;
        }
    }
}
