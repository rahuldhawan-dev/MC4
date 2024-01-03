using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Crew : IValidatableObject, IThingWithOperatingCenter
    {
        #region Constants

        public const string DESCRIPTION = "Name",
                            AVAILABILITY = "Availability (hours)",
                            OPERATING_CENTER = "Operating Center";

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual Contractor Contractor { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        [Required, View(DESCRIPTION)]
        public virtual string Description { get; set; }

        [Required, View(AVAILABILITY, MMSINC.Utilities.FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal Availability { get; set; }

        public virtual bool Active { get; set; }

        public virtual IList<CrewAssignment> CrewAssignments { get; set; }

        public virtual string Display => new CrewDisplayItem {
            Description = Description, OperatingCenterCode = OperatingCenter?.OperatingCenterCode,
            OperatingCenterName = OperatingCenter?.OperatingCenterName
        }.Display;

        #endregion

        #region Constructors

        public Crew()
        {
            CrewAssignments = new List<CrewAssignment>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        public virtual int GetMaxPriorityByDate(DateTime date)
        {
            var startDate = date.Date;
            var endDate = date.Date.AddDays(1);
            return (from ass in CrewAssignments
                    where startDate <= ass.AssignedFor
                          && ass.AssignedFor < endDate
                    select ass.Priority).MaxOrDefault();
        }

        #endregion
    }

    public class CrewDisplayItem : DisplayItem<Crew>
    {
        public string Description { get; set; }

        [SelectDynamic("OperatingCenterCode", Field = "OperatingCenter")]
        public string OperatingCenterCode { get; set; }

        [SelectDynamic("OperatingCenterName", Field = "OperatingCenter")]
        public string OperatingCenterName { get; set; }

        public override string Display => $"{OperatingCenterCode} {OperatingCenterName} - {Description}";
    }
}
