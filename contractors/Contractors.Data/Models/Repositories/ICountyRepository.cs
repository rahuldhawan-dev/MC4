using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface ICountyRepository : IRepository<County>
    {
        IEnumerable<County> GetByStateId(int stateId);
    }
}