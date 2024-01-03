using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CrewAssignment : IEntity, IValidatableObject
    {
        #region Constants

        public struct ModelErrors
        {
            public const string NO_SUCH_WORK_ORDER =
                                    "Work Order # {0} was not found to exist.",
                                INVALID_MARKOUT =
                                    "Work Order # {0} does not have a markout that will be valid on the chosen date.",
                                INVALID_PERMIT =
                                    "Work Order # {0} does not have a permit that will be valid on the chosen date.";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual Crew Crew { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        [Required, View(DisplayFormat = CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime AssignedOn { get; set; }

        [Required, View(DisplayFormat = CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime AssignedFor { get; set; }

        public virtual DateTime? DateStarted { get; set; }
        public virtual DateTime? DateEnded { get; set; }

        [Required]
        public virtual int Priority { get; set; }

        public virtual float? EmployeesOnJob { get; set; }

        public virtual User StartedBy { get; set; }

        #region Logical Fields

        public virtual TimeSpan? TimeToComplete => (DateStarted == null || DateEnded == null)
            ? null
            : DateEnded - DateStarted;

        public virtual float? TotalManHours => (DateStarted == null || DateEnded == null)
            ? null
            : TimeToComplete.Value.Hours * EmployeesOnJob;

        public virtual bool IsOpen => DateStarted.HasValue && !DateEnded.HasValue;

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime? StartTime => DateStarted;

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime? EndTime => DateEnded;

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return String.Format("{0} : {1} : {2}", Crew.Description, DateStarted, DateEnded);
        }

        #endregion
    }
}
