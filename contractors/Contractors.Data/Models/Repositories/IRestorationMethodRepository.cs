using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface IRestorationMethodRepository : IRepository<RestorationMethod>
    {
        #region Methods

        IEnumerable<RestorationMethod> GetByRestorationTypeID(int id);

        #endregion
    }
}