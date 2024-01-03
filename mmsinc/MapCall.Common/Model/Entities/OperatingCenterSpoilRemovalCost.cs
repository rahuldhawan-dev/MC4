using MMSINC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OperatingCenterSpoilRemovalCost : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual int Cost { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion  
    }
}