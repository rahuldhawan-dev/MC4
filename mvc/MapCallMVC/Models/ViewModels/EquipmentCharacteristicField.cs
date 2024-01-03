using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateEquipmentCharacteristicField : ViewModel<EquipmentCharacteristicField>
    {
        #region Properties

        [EntityMap("EquipmentType"), EntityMustExist(typeof(EquipmentType)), Secured]
        public virtual int EquipmentType { get; set; }
        [DropDown, Required, EntityMustExist(typeof(EquipmentCharacteristicFieldType)), EntityMap("FieldType")]
        [ClientCallback("EquipmentCharacteristicField.validateDropDownValues", ErrorMessage = "Fields with type 'DropDown' must have at least one drop down value.")]
        public virtual int? FieldType { get; set; }
        [Required]
        public virtual string FieldName { get; set; }
        [Required]
        public virtual bool? Required { get; set; }
        [Required]
        public virtual bool? IsSAPCharacteristic { get; set; }
        [UIHint("StringArray")]
        public virtual string[] DropDownValues { get; set; }
        public bool? IsActive { get; set; }
        public int? Order { get; set; }
        [StringLength(100)]
        public virtual string Description { get; set; }
        #endregion

        #region Constructors


        public CreateEquipmentCharacteristicField(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override EquipmentCharacteristicField MapToEntity(EquipmentCharacteristicField entity)
        {
            DropDownValues = DropDownValues ?? Array.Empty<string>();

            foreach (var value in DropDownValues)
            {
                entity.DropDownValues.Add(new EquipmentCharacteristicDropDownValue {Value = value, Field = entity});
            }

            return base.MapToEntity(entity);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (typeof(Equipment).GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance)
                    .Any(p => p.Name == FieldName))
            {
                return base.Validate(validationContext)
                    .MergeWith(new[] {
                        new ValidationResult(string.Format("Field named '{0}' cannot be created because Equipment already has a field with that name.", FieldName))
                    });
            }

            var equipmentType = _container.GetInstance<IEquipmentTypeRepository>().Find(EquipmentType);
            if (equipmentType.CharacteristicFields.Any(characteristicField => characteristicField.FieldName == FieldName))
            {
                return base.Validate(validationContext)
                    .MergeWith(new[] {
                        new ValidationResult($"Field named {FieldName} already exists."),
                    });
            }

            var fieldType =
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<EquipmentCharacteristicFieldType>>()
                    .Find(FieldType.Value);

            if (fieldType.Description == "DropDown" && (DropDownValues == null || !DropDownValues.Any()))
            {
                return
                    base.Validate(validationContext)
                        .MergeWith(new[] {
                            new ValidationResult(
                                "Field Type 'DropDown' requires at least one Drop Down Value to be entered.")
                        });
            }

            return base.Validate(validationContext);
        }

        #endregion
    }
}