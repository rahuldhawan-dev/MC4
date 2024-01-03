using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OneCallMarkoutAudit : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }

        [Required]
        public virtual DateTime DateTransmitted { get; set; }

        [Required]
        public virtual DateTime DateReceived { get; set; }

        public virtual bool? Success { get; set; }

        public virtual IList<OneCallMarkoutAuditTicketNumber> TicketNumbers { get; set; }

        public virtual string FullText { get; set; }

        #endregion

        #region Constructors

        public OneCallMarkoutAudit()
        {
            TicketNumbers = new List<OneCallMarkoutAuditTicketNumber>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
