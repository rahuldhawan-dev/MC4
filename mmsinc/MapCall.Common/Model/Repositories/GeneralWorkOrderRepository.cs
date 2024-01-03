using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;
using System.Collections.Generic;
using MMSINC.Utilities;
using NHibernate.SqlCommand;

namespace MapCall.Common.Model.Repositories
{
    // TODO: As mentioned in MC-6260, this repository should go away.
    // Its functionality can be merged into WorkOrderRepository.

    public class GeneralWorkOrderRepository : MapCallSecuredRepositoryBase<WorkOrder>, IGeneralWorkOrderRepository
    {
        #region Constants

        public const int MAX_RESULTS = WorkOrder.MAX_RESULTS;

        #endregion

        #region Properties

        public override RoleModules Role => WorkOrderRepository.ROLE;
        public void SessionEvict(object obj) => base.Session.Evict(obj);

        protected readonly IDateTimeProvider _dateTimeProvider;

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria;
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    crit = crit.Add(Restrictions.In("OperatingCenter.Id", GetUserOperatingCenterIds()));
                }
                return crit.SetMaxResults(MAX_RESULTS);
            }
        }

        #endregion

        #region Exposed Methods

        public ICriterion GetRestorationCriteria()
        {
            return Restrictions.IsNotEmpty("Restorations");
        }

        private static readonly ICriterion NotAssignedContractor = Restrictions.IsNull("AssignedContractor");
        private static readonly ICriterion StreetOpeningPermitRequired = Restrictions.Eq("StreetOpeningPermitRequired", true);
        
        public DetachedCriteria ValidMarkoutIds =>
            DetachedCriteria.For<Markout>("markout")
                            .SetProjection(Projections.Property("markout.WorkOrder"))
                            .Add(Property.ForName("markout.WorkOrder").EqProperty("workorder.Id"))
                            .Add(Restrictions.Ge("ExpirationDate", _dateTimeProvider.GetCurrentDate()));
        
        public DetachedCriteria ValidPermitIds =>
            DetachedCriteria.For<StreetOpeningPermit>("permit")
                            .SetProjection(Projections.Property("permit.WorkOrder.Id"))
                            .Add(Property.ForName("permit.WorkOrder.Id").EqProperty("workorder.Id"))
                            .Add(Restrictions.IsNotNull("DateIssued"))
                            .Add(Restrictions.Ge("ExpirationDate", _dateTimeProvider.GetCurrentDate()));

        private ICriteria GetMarkoutPlanningCriteria => Criteria.CreateAlias("OperatingCenter", "OperatingCenter", JoinType.InnerJoin)
                                                                .Add(Restrictions.Conjunction()
                                                                    .Add(WorkOrderRepository.SAPValidCriteria)
                                                                    .Add(WorkOrderRepository.NotCompleted)
                                                                    .Add(WorkOrderRepository.NotCancelled)
                                                                    .Add(NotAssignedContractor)
                                                                    .Add(WorkOrderRepository.MarkoutRequired)
                                                                    .Add(Restrictions.Not(Subqueries.Exists(ValidMarkoutIds)))
                                                                    .Add(Restrictions.Or(
                                                                         Restrictions.Not(StreetOpeningPermitRequired),
                                                                         Restrictions.And(
                                                                             StreetOpeningPermitRequired,
                                                                             Subqueries.Exists(ValidPermitIds)))));

        public IEnumerable<WorkOrder> SearchRestorationOrders(ISearchSet<WorkOrder> search)
        {
            return Search(search, Criteria.Add(GetRestorationCriteria()));
        }

        public IEnumerable<WorkOrder> SearchMarkoutPlanningOrders(ISearchSet<WorkOrder> search)
        {
            return Search(search, GetMarkoutPlanningCriteria);
        }

        #endregion

        #region Constructors

        public GeneralWorkOrderRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo, IDateTimeProvider dateTimeProvider) : base(session,
            container,
            authenticationService, roleRepo)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion
    }

    public interface IGeneralWorkOrderRepository : IRepository<WorkOrder>
    {
        //needed for 271 to evict an object
        void SessionEvict(object obj);

        IEnumerable<WorkOrder> SearchRestorationOrders(ISearchSet<WorkOrder> search);
        IEnumerable<WorkOrder> SearchMarkoutPlanningOrders(ISearchSet<WorkOrder> search);
    }
}
