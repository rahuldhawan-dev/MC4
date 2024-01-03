using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class DocumentRepository : SecuredRepositoryBase<Document, ContractorUser>, IDocumentRepository
    {
        #region Fields

        private readonly IDocumentDataRepository _documentDataRepository;

        #endregion

        #region Constructors

        public DocumentRepository(ISession session,
            IAuthenticationService<ContractorUser> authenticationService,
            IContainer container,
            IDocumentDataRepository documentDataRepository) : base(session,
            authenticationService, container)
        {
            _documentDataRepository = documentDataRepository;
        }

        #endregion

        #region Exposed Methods

        public override Document Save(Document entity)
        {
            _documentDataRepository.Save(entity.DocumentData);
            return base.Save(entity);
        }

        public IEnumerable<Document> GetByTableAndDocumentName(string tableName, string docName)
        {
            return
                (from d in Linq
                 where d.FileName.Contains(docName) && d.DocumentType.DataType.TableName == tableName
                 select d);
        }

        #endregion
    }
}
