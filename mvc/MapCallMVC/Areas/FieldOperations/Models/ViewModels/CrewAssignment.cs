using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using NHibernate;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CrewAssignmentCalendarSearch
    {
        #region Properties

        [Required, DropDown]
        public int? Crew { get; set; }

        [Required, View(FormatStyle.Date)]
        public DateTime? Date { get; set; }

        public IEnumerable<CrewAssignment> AssignmentsForDate { get; set; }

        public Dictionary<DateTime, decimal> AvailabilityPercentagesForMonth { get; set; }

        #endregion
    }

    public class SchedulingCrewAssignment : ViewModelSet<CrewAssignment>
    {
        #region Constants

        public struct ModelErrors
        {
            #region Constants

            public const string NO_WORK_ORDER_IDS_CHOSEN = "You must pick at least one work order to assign.",
                                NO_CREW_FOUND = "No such crew.",
                                NO_CREW_ASSIGNMENT_FOUND = "No such crew assignment",
                                NO_SUCH_WORK_ORDER = "One or more of the work orders chosen no longer exist",
                                INVALID_MARKOUT =
                                    "One or more of the work orders chosen does not have a markout that is valid on the scheduled date.",
                                INVALID_PERMIT =
                                    "One or more of the work orders chosen does not have a permit that is valid on the scheduled date.";

            #endregion
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

        public IEnumerable<WorkOrder> WorkOrders => Search.Results;

        public ISearchSet<WorkOrder> Search { get; set; }

        public int? OperatingCenter => WorkOrders.FirstOrDefault()?.OperatingCenter.Id;

        [Required, EntityMustExist(typeof(Crew), ErrorMessage = ModelErrors.NO_CREW_FOUND),
         DropDown("FieldOperations", "Crew", "ByOperatingCenterOrAll", DependsOn = nameof(OperatingCenter))]
        public int Crew { get; set; }

        [Required, AtLeastOne]
        public IList<int> WorkOrderIDs { get; set; }

        [Required, DateTimePicker]
        public DateTime? AssignFor { get; set; }

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
                  .GetInstance<IRepository<Crew>>()
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
                                               x.ExpirationDate?.Date > today.Date);
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
                        String.Format(CrewAssignment.ModelErrors.NO_SUCH_WORK_ORDER,
                            workOrderId)));
                    return results;
                }

                if (workOrder.MarkoutRequirement.MarkoutRequirementEnum ==
                    MarkoutRequirementEnum.Routine)
                {
                    var today = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
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
                                String.Format(CrewAssignment.ModelErrors.INVALID_PERMIT,
                                    workOrderId)));
                        return results;
                    }
                }
            }

            return results.Any() ? results : base.Validate(validationContext);
        }

        #endregion
    }

    public class CrewAssignmentStart : ViewModel<CrewAssignment>
    {
        #region Constants

        public struct ModelErrors
        {
            #region Constants

            public const string NO_VALID_MARKOUT =
                                    "Cannot start work. A markout is required but no valid markout exists.",
                                NO_VALID_PERMIT = "Street Opening Permit Required but no valid permit exists",
                                ALREADY_STARTED = "Crew Assignment has already been started",
                                WORK_ORDER_ALREADY_COMPLETED = "Work order has already been completed.",
                                WORK_ORDER_DOESNT_EXIST = "Work order doesn't exist.";

            #endregion
        }

        #endregion

        #region Constructors

        public CrewAssignmentStart(IContainer container) : base(container) { }

        #endregion

        #region Methods

        public override CrewAssignment MapToEntity(CrewAssignment entity)
        {
            entity.DateStarted = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            var workOrder = entity.WorkOrder;
            _container.BuildUp(workOrder);
            if (workOrder.CurrentMarkout.Markout != null &&
                // INC000000118985: Starting work should not change ExpirationDate if WorkOrder.OperatingCenter.MarkoutsEditable == true.
                !workOrder.OperatingCenter.MarkoutsEditable &&
                // 2020-09-29 duncanj - this will error if no markout is actually required
                workOrder.MarkoutRequired)
            {
                workOrder.CurrentMarkout.Markout.ExpirationDate =
                    WorkOrdersWorkDayEngine.GetExpirationDate(
                        workOrder.CurrentMarkout.Markout.DateOfRequest.Value,
                        workOrder.MarkoutRequirement.MarkoutRequirementEnum, true);
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var repository = _container.GetInstance<ICrewAssignmentRepository>();
            var assignment = repository.Find(Id);
            if (assignment.DateStarted != null)
            {
                results.Add(new ValidationResult(ModelErrors.ALREADY_STARTED));
            }

            // At this point in time, to verifiably check that a work order has been completed or not,
            // you have to get the work order from the crew assignment itself. It will not appear in
            // PlanningOrders. 

            else if (assignment.WorkOrder.DateCompleted.HasValue)
            {
                results.Add(new ValidationResult(ModelErrors.WORK_ORDER_ALREADY_COMPLETED));
            }

            else if (assignment.WorkOrder.MarkoutRequirement.Id == (int)MarkoutRequirementEnum.Emergency)
            {
                // noop
                // But why? -Ross 5/29/2018
            }

            else
            {
                // if we don't do this the call to `assignment.WorkOrder.CurrentMarkout` will error
                _container.BuildUp(assignment.WorkOrder);
                if (assignment.WorkOrder.MarkoutRequired)
                {
                    if (!HasValidMarkout(assignment.WorkOrder.Id))
                    {
                        results.Add(
                            new ValidationResult(ModelErrors.NO_VALID_MARKOUT));
                    }
                    else if (!assignment.AssignedFor.IsBetween(
                        assignment.WorkOrder.CurrentMarkout.ReadyDate.Value.Date,
                        assignment.WorkOrder.CurrentMarkout.ExpirationDate.Value.Date))
                    {
                        results.Add(
                            new ValidationResult(ModelErrors.NO_VALID_MARKOUT));
                    }
                }

                if (assignment.WorkOrder.StreetOpeningPermitRequired &&
                    assignment.WorkOrder.Priority.Id != (int)WorkOrderPriorityEnum.Emergency)
                {
                    if (!HasValidStreetOpeningPermit(assignment.WorkOrder.Id))
                    {
                        results.Add(
                            new ValidationResult(ModelErrors.NO_VALID_PERMIT));
                    }
                    else if (!assignment.AssignedFor.IsBetween(
                        assignment.WorkOrder.CurrentStreetOpeningPermit.DateIssued.Value,
                        assignment.WorkOrder.CurrentStreetOpeningPermit.ExpirationDate.Value))
                    {
                        results.Add(
                            new ValidationResult(ModelErrors.NO_VALID_PERMIT));
                    }
                }
            }

            return results;
        }

        private bool HasValidMarkout(int workOrderId)
        {
            var woRepo = _container.GetInstance<IWorkOrderRepository>();
            return woRepo.PlanningWorkOrderHasValidMarkoutForStartingCrewAssignment(workOrderId);
        }

        private bool HasValidStreetOpeningPermit(int workOrderId)
        {
            var woRepo = _container.GetInstance<IWorkOrderRepository>();
            return woRepo.PlanningWorkOrderHasValidStreetOpeningPermitForStartingCrewAssignment(workOrderId);
        }

        #endregion
    }

    public class CrewAssignmentEnd : ViewModel<CrewAssignment>
    {
        #region Constants

        public struct ModelErrors
        {
            #region Constants

            public const string ALREADY_ENDED = "Crew Assignment has already been ended";

            #endregion
        }

        #endregion

        #region Properties

        [Required, Min(1)]
        public virtual float? EmployeesOnJob { get; set; }

        #endregion

        #region Constructors

        public CrewAssignmentEnd(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override CrewAssignment MapToEntity(CrewAssignment entity)
        {
            base.MapToEntity(entity);
            entity.DateEnded = _container
                              .GetInstance<IDateTimeProvider>()
                              .GetCurrentDate();
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var repository = _container.GetInstance<ICrewAssignmentRepository>();
            if (repository.Find(Id).DateEnded != null)
                yield return new ValidationResult(ModelErrors.ALREADY_ENDED);
        }

        #endregion
    }

    public class CrewAssignmentIndex : ViewModel<WorkOrder>
    {
        #region Private Members

        private readonly ICrewAssignmentRepository _crewAssignmentRepo;
        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        //Set manually in Map(WorkOrder wo) 
        [AutoMap(MapDirections.None)]
        public int WorkOrder { get; set; }

        //Set manually in the controller action
        [AutoMap(MapDirections.None)]
        public bool IsFinalizationView { get; set; }

        public IEnumerable<CrewAssignmentIndexTableRowView> CrewAssignments { get; private set; }

        #endregion

        #region Constructors

        public CrewAssignmentIndex(IContainer container) : base(container)
        {
            _crewAssignmentRepo =
                _container.GetInstance<ICrewAssignmentRepository>();
            _dateTimeProvider = _container.GetInstance<IDateTimeProvider>();
        }

        #endregion

        #region Exposed Methods

        public override void Map(WorkOrder wo)
        {
            WorkOrder = wo.Id;

            CrewAssignments = _crewAssignmentRepo.Where(ca => ca.WorkOrder.Id == WorkOrder)
                                                 .Select(c => new CrewAssignmentIndexTableRowView(_dateTimeProvider, c))
                                                 .ToList();
        }

        #endregion
    }

    [StringLengthNotRequired("Readonly view")]
    public class CrewAssignmentIndexTableRowView
    {
        #region Constants

        public const string NO_NOTES = "No notes entered.";

        #endregion

        #region Private Members

        #region Fields

        protected readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #endregion

        #region Properties

        public CrewAssignment CrewAssignment { get; set; }

        // just here to shorten code to access it.
        public WorkOrder WorkOrderObj
        {
            get
            {
                // this would normally be done using _container.BuildUp(), but here that's never called
                // so if we don't do this we'll get exceptions in the CurrentMarkout property
                CrewAssignment.WorkOrder.DateTimeProvider = _dateTimeProvider;
                return CrewAssignment.WorkOrder;
            }
        }

        [View(FormatStyle.Date)]
        public DateTime AssignedFor => CrewAssignment.AssignedFor;

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public DateTime? AssignedOn => CrewAssignment.AssignedOn;

        public string AssignedTo => CrewAssignment.Crew.Description;

        public int CrewAssignmentPriority => CrewAssignment.Priority;

        public float? EmployeesOnJob => CrewAssignment.EmployeesOnJob;

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE)]
        public DateTime? EndTime => CrewAssignment.DateEnded;

        public bool IsCompleted => WorkOrderObj.DateCompleted.HasValue;

        public bool CanSetStartTime
        {
            get
            {
                if (WorkOrderObj.CancelledAt.HasValue)
                {
                    return false;
                }

                var now = _dateTimeProvider.GetCurrentDate();

                if (WorkOrderObj.MarkoutRequirement.Id != (int)MarkoutRequirementEnum.Emergency
                    && WorkOrderObj.MarkoutRequired
                    && (!WorkOrderObj.Markouts.Any(mo => mo.ReadyDate <= now && mo.ExpirationDate > now))) 
                {
                    return false;
                }

                return !StartTime.HasValue;
            }
        }

        public bool CanSetEndTime => (!WorkOrderObj.CancelledAt.HasValue) && (StartTime.HasValue && !EndTime.HasValue);

        public DateTime? MarkoutReadyDate
        {
            get
            {
                var markout = WorkOrderObj.CurrentMarkout;
                if (markout != null)
                {
                    return markout.ReadyDate;
                }

                return null;
            }
        }

        public DateTime? MarkoutExpirationDate
        {
            get
            {
                var markout = WorkOrderObj.CurrentMarkout;
                if (markout != null)
                {
                    return markout.ExpirationDate;
                }

                return null;
            }
        }

        public string NearestCrossStreetName
        {
            get
            {
                var street = WorkOrderObj.NearestCrossStreet;
                if (street != null)
                {
                    return street.FullStName;
                }

                return null;
            }
        }

        public string Notes
        {
            get
            {
                var notes = WorkOrderObj.Notes;
                return (!string.IsNullOrWhiteSpace(notes) ? notes : NO_NOTES);
            }
        }

        public string NotesTitle =>
            // note: WorkDescription should never be null.
            WorkOrderObj.WorkDescription.Description;

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE)]
        public DateTime? StartTime => CrewAssignment.DateStarted;

        public string StreetName
        {
            get
            {
                var street = WorkOrderObj.Street;
                if (street != null)
                {
                    return street.FullStName;
                }

                return null;
            }
        }

        public string StreetNumber => WorkOrderObj.StreetNumber;

        [View(FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public decimal TimeToCompletion =>
            // note: WorkDescription should never be null.
            WorkOrderObj.WorkDescription.TimeToComplete;

        public string Town
        {
            get
            {
                var t = WorkOrderObj.Town;
                return (t != null ? t.ShortName : null);
            }
        }

        public string TownSection
        {
            get
            {
                var t = WorkOrderObj.TownSection;
                return (t != null ? t.Name : null);
            }
        }

        public int WorkOrder => CrewAssignment.WorkOrder.Id;

        public string WorkOrderPriority =>
            // note: Priority should never be null.
            WorkOrderObj.Priority.Description;

        /// <summary>
        /// Is the current contractor for the work order the same contractor 
        /// the crew belongs to.
        /// </summary>
        public bool ContractorsMatch =>
            // Null check on AssignedContractor because not all work orders have assigned contractors.
            WorkOrderObj.AssignedContractor?.Id == CrewAssignment.Crew.Contractor.Id;

        public int Id { get; set; }

        #endregion

        #region Constructor

        public CrewAssignmentIndexTableRowView(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public CrewAssignmentIndexTableRowView(IDateTimeProvider dateTimeProvider, CrewAssignment crewAssignment) :
            this(dateTimeProvider)
        {
            crewAssignment.ThrowIfNull("crewAssignment");
            CrewAssignment = crewAssignment;
            Id = crewAssignment.Id;
        }

        #endregion
    }


    public class CrewAssignmentManage
    {
        #region Properties

        [Required, EntityMustExist(typeof(Crew))]
        public int? Crew { get; set; }

        [Required, View(FormatStyle.Date)]
        public DateTime? Date { get; set; }

        [StringLengthNotRequired]
        public string CrewDescription { get; set; }

        [View("Availability (hours)", FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public decimal Availability { get; set; }

        [View("Remaining (hours)", FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public decimal Remaining { get; set; }

        public IEnumerable<CrewAssignment> AssignmentsForDate { get; set; }

        #region Logical properties

        /// <summary>
        /// Gets whether the remaining time for assignments is less than zero.
        /// </summary>
        public bool IsOverCapacity => Remaining < 0m;

        #endregion

        #endregion
    }

    public class CrewAssignmentPriorityUpdate
    {
        #region Properties

        [Required, EntityMustExist(typeof(Crew))]
        public int? Crew { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public int[] CrewAssignments { get; set; }

        #endregion
    }
}