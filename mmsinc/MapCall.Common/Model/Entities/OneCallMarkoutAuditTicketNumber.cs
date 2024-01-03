using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OneCallMarkoutAuditTicketNumber : IEntity, IValidatableObject
    {
        public virtual string CDCCode { get; set; }
        public virtual int Id { get; set; }
        public virtual OneCallMarkoutAudit Audit { get; set; }
        public virtual OneCallMarkoutMessageType MessageType { get; set; }

        [Required]
        public virtual int RequestNumber { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
