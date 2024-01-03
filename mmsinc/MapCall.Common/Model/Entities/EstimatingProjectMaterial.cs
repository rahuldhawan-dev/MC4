using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EstimatingProjectMaterial : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual EstimatingProject EstimatingProject { get; set; }
        public virtual Material Material { get; set; }
        public virtual AssetType AssetType { get; set; }
        public virtual int Quantity { get; set; }
        public virtual int MaterialCost { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal TotalCost => Quantity * MaterialCost;

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
