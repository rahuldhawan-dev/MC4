using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface ITownRepository : IRepository<Town> 
    {
        IEnumerable<Town> GetByOperatingCenterId(int operatingCenterId);
        IEnumerable<Town> GetByCountyId(int stateId);
    }
}