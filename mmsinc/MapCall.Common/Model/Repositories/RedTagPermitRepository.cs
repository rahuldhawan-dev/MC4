using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using System.Linq;
using MapCall.Common.Data;
using NHibernate;
using StructureMap;
using MMSINC.Authentication;
using MapCall.Common.Model.Entities.Users;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace MapCall.Common.Model.Repositories
{
    public class RedTagPermitRepository : MapCallSecuredRepositoryBase<RedTagPermit>, IRedTagPermitRepository
    {
        #region Properties

        public override RoleModules Role => RoleModules.ProductionWorkManagement;

        public override ICriteria Criteria
        {
            get
            {
                var criteria = base.Criteria
                                   .CreateAlias("Equipment", "criteriaEquipment", JoinType.LeftOuterJoin)
                                   .CreateAlias("criteriaEquipment.Facility", "criteriaFacility", JoinType.LeftOuterJoin)
                                   .CreateAlias("criteriaFacility.OperatingCenter", "criteriaOperatingCenter", JoinType.LeftOuterJoin)
                                   .CreateAlias("criteriaOperatingCenter.State", "criteriaState", JoinType.LeftOuterJoin);

                if (CurrentUserCanAccessAllTheRecords)
                {
                    return criteria;
                }

                var operatingCenterIds = GetUserOperatingCenterIds();
                criteria = criteria.Add(Restrictions.In("criteriaOperatingCenter.Id", operatingCenterIds));

                return criteria;
            }
        }

        public override IQueryable<RedTagPermit> Linq
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Linq;
                }

                var operatingCenterIds = GetUserOperatingCenterIds();
                return base.Linq
                           .Where(x => operatingCenterIds.Contains(x.Equipment.Facility.OperatingCenter.Id));
            }
        }

        #endregion

        #region Constructor

        public RedTagPermitRepository(
            ISession session,
            IContainer container,
            IAuthenticationService<User> authenticationService, 
            IRepository<AggregateRole> roleRepository) : base(session, container, authenticationService, roleRepository) { }

        #endregion
    }

    public interface IRedTagPermitRepository : IRepository<RedTagPermit> { }
}