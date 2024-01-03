using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface IMaterialRepository : IRepository<Material>
    {
        #region Methods

        IEnumerable<Material> GetBySearchAndOperatingCenterId(string search, int operatingCenterID, bool? isActive = true);

        IEnumerable<Material> GetBySearchAndMaterialUsedId(string search,
            int materialUsedId, bool? isActive = true);

        IEnumerable<Material> GetBySearchAndWorkOrderId(string search,
            int workOrderId, bool? isActive = true);

        #endregion
    }
}