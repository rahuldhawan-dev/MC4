using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class SpoilRemovalRepository : RepositoryBase<SpoilRemoval>, ISpoilRemovalRepository
    {
        #region Constants

        #endregion

        #region Properties

        public override ICriteria Criteria =>
            base.Criteria
                .CreateAlias("RemovedFrom", "rf", JoinType.LeftOuterJoin)
                .CreateAlias("rf.OperatingCenter", "oc", JoinType.LeftOuterJoin)
                .CreateAlias("oc.State", "st", JoinType.LeftOuterJoin);
        #endregion

        #region Constructors

        public SpoilRemovalRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion
    }

    public interface ISpoilRemovalRepository : IRepository<SpoilRemoval> { }
}


