using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FireDistrictTown : IEntity, IValidatableObject
    {
        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        [Required]
        public virtual bool IsDefault { get; set; }

        #endregion

        #region References

        [Required]
        public virtual Town Town { get; set; }

        [Required]
        public virtual State State { get; set; }

        [Required]
        public virtual FireDistrict FireDistrict { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
