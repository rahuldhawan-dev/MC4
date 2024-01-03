using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EmployeeLink : IEntity, IValidatableObject, IEmployeeLink
    {
        #region Properties

        public virtual int Id { get; set; }

        [Required]
        public virtual Employee Employee { get; set; }

        [Required]
        public virtual DataType DataType { get; set; }

        [Required]
        public virtual int LinkedId { get; set; }

        [Required]
        public virtual DateTime LinkedOn { get; set; }

        [Required]
        public virtual string LinkedBy { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
