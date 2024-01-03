using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Contractors.Data.Models.Repositories;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public class CrewAssignmentStart : ViewModel<CrewAssignment>
    {
        #region Constants

        public struct ModelErrors
        {
            public const string NO_VALID_MARKOUT = "Cannot start work. A markout is required but no valid markout exists.",
                                NO_VALID_PERMIT = "Street Opening Permit Required but no valid permit exists",
                                ALREADY_STARTED = "Crew Assignment has already been started",
                                WORK_ORDER_ALREADY_COMPLETED = "Work order has already been completed.",
                                WORK_ORDER_DOESNT_EXIST = "Work order doesn't exist.";
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
            if (workOrder.CurrentMarkout.Markout != null &&
                // INC000000118985: Starting work should not change ExpirationDate if
                // WorkOrder.OperatingCenter.MarkoutsEditable == true.
                !workOrder.OperatingCenter.MarkoutsEditable)
            {
                workOrder.CurrentMarkout.Markout.ExpirationDate =
                    WorkOrdersWorkDayEngine.GetExpirationDate(
                        workOrder.CurrentMarkout.Markout.DateOfRequest.Value,
                        workOrder.MarkoutRequirement.MarkoutRequirementEnum,
                        true);
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var repository = _container.GetInstance<ICrewAssignmentRepository>();
            var assignment = repository.Find(Id);
            _container.BuildUp(assignment.WorkOrder);
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

                if (assignment.WorkOrder.StreetOpeningPermitRequired && assignment.WorkOrder.Priority.Id != (int)WorkOrderPriorityEnum.Emergency)
                {
                    if (!HasValidStreetOpeningPermit(assignment.WorkOrder.Id))
                    {
                        results.Add(
                            new ValidationResult(ModelErrors.NO_VALID_PERMIT));
                    }
                    else if (!assignment.AssignedFor.IsBetween(
                        assignment.WorkOrder.CurrentStreetOpeningPermit.
                            DateIssued.Value,
                        assignment.WorkOrder.CurrentStreetOpeningPermit.
                            ExpirationDate.Value))
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
            return MapCall.Common.Model.Repositories
                          .IWorkOrderRepositoryExtensions
                          .PlanningWorkOrderHasValidMarkoutForStartingCrewAssignment(
                               woRepo, workOrderId);
        }

        private bool HasValidStreetOpeningPermit(int workOrderId)
        {
            var woRepo = _container.GetInstance<IWorkOrderRepository>();
            return MapCall.Common.Model.Repositories
                          .IWorkOrderRepositoryExtensions
                          .PlanningWorkOrderHasValidStreetOpeningPermitForStartingCrewAssignment(
                               woRepo, workOrderId);
        }

        #endregion
    }

    public class CrewAssignmentEnd : ViewModel<CrewAssignment>
    {
        #region Constants

        public struct ModelErrors
        {
            public const string ALREADY_ENDED = "Crew Assignment has already been ended";
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

    public class CrewAssignmentCalendarSearch
    {
        [Required, DropDown]
        public int? Crew { get; set; }

        [Required, View(FormatStyle.Date)]
        public DateTime? Date { get; set; }

        public IEnumerable<CrewAssignment> AssignmentsForDate { get; set; }

        public Dictionary<DateTime, decimal> AvailabilityPercentagesForMonth { get; set; }
    }

    public class CrewAssignmentIndex : ViewModel<WorkOrder>
    {
        private readonly ICrewAssignmentRepository _crewAssignmentRepo;
        private readonly IDateTimeProvider _dateTimeProvider;

        //Set manually in Map(WorkOrder wo) 
        [AutoMap(MapDirections.None)]
        public int WorkOrder { get; set; }
        //Set manually in the controller action
        [AutoMap(MapDirections.None)]
        public bool IsFinalizationView { get; set; }
        public IEnumerable<CrewAssignmentIndexTableRowView> CrewAssignments { get; private set; }

        public CrewAssignmentIndex(IContainer container) : base(container)
        {
                _crewAssignmentRepo =
                    _container.GetInstance<ICrewAssignmentRepository>();
            _dateTimeProvider = _container.GetInstance<IDateTimeProvider>();
        }
        public override void Map(WorkOrder wo)
        {
                WorkOrder = wo.Id;

                // This is potentially a problem if 
                // a) The assigned contractor on a workorder is changed and
                // b) The previous assigned contractor assigned crews to something
                
                // Don't use the WorkOrder.CrewAssigments property is that could result in assignments
                // that were from a previous contractor.
                CrewAssignments = _crewAssignmentRepo.GetAllForWorkOrderAssignedContractor(WorkOrder).Select(c => new CrewAssignmentIndexTableRowView(_dateTimeProvider, c)).ToList();
        }
    }

    [StringLengthNotRequired("Readonly view")]
    public class CrewAssignmentIndexTableRowView
    {
        #region Constants

        public const string NO_NOTES = "No notes entered.";

        #endregion

        #region Fields

        protected readonly IDateTimeProvider _dateTimeProvider;

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
        public DateTime AssignedFor
        {
            get { return CrewAssignment.AssignedFor; }
        }

        [View(FormatStyle.Date)]
        public DateTime? AssignedOn
        {
            get
            {
                return CrewAssignment.AssignedOn;
            }
        }

        public string AssignedTo
        {
            get
            {
                return CrewAssignment.Crew.Description;
            }
        }

        public int CrewAssignmentPriority
        {
            get
            {
                return CrewAssignment.Priority;
            }
        }

        public float? EmployeesOnJob
        {
            get { return CrewAssignment.EmployeesOnJob; }
        }

        [View(FormatStyle.DateTimeWithoutSeconds)]
        public DateTime? EndTime
        {
            get { return CrewAssignment.DateEnded; }
        }

        public bool IsCompleted
        {
            get
            {
                return WorkOrderObj.DateCompleted.HasValue;
            }
        }

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
                    && (!WorkOrderObj.Markouts.Any(mo => mo.ReadyDate <= now && mo.ExpirationDate > now))) // Date must be Today.
                {
                    return false;
                }

                return !StartTime.HasValue;
            }
        }

        public bool CanSetEndTime => (!WorkOrderObj.CancelledAt.HasValue) && (StartTime.HasValue && !EndTime.HasValue);

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

        public string NotesTitle
        {
            get
            {
                // note: WorkDescription should never be null.
                return WorkOrderObj.WorkDescription.Description;
            }
        }

        [View(FormatStyle.DateTimeWithoutSeconds)]
        public DateTime? StartTime
        {
            get { return CrewAssignment.DateStarted; }
        }

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

        public string StreetNumber
        {
            get { return WorkOrderObj.StreetNumber; }
        }

        [View(FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public decimal TimeToCompletion
        {
            get
            {
                // note: WorkDescription should never be null.
                return WorkOrderObj.WorkDescription.TimeToComplete;
            }
        }

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

        public int WorkOrder
        {
            get { return CrewAssignment.WorkOrder.Id; }
        }

        public string WorkOrderPriority
        {
            get
            {
                // note: Priority should never be null.
                return WorkOrderObj.Priority.Description;
            }
        }

        /// <summary>
        /// Is the current contractor for the work order the same contractor 
        /// the crew belongs to.
        /// </summary>
        public bool ContractorsMatch
        {
            get
            {
                // Null check on AssignedContractor because not all work orders have assigned contractors.
                return WorkOrderObj.AssignedContractor?.Id == CrewAssignment.Crew.Contractor.Id;
            }
        }

        public int Id { get; set; }

        #endregion

        #region Constructor

        public CrewAssignmentIndexTableRowView(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public CrewAssignmentIndexTableRowView(IDateTimeProvider dateTimeProvider, CrewAssignment crewAssignment) : this(dateTimeProvider)
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

        #endregion

        #region Logical properties

        /// <summary>
        /// Gets whether the remaining time for assignments is less than zero.
        /// </summary>
        public bool IsOverCapacity { get { return Remaining < 0m; } }

        #endregion
    }

    public class CrewAssignmentPriorityUpdate
    {
        [Required, EntityMustExist(typeof(Crew))]
        public int? Crew { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public int[] CrewAssignments { get; set; }
    }
}