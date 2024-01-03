using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class OpeningConditionRepository : RepositoryBase<OpeningCondition>, IOpeningConditionRepository
    {
        public OpeningConditionRepository(
            ISession session,
            IContainer container)
            : base(
                session,
                container) { }

        public override ICriteria Criteria =>
            base.Criteria.Add(Restrictions.Eq("IsActive", true));

        public override IQueryable<OpeningCondition> Linq =>
            base.Linq.Where(x => x.IsActive);
    }

    public interface IOpeningConditionRepository : IRepository<OpeningCondition> { }
}
