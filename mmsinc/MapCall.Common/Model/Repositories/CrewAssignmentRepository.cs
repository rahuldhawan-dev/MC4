using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class CrewAssignmentRepository : MapCallSecuredRepositoryBase<CrewAssignment>, ICrewAssignmentRepository
    {
        #region Fields

        protected readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public CrewAssignmentRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IDateTimeProvider dateTimeProvider) : base(session,
            container, authenticationService, roleRepo)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Properties

        public override RoleModules Role => ROLE;

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria
                               .CreateAlias("WorkOrder", "wo", JoinType.LeftOuterJoin)
                               .CreateAlias("wo.OperatingCenter", "oc", JoinType.LeftOuterJoin);

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var operatingCenterIds = GetUserOperatingCenterIds();
                    crit = crit.Add(Restrictions.In("oc.Id", operatingCenterIds));
                }

                return crit;
            }
        }

        public override IQueryable<CrewAssignment> Linq
        {
            get
            {
                var linq = base.Linq;

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var operatingCenterIds = GetUserOperatingCenterIds();
                    linq = linq.Where(x => operatingCenterIds.Contains(x.WorkOrder.OperatingCenter.Id));
                }

                return linq;
            }
        }

        #endregion

        public IEnumerable<CrewAssignment> OpenCompanyForcesCrewAssignments(int hours)
        {
            return (from ca in base.Linq
                    where !ca.DateEnded.HasValue &&
                          ca.DateStarted.HasValue &&
                          ca.DateStarted >= _dateTimeProvider.GetCurrentDate().AddHours(-hours) &&
                          ca.WorkOrder.AssignedContractor == null
                    select ca);
        }
    }

    public interface ICrewAssignmentRepository : IRepository<CrewAssignment>
    {
        IEnumerable<CrewAssignment> OpenCompanyForcesCrewAssignments(int hours);
    }

    public static class ICrewAssignmentRepositoryExtensions
    {
        public static Dictionary<DateTime, decimal> GetCrewTimePercentagesByMonth(this IRepository<CrewAssignment> that,
            int crewId, DateTime month)
        {
            var startDate = month.GetBeginningOfMonth();
            var endDate =
                startDate.AddMonths(
                    1); // Because this is a < then operation, this'll match up everything to the last millisecond of the month.

            var boo = (from ass in that.GetAllForCrewByDateRange(crewId, startDate, endDate).ToList()
                       let jinkies = new {
                           CrewAssignment = ass,
                           ass.WorkOrder.WorkDescription.TimeToComplete,
                           ass.Crew.Availability
                       }
                       group jinkies by jinkies.CrewAssignment.AssignedFor.Date
                       into assByDate
                           // Setting these as lets for the sake of readability.
                       let timeToComplete = assByDate.Sum(x => x.TimeToComplete)
                       // Only way to get the availability of the passed in crewId without
                       // first getting it from another repo or Session.Query<Crew>
                       let avail = assByDate.First().Availability
                       select new {
                           Date = assByDate.Key,
                           // We ain't want no divide by zero errors!
                           Percentage = (timeToComplete <= 0 || avail <= 0 ? 0 : (timeToComplete / avail))
                       });

            return boo.ToDictionary(b => b.Date, b => b.Percentage);
        }

        public static IQueryable<CrewAssignment> GetAllForCrewByDateRange(this IRepository<CrewAssignment> that,
            int crewId, DateTime startDate, DateTime endDate)
        {
            // We need to search for date this way because Sqlite sucks with nhibernate. -Ross
            return (from ass in that.Linq
                    where ass.Crew.Id == crewId
                          && startDate <= ass.AssignedFor
                          && ass.AssignedFor < endDate
                    select ass)
                  .Fetch(a => a.WorkOrder)
                  .ThenFetch(a => a.WorkDescription);
        }

        public static IQueryable<CrewAssignment> GetAllForCrewByDate(this IRepository<CrewAssignment> that, int crewId,
            DateTime date)
        {
            var startDate = date.Date;
            return GetAllForCrewByDateRange(that, crewId, startDate, startDate.AddDays(1));
        }
    }
}
