using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentCharacteristicField : IEntity, IValidatableObject
    {
        #region Private Members

        private IEnumerable<SelectListItem> _options;

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        [Required]
        public virtual string FieldName { get; set; }

        public virtual bool Required { get; set; }
        public virtual bool IsSAPCharacteristic { get; set; }
        public virtual int? Order { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string Description { get; set; }

        #endregion

        #region References

        [Required]
        public virtual EquipmentType EquipmentType { get; set; }

        [Required]
        public virtual EquipmentCharacteristicFieldType FieldType { get; set; }

        public virtual IList<EquipmentCharacteristicDropDownValue> DropDownValues { get; set; }
        public virtual IList<EquipmentCharacteristic> EquipmentCharacteristics { get; set; }

        #endregion

        #region Logical Properties

        public virtual IEnumerable<SelectListItem> Options
        {
            get
            {
                return _options ??
                       (_options =
                           DropDownValues.Map<EquipmentCharacteristicDropDownValue, SelectListItem>(
                               v => new SelectListItem {Value = v.Id.ToString(), Text = v.Value}));
            }
        }

        public virtual string DisplayField => !string.IsNullOrWhiteSpace(Description) ? Description : FieldName;

        #endregion

        #endregion

        #region Constructors

        public EquipmentCharacteristicField()
        {
            DropDownValues = new List<EquipmentCharacteristicDropDownValue>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public virtual bool ValueIsValid(string value)
        {
            return value != null && (FieldType.Regex == null || FieldType.RegexObj.IsMatch(value));
        }

        #endregion
    }
}
