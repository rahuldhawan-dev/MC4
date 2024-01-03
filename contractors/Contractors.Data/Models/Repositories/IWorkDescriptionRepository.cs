using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface IWorkDescriptionRepository : IRepository<WorkDescription>
    {
        IEnumerable<WorkDescription> GetByAssetTypeId(int assetTypeId);
    }
}