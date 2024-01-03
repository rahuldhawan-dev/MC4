using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface ITownSectionRepository : IRepository<TownSection>
    {
        IEnumerable<TownSection> GetByTownId(int townId);
    }
}