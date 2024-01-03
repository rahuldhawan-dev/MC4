using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Models.ViewModels 
{
    public class EditEquipmentCharacteristicField : ViewModel<EquipmentCharacteristicField>
    {
        #region Constants

        public struct ErrorMessages
        {
            public const string EDIT_SAP_DROPDOWN_VALUES = "Drop Down Values cannot be edited on an SAP Characteristic.";
        }
        
        #endregion
        
        #region Constructors

        public EditEquipmentCharacteristicField(IContainer container) : base(container) { }

        #endregion

        #region Private Members

        private EquipmentCharacteristicField _displayEquipmentCharacteristicField;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public EquipmentCharacteristicField DisplayEquipmentCharacteristicField
        {
            get => _displayEquipmentCharacteristicField 
                   ?? (_displayEquipmentCharacteristicField = _container.GetInstance<IRepository<EquipmentCharacteristicField>>().Find(Id));
            set => _displayEquipmentCharacteristicField = value;
        }

        public bool? IsActive { get; set; }
        public int? Order { get; set; }
        
        [StringLength(100)]
        public string Description { get; set; }

        [UIHint("StringArray"), View("New Drop Down Values")]
        public virtual string[] DropDownValues { get; set; }
        
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
            => base.Validate(validationContext).Concat(ValidateDropDownValuesCanOnlyBeAddedIfSAPCharacteristicIsFalse());

        #endregion
        
        #region Private Methods

        private IEnumerable<ValidationResult> ValidateDropDownValuesCanOnlyBeAddedIfSAPCharacteristicIsFalse()
        {
            var hasDropDownValues = DropDownValues != null && DropDownValues.Any();
            if (hasDropDownValues && DisplayEquipmentCharacteristicField.IsSAPCharacteristic)
            {
                yield return new ValidationResult(ErrorMessages.EDIT_SAP_DROPDOWN_VALUES);
            }
        }
        
        #endregion
    }
}