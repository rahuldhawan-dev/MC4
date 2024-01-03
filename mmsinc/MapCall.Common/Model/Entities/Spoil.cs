using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Spoil : IEntity, IValidatableObject
    {
        #region Consts

        public struct Display
        {
            public const string Quantity = "Quantity (CY)";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [View(Display.Quantity)]
        public virtual decimal Quantity { get; set; }

        #endregion

        #region Associations

        public virtual WorkOrder WorkOrder { get; set; }
        public virtual SpoilStorageLocation SpoilStorageLocation { get; set; }

        #endregion

        #region Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
