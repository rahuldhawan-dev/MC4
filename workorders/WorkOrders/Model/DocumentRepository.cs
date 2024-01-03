using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.Linq;

namespace WorkOrders.Model
{
    public class DocumentRepository : WorkOrdersRepository<Document>, IDocumentRepository
    {
        #region Exposed Static Methods

        // Select for workorder
        public static IEnumerable<Document> GetDocumentsForWorkOrder(int workOrderID)
        {
            return
                (from doc in DataTable
                 where (from docOrder in doc.DocumentsWorkOrders
                        where docOrder.WorkOrderID == workOrderID
                        select docOrder).Count() > 0
                 select doc);
        }

        public static void UpdateDocument(int documentID, string fileName, int documentTypeID, DateTime modifiedOn, int modifiedByID)
        {
            var doc = GetEntity(documentID);

            doc.FileName = fileName;
            doc.DocumentTypeID = documentTypeID;
            doc.ModifiedOn = modifiedOn;
            doc.ModifiedByID = modifiedByID;

            Update(doc);
        }

        #endregion

        #region Public Methods

        public void InsertDocumentForWorkOrder(Document document, int workOrderID)
        {
            var dwo = new DocumentWorkOrder {
                Document = document,
                WorkOrderID = workOrderID
            };
            Insert(document);
        }

        #endregion
    }

    public interface IDocumentRepository : IRepository<Document>
    {
        #region Methods

        void InsertDocumentForWorkOrder(Document document, int workOrderID);

        #endregion
    }
}
