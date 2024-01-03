using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IMaintenancePlanRepository : IRepository<MaintenancePlan>
    {
        IEnumerable<ScheduledMaintenancePlan> GetOnlyScheduledMaintenancePlans();
        void TrimScheduledAssignmentsUpTo(DateTime date);
        void TrimScheduledAssignmentsUpToToday();
    }
    
    public class MaintenancePlanRepository : RepositoryBase<MaintenancePlan>, IMaintenancePlanRepository
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        private DateTime Today => _dateTimeProvider.GetCurrentDate().Date;

        #endregion

        #region Constructors

        public MaintenancePlanRepository(ISession session,
            IContainer container, 
            IDateTimeProvider dateTimeProvider) : base(session, container)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Gets all active/un-paused MaintenancePlans that are scheduled to generate a ProductionWorkOrder today.
        /// </summary>
        /// <remarks>This method will contact the DB at the time of being invoked. Enumerating the result will run each ScheduledMaintenancePlan through <see cref="ProductionWorkOrderFrequency.GetFrequencyDates(int, DateTime, DateTime)"/>
        /// to see if that plan is on-schedule for today.</remarks>
        /// <returns>An enumerable of ScheduledMaintenancePlans, which can be used to generate new ProductionWorkOrder records.</returns>
        public IEnumerable<ScheduledMaintenancePlan> GetOnlyScheduledMaintenancePlans()
        {
            var maintenancePlans =
                Where(x =>
                        x.IsActive
                        && !x.IsPlanPaused
                        && Today >= x.Start.Date)
                   .Select(plan => new ScheduledMaintenancePlan(plan))
                    
                    // I'd like to avoid this ToList() but since we need to check each plan against GetFrequencyDates, we need the .net objects 
                   .ToList();

            return maintenancePlans.Where(x => ProductionWorkOrderFrequency.GetFrequencyDates(x.ProductionWorkOrderFrequencyId, x.Start.Date, Today.AddDays(1)).Contains(Today));
        }

        /// <summary>
        /// Clear assignments on all maintenance plans that are scheduled before and up to (inclusive) the given day
        /// </summary>
        /// <param name="date"></param>
        public void TrimScheduledAssignmentsUpTo(DateTime date)
        {
            Session.Query<ScheduledAssignment>()
                   .Where(x => x.ScheduledDate.Date <= date.Date)
                   .Delete();
        }

        public void TrimScheduledAssignmentsUpToToday()
        {
            TrimScheduledAssignmentsUpTo(Today);
        }

        #endregion
    }
}