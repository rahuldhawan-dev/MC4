using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface IServiceMaterialRepository : IRepository<ServiceMaterial>
    {
        IEnumerable<ServiceMaterial> GetAllButUnknown();
    }
}