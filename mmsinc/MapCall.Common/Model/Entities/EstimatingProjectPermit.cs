using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EstimatingProjectPermit : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual EstimatingProject EstimatingProject { get; set; }
        public virtual PermitType PermitType { get; set; }
        public virtual int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal Cost { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal TotalCost => Quantity * Cost;

        public virtual AssetType AssetType { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
