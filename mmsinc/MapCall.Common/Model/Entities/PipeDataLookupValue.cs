using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PipeDataLookupValue : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual PipeDataLookupType PipeDataLookupType { get; set; }

        public virtual string Description { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS)]
        public virtual decimal VariableScore { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS)]
        public virtual decimal PriorityWeightedScore { get; set; }

        public virtual bool IsEnabled { get; set; }
        public virtual bool IsDefault { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
