using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class TownSectionRepository : RepositoryBase<TownSection>, ITownSectionRepository
    {
        #region Constructors

        public TownSectionRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IQueryable<TownSection> GetByTown(params int[] ids)
        {
            var query = ids.Length == 0
                ? Linq
                : Linq.Where(x => ids.Contains(x.Town.Id));

            return query.OrderBy(x => x.Name);
        }

        public IQueryable<TownSection> GetActiveByTown(params int[] ids)
        {
            return GetByTown(ids).Where(x => x.Active);
        }

        #endregion
    }

    public interface ITownSectionRepository : IRepository<TownSection>
    {
        #region Abstract Methods

        IQueryable<TownSection> GetByTown(params int[] ids);
        IQueryable<TownSection> GetActiveByTown(params int[] ids);

        #endregion
    }
}
