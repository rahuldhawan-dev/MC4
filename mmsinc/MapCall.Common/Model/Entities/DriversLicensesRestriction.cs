using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DriversLicensesRestriction : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual DriversLicense DriversLicense { get; set; }
        public virtual DriversLicenseRestriction DriversLicenseRestriction { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
