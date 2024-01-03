using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentCharacteristicDropDownValue : IEntity, IValidatableObject
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual EquipmentCharacteristicField Field { get; set; }

        [Required]
        public virtual string Value { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
