using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class RestorationMethodRepository : RepositoryBase<RestorationMethod>, IRestorationMethodRepository
    {
        #region Constructors

        public RestorationMethodRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<RestorationMethod> GetByRestorationTypeID(int restorationTypeId)
        {
            return Linq.Where(rm => rm.RestorationTypes.Any(rt => rt.Id == restorationTypeId));
        }

        #endregion
    }

    public interface IRestorationMethodRepository : IRepository<RestorationMethod>
    {
        #region Methods

        IEnumerable<RestorationMethod> GetByRestorationTypeID(int id);

        #endregion
    }
}
