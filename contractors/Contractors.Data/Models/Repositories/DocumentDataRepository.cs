using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.Documents;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    // If here by now then bad place be.  Trouble time for you when delete repo.
    public class DocumentDataRepository : SecuredRepositoryBase<DocumentData, ContractorUser>, IDocumentDataRepository
    {
        #region Fields

        protected readonly IDocumentService _docServ;

        #endregion

        #region Constructors

        public DocumentDataRepository(ISession session,
            IAuthenticationService<ContractorUser> authenticationService,
            IContainer container, IDocumentService docServ) : base(session,
            authenticationService, container)
        {
            _docServ = docServ;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an existing DocumentData instance if one exists for the given hash.
        /// </summary>
        public DocumentData FindByHash(string hash)
        {
            return Linq.SingleOrDefault(x => x.Hash == hash);
        }

        /// <summary>
        /// Returns an existing DocumentData instance that has a hash 
        /// that matches the hash of the binaryData given.
        /// </summary>
        public DocumentData FindByBinaryData(byte[] binaryData)
        {
            var hash = _docServ.GetFileHash(binaryData);
            return FindByHash(hash);
        }

        public override void Delete(DocumentData entity)
        {
            throw new NotSupportedException("Deleting DocumentData is not supported. This is the sweeper service's job.");
        }

        public override DocumentData Save(DocumentData entity)
        {
            // The mapping prevents this from being updated, but we also
            // don't want exceptions being thrown when a Document is modified
            // and the DocumentRepository calls Save on this. A modified Document
            // will almost never have a DocumentData with its BinaryData
            // loaded into memory.
            if (entity.Id == 0)
            {
                entity.Hash = _docServ.Save(entity.BinaryData);
                entity.FileSize = entity.BinaryData.Length;
            }
           
            return base.Save(entity);
        }

        #endregion
    }
}