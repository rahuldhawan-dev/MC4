using System.Linq;
using System.Web.Mvc;
using MMSINC.Utilities.Documents;
using MMSINC.Data.Linq;
using StructureMap;

namespace WorkOrders.Model
{
    public class DocumentDataRepository : WorkOrdersRepository<DocumentData>, IDocumentDataRepository
    {
        #region Private Methods

        private DocumentData FindByHash(string hash)
        {
            return DataTable.Where(x => x.Hash == hash).SingleOrDefault();
        }

        private DocumentData FindByBinaryData(byte[] binaryData)
        {
            var docServ = DependencyResolver.Current.GetService<IDocumentService>();
            var hash = docServ.GetFileHash(binaryData);
            return FindByHash(hash);
        }

        #endregion

        #region Public Methods

        public DocumentData SaveOrGetExisting(byte[] binaryData)
        {
            var docData = FindByBinaryData(binaryData);
            if (docData == null)
            {
                docData = new DocumentData();
                docData.FileSize = binaryData.Length;
                var docServ = DependencyResolver.Current.GetService<IDocumentService>();
                docData.Hash = docServ.Save(binaryData);
                InsertNewEntity(docData);
            }

            return docData;
        }

        #endregion
    }

    public interface IDocumentDataRepository : IRepository<DocumentData>
    {
        DocumentData SaveOrGetExisting(byte[] binaryData);
    }
}
