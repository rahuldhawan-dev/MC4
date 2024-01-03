using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        #region Fields

        protected readonly IDocumentDataRepository _documentDataRepository;

        #endregion

        #region Constructors

        public DocumentRepository(ISession session, IContainer container,
            IDocumentDataRepository documentDataRepository) : base(session, container)
        {
            _documentDataRepository = documentDataRepository;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public IEnumerable<Document> GetByTableAndDocumentName(string tableName, string docName)
        {
            return
                (from d in Linq
                 where d.FileName.Contains(docName) && d.DocumentType.DataType.TableName == tableName
                 select d);
        }

        public override Document Save(Document entity)
        {
            _documentDataRepository.Save(entity.DocumentData);
            return base.Save(entity);
        }

        #endregion
    }

    public interface IDocumentRepository : IRepository<Document>
    {
        IEnumerable<Document> GetByTableAndDocumentName(string tableName, string docName);
    }
}
