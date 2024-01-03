using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits
{
    public class EditRedTagPermitViewModel : RedTagPermitViewModel
    {
        #region Constants

        public const string EQUIPMENT_RESTORED_ON_DATE_CANNOT_BE_BEFORE_TODAY = "EquipmentRestoredOn date cannot be before today.";

        #endregion

        #region Constructors

        public EditRedTagPermitViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required,
         DateTimePicker,
         View(FormatStyle.DateTimeWithoutSeconds),
         CompareTo(nameof(EquipmentImpairedOn), ComparisonType.GreaterThan, TypeCode.DateTime, IgnoreNullValues = true)]
        public DateTime? EquipmentRestoredOn { get; set; }

        // TODO: Shouldn't this just be a normal required field? Its RequiredWhen is based on
        // a property that's always required. So logically this field should always be required.
        [DataType(DataType.MultilineText),
         StringLength(RedTagPermit.StringLengths.EQUIPMENT_RESTORED_ON_CHANGE_REASON),
         RequiredWhen(nameof(EquipmentRestoredOn),
             ComparisonType.NotEqualTo,
             null,
             ErrorMessage = "Change Reason is required.",
             FieldOnlyVisibleWhenRequired = true)]
        public string EquipmentRestoredOnChangeReason { get; set; }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateThatEquipmentRestoredOnIsNotBeforeToday()
        {
            var authServ = _container.GetInstance<IAuthenticationService<User>>();

            if (EquipmentRestoredOn.HasValue &&
                EquipmentRestoredOn.Value < _container.GetInstance<IDateTimeProvider>().GetCurrentDate().Date.AddSeconds(-1) &&
                !authServ.CurrentUserIsAdmin)
            {
                yield return new ValidationResult(EQUIPMENT_RESTORED_ON_DATE_CANNOT_BE_BEFORE_TODAY);
            }
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateThatEquipmentRestoredOnIsNotBeforeToday());
        }

        public override RedTagPermit MapToEntity(RedTagPermit entity)
        {
            entity = base.MapToEntity(entity);

            var productionWorkOrder = entity.ProductionWorkOrder;

            if (productionWorkOrder == null || 
                !productionWorkOrder.IsEligibleForRedTagPermit ||
                !productionWorkOrder.NeedsRedTagPermitAuthorization.GetValueOrDefault() ||
                !entity.EquipmentRestoredOn.HasValue)
            {
                return entity;
            }

            var prerequisite = productionWorkOrder.ProductionWorkOrderProductionPrerequisites
                                                  .Single(x => x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.RED_TAG_PERMIT);

            prerequisite.SatisfiedOn = _container.GetInstance<IDateTimeProvider>()
                                                 .GetCurrentDate();

            return entity;
        }

        #endregion
    }
}
