using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DriversLicensesEndorsement : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual DriversLicense DriversLicense { get; set; }
        public virtual DriversLicenseEndorsement DriversLicenseEndorsement { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
