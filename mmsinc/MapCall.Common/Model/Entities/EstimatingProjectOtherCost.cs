using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EstimatingProjectOtherCost : IEntity, IValidatableObject
    {
        #region Constants

        public struct StringLengths
        {
            public const int DESCRIPTION = 250;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual EstimatingProject EstimatingProject { get; set; }

        public virtual int Quantity { get; set; }
        
        public virtual string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal Cost { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual Decimal TotalCost => Cost * Quantity;

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
