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
    public class ChemicalRepository : RepositoryBase<Chemical>, IChemicalRepository
    {
        #region Properties

        public override IQueryable<Chemical> Linq => base.Linq.OrderBy(e => e.Name);

        #endregion

        #region Constructors

        public ChemicalRepository(
            ISession session,
            IContainer container) 
            : base(session, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<Chemical> SearchForApi(ISearchSet<Chemical> search)
        {
            StateOfMatter som = default;

            var query = Session.QueryOver<Chemical>()
                               .JoinAlias(x => x.ChemicalStates, () => som, JoinType.LeftOuterJoin)
                               .TransformUsing(Transformers.DistinctRootEntity);

            return Search(search, query);
        }

        #endregion
    }

    public static class ChemicalRepositoryExtensions
    {
        public static Chemical GetByPartNumber(this IRepository<Chemical> that, string partNumber)
        {
            return that.Where(m => m.PartNumber == partNumber).SingleOrDefault();
        }
    }

    public interface IChemicalRepository : IRepository<Chemical>
    {
        IEnumerable<Chemical> SearchForApi(ISearchSet<Chemical> searchSet);
    }
}
