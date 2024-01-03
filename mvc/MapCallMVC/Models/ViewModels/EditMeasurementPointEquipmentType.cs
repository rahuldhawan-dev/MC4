using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class EditMeasurementPointEquipmentType : MeasurementPointEquipmentTypeViewModel
    {
        #region Constructors

        public EditMeasurementPointEquipmentType(IContainer container) : base(container) { }

        #endregion
        
        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            => base.Validate(validationContext).Concat(Validate());

        #endregion
        
        #region Private Methods
        
        private IEnumerable<ValidationResult> Validate()
        {
            var repo = _container.GetInstance<IMeasurementPointEquipmentTypeRepository>();
            var model = repo.Find(Id);
            
            var isInUse = repo.IsCurrentlyInUse(Id, EquipmentType);

            if (isInUse && IsModifyingBusinessCriticalFields(model))
            {
                yield return new ValidationResult(
                    $"Certain fields on this record cannot be updated, as this Measurement Point is already being used.");
            }
        }

        /// <summary>
        /// This tests for a change in any field that should not be editable once a Measurement Point is in use by a ProductionWorkOrder.
        /// </summary>
        /// <param name="existingModel"></param>
        /// <returns></returns>
        private bool IsModifyingBusinessCriticalFields(MeasurementPointEquipmentType existingModel) =>
            Category != existingModel.Category ||
            UnitOfMeasure != existingModel.UnitOfMeasure.Id ||
            Position != existingModel.Position ||
            Min != existingModel.Min ||
            Max != existingModel.Max;

        #endregion
    }
}
