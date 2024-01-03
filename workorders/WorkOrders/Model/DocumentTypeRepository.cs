using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class DocumentTypeRepository : WorkOrdersRepository<DocumentType>
    {
        #region Exposed Static Methods

        // Select for workorder
        public static IEnumerable<DocumentType> SelectAllWorkOrderDocumentTypes()
        {
            return
                (from docType in DataTable
                 orderby docType.DocumentTypeName
                 where docType.DataTypeID == DataType.WORK_ORDER_TYPE_ID
                 select docType);
        }
        
        #endregion
    }
}
