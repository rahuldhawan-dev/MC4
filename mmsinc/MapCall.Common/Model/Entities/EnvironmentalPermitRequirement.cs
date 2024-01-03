using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Validation;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalPermitRequirement : IEntity, IValidatableObject
    {
        #region Constants

        public struct StringLengths
        {
            public const int
                COMMUNICATION_EMAIL = 254,
                COMMUNICATION_LINK = 1024,
                REPORT_DATA_STORAGE_LOCATION = 50,
                REPORT_CREATION_INSTRUCTIONS = 50,
                REQUIREMENT = 50;
        }

        #endregion

        public virtual int Id { get; set; }
        public virtual EnvironmentalPermit EnvironmentalPermit { get; set; }
        public virtual EnvironmentalPermitRequirementType RequirementType { get; set; }
        public virtual EnvironmentalPermitRequirementValueUnit ValueUnit { get; set; }
        public virtual EnvironmentalPermitRequirementValueDefinition ValueDefinition { get; set; }
        public virtual EnvironmentalPermitRequirementTrackingFrequency TrackingFrequency { get; set; }
        public virtual EnvironmentalPermitRequirementReportingFrequency ReportingFrequency { get; set; }
        public virtual Employee ProcessOwner { get; set; }
        public virtual Employee ReportingOwner { get; set; }
        public virtual CommunicationType CommunicationType { get; set; }
        public virtual string Requirement { get; set; }
        public virtual string ReportingFrequencyDetails { get; set; }
        public virtual string ReportDataStorageLocation { get; set; }
        public virtual string ReportCreationInstructions { get; set; }
        public virtual string ReportSendTo { get; set; }

        public virtual string Notes { get; set; }
        public virtual string CommunicationEmail { get; set; }
        public virtual string CommunicationLink { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
