using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MMSINC.Data
{
    public class RequiresConfirmationAttribute : ValidationAttribute, IClientValidatable
    {
        #region Exposed Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is bool && (bool)value)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(String.Format(ErrorMessageString, validationContext.DisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var rule = new ModelClientValidationRule {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiresconfirmation"
            };

            yield return rule;
        }

        #endregion
    }
}
