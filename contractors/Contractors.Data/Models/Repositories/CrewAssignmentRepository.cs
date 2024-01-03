using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class CrewAssignmentRepository : SecuredRepositoryBase<CrewAssignment, ContractorUser>, ICrewAssignmentRepository
    {
        #region Properties

        public override IQueryable<CrewAssignment> Linq
        {
            get
            {
                return base.Linq.Where(ca => ca.Crew.Contractor == CurrentUser.Contractor);
            }
        }

        // TODO: This is inefficient
        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .CreateAlias("Crew", "crew")
                    .CreateAlias("crew.Contractor", "contractor")
                    .Add(Restrictions.Eq("contractor.Id", CurrentUser.Contractor.Id));
            }
        }

        #endregion

        #region Constructor

        public CrewAssignmentRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<CrewAssignment> GetAllForCrewByDateRange(int crewId, DateTime startDate, DateTime endDate)
        {
            // We need to search for date this way because Sqlite sucks with nhibernate. -Ross
            return (from ass in Linq
                    where ass.Crew.Id == crewId
                          && startDate <= ass.AssignedFor
                          && ass.AssignedFor < endDate
                    select ass)
                    .Fetch(a => a.WorkOrder)
                    .ThenFetch(a => a.WorkDescription);
        }

        public IEnumerable<CrewAssignment> GetAllForCrewByDate(int crewId, DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);
            return GetAllForCrewByDateRange(crewId, startDate, endDate);
        }

        public Dictionary<DateTime, decimal> GetCrewTimePercentagesByMonth(int crewId, DateTime yearMonth)
        {
            var startDate = yearMonth.GetBeginningOfMonth();
            var endDate = startDate.AddMonths(1); // Because this is a < then operation, this'll match up everything to the last millisecond of the month.

            var boo = (from ass in GetAllForCrewByDateRange(crewId, startDate, endDate)
                       let jinkies = new
                                         {
                                             CrewAssignment = ass,
                                             ass.WorkOrder.WorkDescription.TimeToComplete,
                                             ass.Crew.Availability
                                         }
                       group jinkies by jinkies.CrewAssignment.AssignedFor.Date into assByDate
                       // Setting these as lets for the sake of readability.
                       let timeToComplete = assByDate.Sum(x => x.TimeToComplete)
                       // Only way to get the availability of the passed in crewId without
                       // first getting it from another repo or Session.Query<Crew>
                       let avail = assByDate.First().Availability
                       select new
                                  {
                                      Date = assByDate.Key,
                                      // We ain't want no divide by zero errors!
                                      Percentage = (timeToComplete <= 0 || avail <= 0 ? 0 : (timeToComplete / avail))
                                  });
           
            return boo.ToDictionary(b => b.Date, b => b.Percentage);
        }

        public IEnumerable<CrewAssignment> GetAllForWorkOrderAssignedContractor
            (int workOrderId)
        {
            return (from ca in Linq
                    where ca.WorkOrder.Id == workOrderId
                          && ca.WorkOrder.AssignedContractor == CurrentUser.Contractor
                    select ca);
        }

        public CrewAssignment Create(WorkOrder workOrder, Crew crew, DateTime assignedFor)
        {
            workOrder.ThrowIfNull("workOrder");
            crew.ThrowIfNull("crew");
            return Save(new CrewAssignment
            {
                WorkOrder = workOrder,
                Crew = crew,
                AssignedFor = assignedFor.Date,
                AssignedOn = DateTime.Now
            });
        }

        public override CrewAssignment Save(CrewAssignment entity)
        {
            if (entity.Priority == 0)
            {
                // need to inc that by 1 in order for things to work right.
                entity.Priority = 1 + entity.Crew.GetMaxPriorityByDate(entity.AssignedFor);
            }
            return base.Save(entity);
        }

        #endregion
    }
}
