using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface IStreetRepository : IRepository<Street>
    {
        #region Methods

        IEnumerable<Street> GetByTownId(int townId);

        #endregion
    }
}