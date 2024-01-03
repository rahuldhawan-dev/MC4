using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class DocumentTypeRepository : RepositoryBase<DocumentType>, IDocumentTypeRepository
    {
        #region Constructors

        public DocumentTypeRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<DocumentType> GetByTableName(string tableName)
        {
            return (from dt in Linq where dt.DataType.TableName == tableName select dt);
        }

        public IEnumerable<DocumentType> GetAllWorkOrderDocumentTypes()
        {
            return GetByTableName("WorkOrders");
        }

        public IEnumerable<DocumentType> GetAllRestorationDocumentTypes()
        {
            return GetByTableName("Restorations");
        }

        public IEnumerable<DocumentType> GetAllMeterChangeOutDocumentTypes()
        {
            return GetByTableName("MeterChangeOuts");
        }

        #endregion
    }

    public interface IDocumentTypeRepository : IRepository<DocumentType>
    {
        #region Abstract Methods

        IEnumerable<DocumentType> GetByTableName(string tableName);
        IEnumerable<DocumentType> GetAllWorkOrderDocumentTypes();
        IEnumerable<DocumentType> GetAllRestorationDocumentTypes();
        IEnumerable<DocumentType> GetAllMeterChangeOutDocumentTypes();

        #endregion
    }
}
