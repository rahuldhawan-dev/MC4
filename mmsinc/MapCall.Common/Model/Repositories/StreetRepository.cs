using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class StreetRepository : RepositoryBase<Street>, IStreetRepository
    {
        #region Constructors

        public StreetRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public Street FindByTownIdAndStreetName(int townId, string streetPrefix, string streetName, string streetSuffix)
        {
            return
                Linq.SingleOrDefault(
                    x =>
                        x.Town.Id == townId &&
                        x.Name == streetName &&
                        ((x.Prefix == null && streetPrefix == null)
                         || x.Prefix.Description == streetPrefix) &&
                        ((x.Suffix == null && streetSuffix == null)
                         || x.Suffix.Description == streetSuffix));
        }

        public IQueryable<Street> GetByTown(params int[] ids)
        {
            return ids.Length == 0 
                ? Linq
                : Linq.Where(x => ids.Contains(x.Town.Id));
        }

        /// <summary>
        /// Used for more effienct queries against SQL
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<Street> SearchForApi(ISearchSet<Street> search)
        {
            StreetPrefix prefix = default;
            StreetSuffix suffix = default;
            var query = Session.QueryOver<Street>()
                               .JoinAlias(x => x.Prefix, () => prefix, JoinType.LeftOuterJoin)
                               .JoinAlias(x => x.Suffix, () => suffix, JoinType.LeftOuterJoin)
                               .TransformUsing(Transformers.DistinctRootEntity);
            
            return Search(search, query);
        }

        #endregion
    }

    public interface IStreetRepository : IRepository<Street>
    {
        IQueryable<Street> GetByTown(params int[] ids);
        Street FindByTownIdAndStreetName(int townId, string streetPrefix, string streetName, string streetSuffix);
        IEnumerable<Street> SearchForApi(ISearchSet<Street> search);
    }
}
