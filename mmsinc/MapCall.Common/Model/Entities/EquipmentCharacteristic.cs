using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentCharacteristic : IEntity, IValidatableObject
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual Equipment Equipment { get; set; }

        [Required]
        public virtual EquipmentCharacteristicField Field { get; set; }

        [Required]
        public virtual string Value { get; set; }
        //public virtual int Order { get; set; }
        //public virtual bool IsActive { get; set; }

        public virtual string DisplayValue
        {
            get
            {
                switch (Field.FieldType.Description)
                {
                    case "DropDown":
                        try
                        {
                            return Field.DropDownValues.Single(v => v.Id.ToString() == Value).Value;
                        }
                        catch (InvalidOperationException e)
                        {
                            throw new Exception($"Error loading drop down value with id '{Value}'.", e);
                        }
                    case "Currency":
                        return $"{Convert.ToDecimal(Value):C}";
                    default:
                        return Value;
                }
            }
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Field.ValueIsValid(Value))
            {
                yield return new ValidationResult($"Value '{Value}' is not valid for field '{Field.FieldName}'.");
            }
        }
    }
}
