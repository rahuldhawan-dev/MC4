using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RecurringProjectEndorsement : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual RecurringProject RecurringProject { get; set; }

        [DisplayName("Employee")]
        public virtual User User { get; set; }

        public virtual EndorsementStatus EndorsementStatus { get; set; }
        public virtual DateTime EndorsementDate { get; set; }
        public virtual string Comment { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
