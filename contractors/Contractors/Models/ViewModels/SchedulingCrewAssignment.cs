using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using NHibernate;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public class SchedulingCrewAssignment : ViewModelSet<CrewAssignment>
    {
        #region Constants

        public struct ModelErrors
        {
            public const string NO_WORK_ORDER_IDS_CHOSEN = "You must pick at least one work order to assign.",
                                NO_CREW_FOUND = "No such crew.",
                                NO_CREW_ASSIGNMENT_FOUND = "No such crew assignment", 
                                NO_SUCH_WORK_ORDER = "One or more of the work orders chosen no longer exist",
                                INVALID_MARKOUT = "One or more of the work orders chosen does not have a markout that is valid on the scheduled date.",
                                INVALID_PERMIT = "One or more of the work orders chosen does not have a permit that is valid on the scheduled date.";
        }

        #endregion
        
        #region Private Members

        private readonly IList<CrewAssignment> _items;

        #endregion

        #region Properties

        public override IEnumerable<CrewAssignment> Items
        {
            get
            {
                _items.Clear();
                var session = _container.GetInstance<ISession>();
                foreach (var wo in WorkOrderIDs)
                {
                    _items.Add(new CrewAssignment {
                        WorkOrder = session.Load<WorkOrder>(wo),
                        AssignedFor = AssignFor.Value,
                        AssignedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                        Crew = session.Load<Crew>(Crew)
                    });
                }
                return _items;
            }
        }

        public IEnumerable<WorkOrder> WorkOrders
        {
            get { return Search.Results; } 
        }

        public ISearchSet<WorkOrder> Search { get; set; }

        #region Table Properties

        [Required, DropDown, EntityMustExist(typeof(Crew), ErrorMessage = ModelErrors.NO_CREW_FOUND)]
        public int Crew { get; set; }

        [Required, AtLeastOne]
        public IList<int> WorkOrderIDs { get; set; }

        [Required]
        public DateTime? AssignFor { get; set; }

        #endregion

        #endregion

        #region Constructors

        // used going into the controller
        public SchedulingCrewAssignment(IContainer container) : base(container)
        {
            _items = new List<CrewAssignment>();
            WorkOrderIDs = WorkOrderIDs ?? new List<int>();
        }

        #endregion

        #region Private Methods

        private Crew GetCrew(int crewId)
        {
            return _container
                .GetInstance<MMSINC.Data.NHibernate.IRepository<Crew>>()
                .Find(crewId);
        }

        #endregion

        #region Exposed Methods

        public static bool MarkoutsAreValidForScheduling(
            WorkOrder order,
            DateTime assignedFor,
            DateTime today)
        {
            if (order.MarkoutRequirement.Id != (int)MarkoutRequirement.Indices.ROUTINE)
            {
                return true;
            }

            if (order.OperatingCenter.State.Abbreviation == "NJ")
            {
                return order.Markouts.Any(x => x.ReadyDate?.Date <= assignedFor.Date &&
                                               x.ExpirationDate?.Date >= assignedFor.Date &&
                                               x.ExpirationDate?.Date > today);
            }

            return order.Markouts.Any(x => x.ReadyDate?.Date <= assignedFor.Date &&
                                           x.ExpirationDate >= assignedFor);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            
            if (WorkOrderIDs.Count == 0)
            {
                results.Add(new ValidationResult(ModelErrors.NO_WORK_ORDER_IDS_CHOSEN));
                return results;
            }

            var crew = GetCrew(Crew);
            if (crew == null)
            {
                results.Add(new ValidationResult(ModelErrors.NO_CREW_FOUND));
                return results;
            }

            foreach (var workOrderId in WorkOrderIDs)
            {
                var workOrder = _container
                               .GetInstance<IWorkOrderRepository>()
                               .FindSchedulingOrder(workOrderId);

                if (workOrder == null)
                {
                    results.Add(new ValidationResult(
                        string.Format(CrewAssignment.ModelErrors.NO_SUCH_WORK_ORDER,
                            workOrderId)));
                    return results;
                }
                _container.BuildUp(workOrder);

                if (workOrder.MarkoutRequirement.MarkoutRequirementEnum ==
                    MarkoutRequirementEnum.Routine)
                {
                    var today = _container.GetInstance<IDateTimeProvider>().GetCurrentDate().Date;
                    if (!MarkoutsAreValidForScheduling(workOrder, AssignFor.Value, today))
                    {
                        results.Add(
                            new ValidationResult(
                                string.Format(CrewAssignment.ModelErrors.INVALID_MARKOUT,
                                    workOrderId)));
                        return results;
                    }
                }

                if (workOrder.StreetOpeningPermitRequired &&
                    workOrder.Priority.WorkOrderPriorityEnum !=
                    WorkOrderPriorityEnum.Emergency)
                {
                    if (workOrder.CurrentStreetOpeningPermit == null
                        || workOrder.CurrentStreetOpeningPermit.DateIssued ==
                        null
                        || workOrder
                          .CurrentStreetOpeningPermit.ExpirationDate == null
                        || !AssignFor.Value.IsBetween(
                            workOrder
                               .CurrentStreetOpeningPermit.DateIssued.Value,
                            workOrder
                               .CurrentStreetOpeningPermit.ExpirationDate
                               .Value))
                    {
                        results.Add(
                            new ValidationResult(
                                string.Format(CrewAssignment.ModelErrors.INVALID_PERMIT,
                                    workOrderId)));
                        return results;
                    }
                }
            }

            return results.Any() ? results : base.Validate(validationContext);
        }

    	#endregion
    }
}