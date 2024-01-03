using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Validation;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ContractorOverrideLaborCost : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual Contractor Contractor { get; set; }
        public virtual ContractorLaborCost ContractorLaborCost { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}"), RequiredWhen("Percentage", ComparisonType.EqualTo, null)]
        public virtual decimal? Cost { get; set; }

        [DisplayFormat(DataFormatString = "{0:p}"), RequiredWhen("Cost", ComparisonType.EqualTo, null)]
        [Range(1, 100)]
        public virtual int? Percentage { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime EffectiveDate { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
