using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OperatingCenterStockedMaterial : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Material Material { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal? Cost { get; set; }

        #region Logical Properties

        public virtual string OldPartNumber => Material.OldPartNumber;
        public virtual string PartNumber => Material.PartNumber;
        public virtual string Description => Material.Description;
        public virtual string Size => Material.Size;

        public virtual bool IsActive => Material.IsActive;

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal? Price => Cost * 1.25m;

        public virtual bool DoNotOrder => Material.DoNotOrder;

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return $"{OperatingCenter} - {Material.FullDescription}";
        }

        #endregion
    }
}
