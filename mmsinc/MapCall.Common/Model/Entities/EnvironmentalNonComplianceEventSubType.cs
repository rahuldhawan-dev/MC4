using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalNonComplianceEventSubType : IEntityLookup
    {
        public virtual int Id { get; set; }
        public virtual String Description { get; set; }
        public virtual EnvironmentalNonComplianceEventType EnvironmentalNonComplianceEventType { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
