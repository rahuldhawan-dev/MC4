using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EmployeeAssignment : IEntity
    {
        #region Constants

        public const string CANNOT_BE_STARTED =
            "Work cannot be started. Please ensure you have met all the prerequisite conditions.";
        public const string MUST_BE_TWO_DECIMAL_PLACES = "Must be a number with maximum of 2 decimal places";

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        [View(FormatStyle.Date)]
        public virtual DateTime AssignedOn { get; set; }
        [View(FormatStyle.Date)]
        public virtual DateTime AssignedFor { get; set; }
        public virtual Employee AssignedTo { get; set; }
        public virtual Employee AssignedBy { get; set; }
        public virtual DateTime? DateStarted { get; set; }
        public virtual DateTime? DateEnded { get; set; }
        [Obsolete("There's nothing in the database for this besides an empty table. This should probably go away.")]
        public virtual IList<Employee> Employees { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual bool OrderIsOpen { get; }
        [View("Hours Worked", FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal HoursWorked { get; set; }

        #region Logical

        public virtual float? EmployeesOnJob => Employees?.Count + 1 ?? 1; //= +1 for the assigned employee

        public virtual TimeSpan? TimeToComplete =>
            (DateStarted == null || DateEnded == null) ? null : DateEnded - DateStarted;

        public virtual bool IsOpen => HasStarted && DateEnded == null;
        public virtual bool HasStarted => DateStarted != null;

        public virtual float? TotalManHours
        {
            get
            {
                if (TimeToComplete != null)
                {
                    return (DateStarted == null || DateEnded == null)
                        ? null
                        : TimeToComplete.Value.Hours * EmployeesOnJob;
                }

                return null;
            }
        }

        public virtual bool CanBeStarted
        {
            get
            {
                if (!ProductionWorkOrder.ProductionPreJobSafetyBriefs.Any(x => x.Workers.Any(y => y.Employee == AssignedTo)) 
                    && (ProductionWorkOrder.OrderType.Id == OrderType.Indices.CORRECTIVE_ACTION_20 || ProductionWorkOrder.OrderType.Id == OrderType.Indices.OPERATIONAL_ACTIVITY_10))
                {
                    return false;
                }
                if (ProductionWorkOrder.ProductionWorkOrderProductionPrerequisites.Any(x => x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT && !x.SkipRequirement) 
                    && !ProductionWorkOrder.LockoutForms.Any())
                {
                    return false;
                }

                if (ProductionWorkOrder.ProductionWorkOrderProductionPrerequisites.Any(x => x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.IS_CONFINED_SPACE && !x.SkipRequirement)
                    && !ProductionWorkOrder.ConfinedSpaceForms.Any(x => x.IsCompleted))
                {
                    return false;
                }

                /*
                 * A production work order with a red tag permit prerequisite may or may not require the user to actually create red tag permits
                 *  1. Do we have any prerequisites of type red tag permit and are not 'skip requirement'?
                 *  2. Yes, we have a red tag prerequisite, so - does this work order need red tag permit authorizations?
                 *  3. Yes, this work order needs authorization, so - does this work order not have a red tag permit filled out yet
                 *  4. Yes, the employee cannot begin work
                 */
                if (ProductionWorkOrder.IsEligibleForRedTagPermit &&
                    ProductionWorkOrder.RedTagPermit == null && 
                    (!ProductionWorkOrder.NeedsRedTagPermitAuthorization.HasValue || ProductionWorkOrder.NeedsRedTagPermitAuthorization.Value))
                {
                    return false;
                }

                if (ProductionWorkOrder.ProductionWorkOrderProductionPrerequisites.Any(x => x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.PRE_JOB_SAFETY_BRIEF && !x.SkipRequirement)
                    && !ProductionWorkOrder.ProductionPreJobSafetyBriefs.Any())
                {
                    return false;
                }

                return true;
            }
        }

        public virtual bool RequiredTankInspectionNotCompleted => ProductionWorkOrder.RequiresTankInspection && !ProductionWorkOrder.TankInspections.Any();

        #endregion

        #endregion

        #region Constructors

        public EmployeeAssignment()
        {
            Employees = new List<Employee>();
        }

        #endregion
    }
}
