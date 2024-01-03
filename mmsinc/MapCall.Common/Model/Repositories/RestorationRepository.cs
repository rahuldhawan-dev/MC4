using MMSINC.Data.NHibernate;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class RestorationRepository : RepositoryBase<Restoration>, IRestorationRepository
    {
        #region Constructor

        public RestorationRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<Restoration> SearchRestorationsForAccrualReport(ISearchSet<Restoration> search)
        {
            var query = Session.QueryOver<Restoration>()
                               .Where(x => x.WorkOrder.ApprovedOn != null &&
                                           x.FinalRestorationDate == null);

            return Search(search, query);
        }

        #endregion
    }

    public interface IRestorationRepository : IRepository<Restoration>
    {
        IEnumerable<Restoration> SearchRestorationsForAccrualReport(ISearchSet<Restoration> search);
    }
}