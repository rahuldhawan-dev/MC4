using System;
using System.Collections.Generic;
using System.Linq;
using Contractors.Data.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MapCall.Common.Utility;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using StructureMap;
using NHibernate.Linq.Util;
using MapCall.Common.Model.Mappings;

namespace Contractors.Data.Models.Repositories
{
    public class WorkOrderRepository : SecuredRepositoryBase<WorkOrder, ContractorUser>, IWorkOrderRepository
    {
        #region Constants

        // NOTE: DO NOT MAKE STATIC FIELDS THAT USE DATETIME.NOW!!!! -Ross 4/5/2016

        public static readonly ICriterion NotCompleted = Restrictions.IsNull("DateCompleted");
        public static readonly ICriterion MarkoutRequired = Restrictions.Not(Restrictions.Eq("MarkoutRequirement.Id", (int)MarkoutRequirementEnum.None));
        public static readonly ICriterion NotApproved = Restrictions.And(Restrictions.IsNull("ApprovedOn"), Restrictions.IsNull("ApprovedBy"));
        public static readonly ICriterion EmergencyPriority = Restrictions.Eq("Priority.Id", (int)WorkOrderPriorityEnum.Emergency);
        public static readonly ICriterion NotCancelled = Restrictions.IsNull("CancelledAt");

        public ICriterion StreetOpeningPermitRequired => Restrictions.Eq("StreetOpeningPermitRequired", true);

        // We can schedule for markouts in the future
        public DetachedCriteria ValidMarkoutIds
        {
            get
            {
                return DetachedCriteria.For<Markout>("markout")
                     .SetProjection(Projections.Property("markout.WorkOrder"))
                     .Add(Property.ForName("markout.WorkOrder").EqProperty("workorder.Id"))
                     .Add(Restrictions.Ge("ExpirationDate", DateTime.Now));
            }
        }

        // We use this because we don't want to allow them to start work if it isn't current
        public DetachedCriteria CurrentValidMarkoutIds
        {
            get
            {
                return DetachedCriteria.For<Markout>("markout")
                  .SetProjection(Projections.Property("markout.WorkOrder"))
                  .Add(Property.ForName("markout.WorkOrder").EqProperty("workorder.Id"))
                  .Add(Restrictions.Ge("ExpirationDate", DateTime.Now))
                  .Add(Restrictions.Le("ReadyDate", DateTime.Now));
            }
        }

        public DetachedCriteria ValidPermitIds
        {
            get
            {
                return DetachedCriteria.For<StreetOpeningPermit>("permit")
                .SetProjection(Projections.Property("permit.WorkOrder.Id"))
                .Add(Property.ForName("permit.WorkOrder.Id").EqProperty("workorder.Id"))
                .Add(Restrictions.IsNotNull("DateIssued"))
                .Add(Restrictions.Ge("ExpirationDate", DateTime.Now));
            }
        }

        public DetachedCriteria ValidCrewAssignments
        {
            get
            {
                return DetachedCriteria.For<CrewAssignment>("crewassignment")
             .SetProjection(Projections.Property("crewassignment.WorkOrder.Id"))
             .Add(Property.ForName("crewassignment.WorkOrder.Id").EqProperty("workorder.Id"))
             .Add(Restrictions.Or(
                 Restrictions.Le("AssignedFor", DateTime.Now),
                 Restrictions.IsNotNull("DateStarted")
             ))
             .CreateAlias("Crew", "crew", JoinType.InnerJoin)
             .Add(Property.ForName("crewassignment.Crew.Id").EqProperty("crew.Id"))
             .Add(Property.ForName("workorder.AssignedContractor").EqProperty("crew.Contractor"));
            }
        }

        #endregion

        #region Constructors

        public WorkOrderRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Properties

        #region Base Linq/Criteria

        public override IQueryable<WorkOrder> Linq => base.Linq.Where(wo => wo.AssignedContractor == CurrentUser.Contractor);

        public override ICriteria Criteria => base.Criteria.Add(AssignedContractor);

        #endregion

        #region Criteria

        public ICriteria FinalizationOrders
        {
            get { return Criteria.Add(GetFinalizationCriteria()); }
        }

        // added to satisfy the interface, but not needed for the contractors site
        public ICriteria PrePlanningOrders => throw new NotImplementedException();

        public ICriteria PlanningOrders
        {
            get { return Criteria.Add(GetPlanningCriteria()); }
        }

        public ICriteria SchedulingOrders
        {
            get { return Criteria.Add(GetSchedulingCriteria()); }
        }

        public ICriteria GeneralOrders
        {
            get { return Criteria.Add(GetGeneralCriteria()); }
        }

        public ICriterion AssignedContractor
        {
            get
            {
                return Restrictions.Eq("AssignedContractor",
                    CurrentUser.Contractor);
            }
        }

        #endregion

        #endregion

        #region Exposed Methods

        public ICriterion GetPlanningCriteria()
        {
            return Restrictions.And(
                AssignedContractor, 
                Restrictions.And(
                    NotCancelled,
                Restrictions.And(
                    NotCompleted,
                    Restrictions.Or(
                        Restrictions.And(
                            MarkoutRequired,
                            Restrictions.Not(Subqueries.Exists(ValidMarkoutIds))
                        ),
                        Restrictions.And(
                            StreetOpeningPermitRequired,
                            Restrictions.Not(Subqueries.Exists(ValidPermitIds)))
                        )
                    )));
        }

        public ICriterion GetSchedulingCriteria()
        {
            return Restrictions.And(
                AssignedContractor,
                Restrictions.And(
                    NotCancelled,
                Restrictions.And(
                NotCompleted,
                Restrictions.Or(
                    Restrictions.Eq("MarkoutRequirement.Id", (int)MarkoutRequirementEnum.Emergency),
                    Restrictions.And(
                        Restrictions.Or(
                            Restrictions.Not(Restrictions.Eq("MarkoutRequirement.Id", (int)MarkoutRequirementEnum.Routine)),
                            Subqueries.Exists(ValidMarkoutIds)
                        ),
                        Restrictions.Or(
                            Restrictions.Or(
                                Restrictions.Not(Restrictions.Eq("StreetOpeningPermitRequired", true)),
                                EmergencyPriority
                                ),
                            Subqueries.Exists(ValidPermitIds)
                        )
                    )
                )
            )));
        }

        public ICriterion GetFinalizationCriteria()
        {
            return
                Restrictions.And(
                    AssignedContractor,
                    Restrictions.And(
                        NotCancelled,

                    Restrictions.And(
                        NotApproved,
                        Restrictions.Or(
                            Subqueries.Exists(ValidCrewAssignments),
                            EmergencyPriority
                        ))));
        }

        public ICriterion GetGeneralCriteria()
        {
            return AssignedContractor;
        }

        public WorkOrder FindSchedulingOrder(int id)
        {
            return SchedulingOrders.Add(base.GetIdEqCriterion(id)).UniqueResult<WorkOrder>();
        }

        public IEnumerable<WorkOrder> SearchGeneralOrders(IWorkOrderSearch search)
        {
            WorkOrder workOrder = null;
            var query = Session.QueryOver(() => workOrder);
            query.And(GetGeneralCriteria());

            if (search.DocumentType?.Any() == true)
            { 
                Document document = null;
                DataType dataType = null;
                var documents = QueryOver.Of<DocumentLink>()
                                         .JoinAlias(x => x.Document, () => document, JoinType.LeftOuterJoin)
                                         .JoinAlias(x => x.DataType, () => dataType, JoinType.LeftOuterJoin)
                                         .Where(dl =>
                                              dl.LinkedId == workOrder.Id &&
                                              dataType.TableName == WorkOrderMap.TABLE_NAME)
                                         .Where(Restrictions.In("DocumentType", search.DocumentType));
                
                query.WithSubquery.WhereExists(documents.Select(Projections.Constant(1)));
            }

            return Search(search, query);
        }

        public IEnumerable<WorkOrder> SearchPlanningOrders(IWorkOrderSearch search)
        {
            return Search(search, Criteria.Add(GetPlanningCriteria()));
        }

        public IEnumerable<WorkOrder> SearchSchedulingOrders(IWorkOrderSearch search)
        {
            return Search(search, Criteria.Add(GetSchedulingCriteria()));
        }

        public IEnumerable<WorkOrder> SearchFinalizationOrders(IWorkOrderSearch search)
        {
            return Search(search, Criteria.Add(GetFinalizationCriteria()));
        }

        public IEnumerable<WorkOrder> GetByTownIdForServices(int townId)
        {
            return (from wo in base.Linq
                where wo.Town.Id == townId &&
                      (wo.AssetType.Id == AssetType.Indices.SERVICE ||
                       wo.AssetType.Id == AssetType.Indices.SEWER_LATERAL)
                select new WorkOrder {Id = wo.Id}).OrderByDescending(x => x.Id);
        }

        #endregion
    }
}
