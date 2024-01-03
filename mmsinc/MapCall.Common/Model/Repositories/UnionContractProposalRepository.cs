using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class UnionContractProposalRepository : MapCallSecuredRepositoryBase<UnionContractProposal>,
        IUnionContractProposalRepository
    {
        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                var critter = base.Criteria
                                  .CreateAlias("Contract", "c")
                                  .CreateAlias("c.OperatingCenter", "oc");
                return CurrentUserCanAccessAllTheRecords
                    ? critter
                    : critter
                       .Add(Restrictions.In("oc.Id", GetUserOperatingCenterIds().ToArray()));
            }
        }

        public override IQueryable<UnionContractProposal> Linq
        {
            get
            {
                return CurrentUserCanAccessAllTheRecords
                    ? base.Linq
                    : (from c in base.Linq
                       where GetUserOperatingCenterIds().ToArray().Contains(c.Contract.OperatingCenter.Id)
                       select c);
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.HumanResourcesUnion; }
        }

        #endregion

        public UnionContractProposalRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public interface IUnionContractProposalRepository : IRepository<UnionContractProposal> { }
}
