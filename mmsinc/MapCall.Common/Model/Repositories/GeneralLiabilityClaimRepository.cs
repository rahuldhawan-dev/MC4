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
    public interface IGeneralLiabilityClaimRepository : IRepository<GeneralLiabilityClaim> { }

    public class GeneralLiabilityClaimRepository : MapCallSecuredRepositoryBase<GeneralLiabilityClaim>,
        IGeneralLiabilityClaimRepository
    {
        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Criteria;
                }

                var opCenterIds = GetUserOperatingCenterIds();
                return base.Criteria.Add(Restrictions.In("OperatingCenter.Id", opCenterIds));
            }
        }

        public override IQueryable<GeneralLiabilityClaim> Linq
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                    return base.Linq;

                var opCenterIds = GetUserOperatingCenterIds();
                return base.Linq.Where(x => opCenterIds.Contains(x.OperatingCenter.Id));
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.OperationsHealthAndSafety; }
        }

        #endregion

        public override void Delete(GeneralLiabilityClaim entity)
        {
            // Action items need to be deleted with the record. Reports based on action items
            // start throwing NHibernate errors when you need to reference the ActionItem.Entity property 
            // and the entity no longer exists. We need to find a better way of dealing with this, if possible.
            // Otherwise we need to implement this deletion for everything that uses action items. -Ross 8/7/2020
            this.DeleteAllActionItems(Session, entity);

            base.Delete(entity);
        }

        public GeneralLiabilityClaimRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }
}
