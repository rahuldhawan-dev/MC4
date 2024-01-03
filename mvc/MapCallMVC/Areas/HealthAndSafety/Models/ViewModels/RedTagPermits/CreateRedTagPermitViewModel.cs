using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits
{
    public class CreateRedTagPermitViewModel : RedTagPermitViewModel
    {
        #region Constants

        public const string EQUIPMENT_IMPAIRED_ON_DATE_CANNOT_BE_BEFORE_TODAY = "EquipmentImpairedOn date cannot be before today.";

        #endregion

        #region Properties

        [Required,
         DateTimePicker,
         ClientCallback("RedTagPermit.validateEquipmentImpairedOn", ErrorMessage = EQUIPMENT_IMPAIRED_ON_DATE_CANNOT_BE_BEFORE_TODAY)]
        public override DateTime? EquipmentImpairedOn { get; set; }

        #endregion

        #region Constructors

        public CreateRedTagPermitViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateThatEquipmentImpairedOnIsNotBeforeToday()
        {
            if (EquipmentImpairedOn.HasValue &&
                EquipmentImpairedOn.Value < _container.GetInstance<IDateTimeProvider>().GetCurrentDate().Date.AddSeconds(-1))
            {
                yield return new ValidationResult(EQUIPMENT_IMPAIRED_ON_DATE_CANNOT_BE_BEFORE_TODAY);
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateThatEquipmentImpairedOnIsNotBeforeToday());
        }

        #endregion
    }
}
