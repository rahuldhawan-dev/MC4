using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        IEnumerable<Document> GetByTableAndDocumentName(string tableName, string docName);
    }
}