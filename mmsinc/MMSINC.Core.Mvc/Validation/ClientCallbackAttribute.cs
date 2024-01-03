using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MMSINC.Validation
{
    /// <summary>
    /// Validator for calling a specific client-side javascript function.
    /// This is primarily for times where client-side validation is needed
    /// but there's no reason to make a full on custom validator for it.
    /// </summary>
    public class ClientCallbackAttribute : ValidationAttribute, IClientValidatable
    {
        #region Properties

        public string CallbackMethod { get; set; }

        #endregion

        #region Constructor

        public ClientCallbackAttribute(string callbackMethod)
        {
            CallbackMethod = callbackMethod;
        }

        #endregion

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ValidationType = "clientcallback";
            rule.ValidationParameters.Add("method", CallbackMethod);
            rule.ErrorMessage = ErrorMessage;
            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // This is always to be successful. There's no server-side part for this.
            return ValidationResult.Success;
        }
    }
}
