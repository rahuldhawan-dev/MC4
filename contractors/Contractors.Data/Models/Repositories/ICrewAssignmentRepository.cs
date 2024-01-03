using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface ICrewAssignmentRepository : IRepository<CrewAssignment>
    {
        /// <summary>
        /// Returns the CrewAssignments for a crew on a given date. 
        /// </summary>
        IEnumerable<CrewAssignment> GetAllForCrewByDate(int crewId, DateTime date);

        /// <summary>
        /// Returns the CrewAssignments for a crew within a given date range. 
        /// </summary>
        IEnumerable<CrewAssignment> GetAllForCrewByDateRange(int crewId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns the percentage of work for a crew for each day of the given month.
        /// yearMonth can be any 
        /// </summary>
        Dictionary<DateTime, decimal> GetCrewTimePercentagesByMonth(int crewId, DateTime yearMonth);

        /// <summary>
        /// Gets all the crew assignments for a WorkOrder based on the WorkOrder's AssociatedContractor.
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        IEnumerable<CrewAssignment> GetAllForWorkOrderAssignedContractor(int workOrderId);

        CrewAssignment Create(WorkOrder workOrder, Crew crew, DateTime assignedFor);
    }
}