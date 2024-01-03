using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class RemoveEquipmentCharacteristicFieldDropDownValue : ViewModel<EquipmentCharacteristicField>
    {
        #region Constants

        public struct ErrorMessages
        {
            public const string REMOVE_SAP_DROPDOWN_VALUES = "Drop Down Values on SAP Characteristics cannot be deleted.",
                                ALREADY_IN_USE = "The selected Drop Down Value could not be deleted. It is currently in use by an Equipment record.";
        }
        
        #endregion
        
        #region Constructors

        public RemoveEquipmentCharacteristicFieldDropDownValue(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, DoesNotAutoMap]
        public virtual int SelectedDropDownValue { get; set; }

        #endregion

        #region Exposed Methods

        public override EquipmentCharacteristicField MapToEntity(EquipmentCharacteristicField entity)
        {
            var item = _container.GetInstance<RepositoryBase<EquipmentCharacteristicField>>()
                                 .Find(Id)
                                 .DropDownValues
                                 .FirstOrDefault(x => x.Id == SelectedDropDownValue);

            entity.DropDownValues.Remove(item);
            return base.MapToEntity(entity);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            => base.Validate(validationContext).Concat(ValidateDropDownValueIsNotCurrentlyInUse());

        #endregion
        
        #region Private Methods

        private IEnumerable<ValidationResult> ValidateDropDownValueIsNotCurrentlyInUse()
        {
            var entity = _container.GetInstance<IRepository<EquipmentCharacteristicField>>().Find(Id);
            var isInUse = _container.GetInstance<IEquipmentRepository>()
                                    .IsCharacteristicDropDownValueCurrentlyInUse(entity, SelectedDropDownValue);
            if (entity.IsSAPCharacteristic)
            {
                yield return new ValidationResult(ErrorMessages.REMOVE_SAP_DROPDOWN_VALUES);
            }
            
            if (isInUse)
            {
                yield return new ValidationResult(ErrorMessages.ALREADY_IN_USE);
            }
        }
        
        #endregion
    }
}
