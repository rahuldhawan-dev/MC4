using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class RemoveMeasurementPointEquipmentType : ViewModel<EquipmentType>
    {
        #region Properties

        [DoesNotAutoMap]
        public virtual int MeasurementPointId { get; set; }

        #endregion

        #region Constructors

        public RemoveMeasurementPointEquipmentType(IContainer container) : base(container) { }
        
        #endregion

        #region Exposed Methods

        public override EquipmentType MapToEntity(EquipmentType entity)
        {
            entity.MeasurementPoints.RemoveSingle(x => x.Id == MeasurementPointId);
            return base.MapToEntity(entity);
        }
        
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            => base.Validate(validationContext).Concat(Validate());

        #endregion
        
        #region Private Methods
        
        private IEnumerable<ValidationResult> Validate()
        {
            var isInUse = _container.GetInstance<IMeasurementPointEquipmentTypeRepository>()
                                    .IsCurrentlyInUse(MeasurementPointId, Id);
            
            if (isInUse)
            {
                yield return new ValidationResult(
                    $"The Measurement Point with Id '{MeasurementPointId}' is already in use and cannot be deleted.");
            }
        }
        
        #endregion
    }
}
